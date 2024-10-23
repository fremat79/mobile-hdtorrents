using HdTorrents.Types.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HdTorrents.Types.Attributes
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public abstract class CssSelectorAttribute : Attribute
    {
        public CssSelectorAttribute(string selector)
        {
            Selector = selector;
        }
        public string Selector { get; protected set; }
    }

    public class TextContentSelector : CssSelectorAttribute
    {
        public TextContentSelector(string selector) : base(selector)
        { }
    }

    public class IntContentSelector : CssSelectorAttribute
    {
        public IntContentSelector(string selector) : base(selector)
        { }
    }

    public class BoolContentSelector : CssSelectorAttribute
    {
        public BoolContentSelector(string selector) : base(selector)
        { }
    }

    public class TorrentTypeCssConverter : CssSelectorAttribute
    {
        public TorrentTypeCssConverter(string selector) : base(selector)
        { }
        public string FromCssClasses(string cssClasses)
        {
            return cssClasses switch
            {
                string film when cssClasses.Contains("fa-film") => "\uf008",
                string tv when cssClasses.Contains("fa-tv-retro") => "\uf401",
                _ => ""
            };
        }    
    }

    public class AttributeContentSelector : CssSelectorAttribute
    {
        public string AttributeName { get; private set; }
        public string? DefaultValue { get; private set; }
        public AttributeContentSelector(string selector, string attributeName, string defaultValue = null) 
            : base(selector)
        {
            AttributeName = attributeName;
            DefaultValue = defaultValue;
        }
    }

    public class AttributeSelector : CssSelectorAttribute
    {
        public string AttributeName { get; private set; }

        public AttributeSelector(string attributeName)
            : base(string.Empty)
        {
            AttributeName = attributeName;
        }
    }

    public class FallBackValue : Attribute
    {
        public string Value { get; private set; }   
        public FallBackValue(string value)
        {
            Value = value;
        }
    }

    public class CollectionAttributeSelector : CssSelectorAttribute
    {
        public CollectionAttributeSelector(string selector) 
            : base(selector) { }
    }

    public class CustomProcessingAttributeSelector : CssSelectorAttribute
    {        
        public Type Processor { get; private set; }
        public CustomProcessingAttributeSelector(string selector, Type processorType)
            : base(selector) 
        {
            Processor = processorType;
        }
    }
}
