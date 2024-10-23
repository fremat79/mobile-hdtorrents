using AngleSharp.Dom;
using HdTorrents.Types.CustomProcessor;
using HdTorrents.Types.Attributes;
using HdTorrents.Types.Interface;
using HdTorrents.Types.Models;
using System.Collections;
using System.Net;
using System.Reflection;

namespace HdTorrents.Biz.Helpers
{
    public class HDTorrentBuilderHelper
    {
        public static T? FromIElement<T>(IElement? element) where T : new()
        {
            if (element == null)
            {
                return default;
            }

            T result = new();

            var props = typeof(T).GetProperties()
             .Where(p => p.GetCustomAttributes(typeof(CssSelectorAttribute)).ToList().Count > 0);

            object? propertyValue;

            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes(typeof(CssSelectorAttribute));
                var formatter = prop.GetCustomAttribute(typeof(FormatterAttribute)) as FormatterAttribute;
                var fallback = prop.GetCustomAttribute(typeof(FallBackValue)) as FallBackValue;
               
                foreach (CssSelectorAttribute attr in attrs)
                {
                    switch (attr)
                    {
                        case TextContentSelector:
                            propertyValue = WebUtility.HtmlDecode(element.QuerySelector(attr.Selector)?.TextContent.ReplaceLineEndings(string.Empty).Trim().Replace("  ", string.Empty));
                            break;
                        case AttributeSelector:
                            var attributeS = ((AttributeSelector)attr);
                            var attributeName = attributeS.AttributeName;
                            var value = element?.GetAttribute(attributeName);                            
                            propertyValue = WebUtility.HtmlDecode(value) ?? string.Empty;
                            break;
                        case AttributeContentSelector:
                            var attribute = ((AttributeContentSelector)attr);
                            attributeName = attribute.AttributeName;
                            value = element.QuerySelector(attr.Selector)?.GetAttribute(attributeName);
                            if (string.IsNullOrEmpty(value))
                            {
                                value = attribute.DefaultValue;
                            }
                            propertyValue = WebUtility.HtmlDecode(value);
                            break;
                        case IntContentSelector:
                            var intString = WebUtility.HtmlDecode(element.QuerySelector(attr.Selector)?.TextContent.ReplaceLineEndings(string.Empty).Trim());
                            propertyValue = int.TryParse(intString, out int _result) ? _result : 0;
                            break;
                        case BoolContentSelector:
                            propertyValue = element.QuerySelector(attr.Selector) != null;
                            break;
                        case TorrentTypeCssConverter:
                            propertyValue = ((TorrentTypeCssConverter)attr).FromCssClasses(
                                string.Join(" ", element.QuerySelector(attr.Selector)?.ClassList ?? new EmptyTokenList()));
                            break;
                        case CollectionAttributeSelector:
                            var pt = prop.PropertyType;
                            IList? list = null;
                            if (pt.IsGenericType && pt.GetGenericTypeDefinition() == typeof(List<>))
                            {
                                var elements = element.QuerySelectorAll(attr.Selector);
                                if (elements.Length > 0)
                                {
                                    Type collectionElementType = pt.GetGenericArguments()[0];
                                    var mi = typeof(HDTorrentBuilderHelper).GetMethod("FromIElement", BindingFlags.Static | BindingFlags.Public);
                                    var genericMi = mi?.MakeGenericMethod(collectionElementType);

                                    var listType = typeof(List<>).MakeGenericType(collectionElementType);
                                    list = (IList?)Activator.CreateInstance(listType);

                                    foreach (var el in elements)
                                    {
                                        var collectionItem = Activator.CreateInstance(collectionElementType);
                                        list?.Add(genericMi?.Invoke(null, new object[] { el }));
                                    }
                                }
                            }
                            propertyValue = list;
                            break;
                        case CustomProcessingAttributeSelector:
                            var customProcessing = attr as CustomProcessingAttributeSelector;
                            var processor = Activator.CreateInstance(customProcessing.Processor) as ICustomProcessor;
                            propertyValue = processor?.Process(element.QuerySelector(attr.Selector));
                            break;
                        default:
                            propertyValue = null;
                            break;
                    }
                    if (propertyValue == null && fallback is not null)
                    {
                        propertyValue = fallback.Value;
                    }
                    if (propertyValue != null)
                    {
                        if (formatter != null)
                        {
                            propertyValue = formatter.Format(propertyValue);
                        }
                        prop.SetValue(result, propertyValue);
                        break;
                    }
                }
            }
            return result;
        }
    }

}
