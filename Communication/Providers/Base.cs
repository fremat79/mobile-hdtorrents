using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using HdTorrents.Biz.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HdTorrents.Biz.Providers
{
    public abstract class BaseProvider
    {       
        protected HtmlParser Parser { get; private set; }
        internal HDTorrentClient Client {get; private set; }
        
        public BaseProvider()
        {            
            Client = new HDTorrentClient();
            Parser = new HtmlParser();            
        }

        public BaseProvider(HDTorrentClient client)
        {
            Client = client;
            Parser = new HtmlParser();
        }
    }
}
