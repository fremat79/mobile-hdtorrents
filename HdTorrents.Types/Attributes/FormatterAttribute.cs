using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HdTorrents.Types.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class FormatterAttribute : Attribute
    {
        public abstract string Format(object rawValue);
    }

    public class StringSplitFormatter: FormatterAttribute
    {
        char Separator { get; set; }
        public StringSplitFormatter(char splitChar) 
        {
            Separator = splitChar;
        }
        public override string Format(object rawValue)
        {            
            return string.Join(Environment.NewLine, $"{rawValue}".Split(Separator));
        }
    }
}
