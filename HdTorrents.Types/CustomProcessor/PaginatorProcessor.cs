using AngleSharp.Dom;
using HdTorrents.Types.Interface;
using HdTorrents.Types.Models;
using System.Collections;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace HdTorrents.Types.CustomProcessor
{
    public class PaginatorProcessor : ICustomProcessor
    {
        Regex pageRegEx = new Regex(@"\Spage=(?<page>\d+)", RegexOptions.Compiled| RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        string[] selectors = new string[] { "li [class*='pagination__previous']", "ul[class='pagination__pages'] li" , "li [class='pagination__next']" };
        public object Process(IElement item)
        {
            var result = new Paginator();

            Array.ForEach(selectors, s => {
                var x = s;
                item.QuerySelectorAll(s).ToList().ForEach(p =>
                {                                       
                    var page = CreatePage(p);
                    if (page != null)
                    {
                        result.AvailablePages.Add(page);
                    }
                });
            });
            
            return result;
        }
        BasePage? CreatePage(IElement item)
        {           
            var classes = string.Join(' ', item.ClassList);
            if (string.IsNullOrEmpty(classes))
            {
                classes = string.Join(' ', item.QuerySelector("a")?.ClassList ?? new EmptyTokenList() );
            }

            return classes switch
            {
                var c when c.Contains("current") => CreatePage<CurrentPage>(item),
                var c when c.Contains("previous") => CreatePage<PreviousPage>(item),
                var c when c.Contains("next") => CreatePage<NextPage>(item),
                var c when c.Contains("pagination__link") => CreatePage<Page>(item),
                _ => null
            }; 
        }
        T CreatePage<T> (IElement? pageElement) where T : BasePage
        {
            T? result = (T?)Activator.CreateInstance(typeof (T));
            var pageMatch = pageRegEx.Match(pageElement?.Attributes["href"]?.Value ?? string.Empty);
            if (pageMatch.Success)
            {
                result!.PageNumber = Convert.ToInt32(pageMatch.Groups["page"].Value);
                if (string.IsNullOrEmpty(result.DisplayText))
                {
                    result.DisplayText = pageMatch.Groups["page"].Value;
                }
            }
            else
            {
                if (int.TryParse(pageElement?.TextContent, out var pageNumber))
                {
                    result!.PageNumber = pageNumber;
                    result!.DisplayText = pageNumber.ToString();
                }               
            }
            return result;
        }
    }

    public class EmptyTokenList : ITokenList
    {
        public string this[int index] => string.Empty;
        public int Length => 0;
        public void Add(params string[] tokens)
        {
            
        }
        public bool Contains(string token)
        {
            return false;
        }
        public IEnumerator<string> GetEnumerator() => new List<string>().GetEnumerator();        
        public void Remove(params string[] tokens)
        {
        }
        public bool Toggle(string token, bool force = false) => false;        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();        
    }
}
