using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HdTorrents.Biz.Client
{
    public class HDTorrentClient : HttpClient
    {
        public HDTorrentClient()
        {
            SetUpHttpClient();
        }

        void SetUpHttpClient()
        {
            base.DefaultRequestHeaders.Add("Host", "hdtorrents.eu");
            base.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36 Edg/119.0.0.0");
            base.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            base.DefaultRequestHeaders.Add("Accept-Encoding", "deflate, br");
            base.DefaultRequestHeaders.Add("Accept-Language", "it");
            base.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            base.DefaultRequestHeaders.Add("Connection", "Keep-alive");
            base.DefaultRequestHeaders.Add("Referer", "https://hdtorrents.eu/login");
            base.DefaultRequestHeaders.Add("Sec-Ch-Ua", "Microsoft Edge\";v=\"119\", \"Chromium\";v=\"119\", \"Not?A_Brand\";v=\"24");
            base.DefaultRequestHeaders.Add("Sec-Ch-Ua-Mobile", "?0");
            base.DefaultRequestHeaders.Add("Sec-Ch-Ua-Platform", "Windows");
            base.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            base.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            base.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            base.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            base.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
        }
    }
}
