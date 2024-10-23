using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HdTorrents.Types.Interface
{
    public interface ICustomProcessor
    {
        public object Process(IElement item);
    }
}
