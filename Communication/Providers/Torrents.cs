using AngleSharp.Html.Parser;
using AngleSharp.Io;
using HdTorrents.Biz.Client;
using HdTorrents.Biz.Helpers;
using HdTorrents.Types.Models;
using System.IO.Enumeration;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;

namespace HdTorrents.Biz.Providers
{
    public class TorrentsProvider : BaseProvider
    {
        string _baseUrl;

        Uri Page
        {
            get
            {
                return Layout switch
                {
                    LayoutMode.Details => new Uri($"{_baseUrl}?perPage{ItemsPerPage}&page={PageNumber}"),
                    LayoutMode.Poster => new Uri($"{_baseUrl}?perPage{ItemsPerPage}&page={PageNumber}&view=poster"),
                    LayoutMode.Card => new Uri($"{_baseUrl}?perPage{ItemsPerPage}&page={PageNumber}&view=card"),
                    _ => new Uri($"{_baseUrl}?perPage{ItemsPerPage}&page={PageNumber}")
                };                
            }
        }

        new AuthenticationProvider Client { get; set; }

        public int PageNumber { get; set; }

        public int ItemsPerPage { get; set; }

        public LayoutMode Layout { get; set; }

        public TorrentsProvider(AuthenticationProvider authenticationProvider)
            : base(authenticationProvider.Client)
        {
            Client = authenticationProvider;
            _baseUrl = "https://hdtorrents.eu/torrents";
            PageNumber = 1;
            ItemsPerPage = 25;
            Layout = LayoutMode.Details;
        }

        public async Task<PagedTorrentDetailsView?> GetTorrentsView()
        {
            var torrentsHtml = await Client.GetAsync(Page);
            var htmlDoc = await Parser.ParseDocumentAsync(torrentsHtml);
            var main = htmlDoc.QuerySelector("[class='panelV2 torrent-search__results']");
            var result = HDTorrentBuilderHelper.FromIElement<PagedTorrentDetailsView>(main);
            return result;
        }

        public async Task<TorrentDetails?> GetDetails(string torrentId)
        {
            var torrentHtml = await Client.GetAsync(torrentId);
            var htmlDoc = await Parser.ParseDocumentAsync(torrentHtml);
            var detail = htmlDoc.QuerySelector("body > main");
            var torrentDetails = HDTorrentBuilderHelper.FromIElement<TorrentDetails>(detail);
            torrentDetails!.Url = torrentId;
            return torrentDetails;
        }

        public async Task<PersonDetails> GetPersonDetails(string url)
        {
            var torrentHtml = await Client.GetAsync(url);
            var htmlDoc = await Parser.ParseDocumentAsync(torrentHtml);
            var detail = htmlDoc.QuerySelector("body > main");
            var personDetails = HDTorrentBuilderHelper.FromIElement<PersonDetails>(detail);
            personDetails!.Url = url;
            return personDetails;
        }

        public void TestPost()
        {

            var urlEncodedParameters = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },                   
                };

            var json = "{\"_token\":\"Ii0hAMqLjXt9d9u9vhprS7KOYshwYj1LmFIYCeLR\",\"components\":[{\"snapshot\":\"{\\\"data\\\":{\\\"name\\\":\\\"zombielan\\\",\\\"description\\\":\\\"\\\",\\\"mediainfo\\\":\\\"\\\",\\\"uploader\\\":\\\"\\\",\\\"keywords\\\":\\\"\\\",\\\"startYear\\\":null,\\\"endYear\\\":null,\\\"minSize\\\":null,\\\"minSizeMultiplier\\\":1,\\\"maxSize\\\":null,\\\"maxSizeMultiplier\\\":1,\\\"categories\\\":[[],{\\\"s\\\":\\\"arr\\\"}],\\\"types\\\":[[],{\\\"s\\\":\\\"arr\\\"}],\\\"resolutions\\\":[[],{\\\"s\\\":\\\"arr\\\"}],\\\"genres\\\":[[],{\\\"s\\\":\\\"arr\\\"}],\\\"regions\\\":[[],{\\\"s\\\":\\\"arr\\\"}],\\\"distributors\\\":[[],{\\\"s\\\":\\\"arr\\\"}],\\\"adult\\\":\\\"any\\\",\\\"tmdbId\\\":null,\\\"imdbId\\\":\\\"\\\",\\\"tvdbId\\\":null,\\\"malId\\\":null,\\\"playlistId\\\":null,\\\"collectionId\\\":null,\\\"networkId\\\":null,\\\"companyId\\\":null,\\\"primaryLanguages\\\":[[],{\\\"s\\\":\\\"arr\\\"}],\\\"free\\\":[[],{\\\"s\\\":\\\"arr\\\"}],\\\"doubleup\\\":false,\\\"featured\\\":false,\\\"refundable\\\":false,\\\"stream\\\":false,\\\"sd\\\":false,\\\"highspeed\\\":false,\\\"bookmarked\\\":false,\\\"wished\\\":false,\\\"internal\\\":false,\\\"personalRelease\\\":false,\\\"alive\\\":false,\\\"dying\\\":false,\\\"dead\\\":false,\\\"graveyard\\\":false,\\\"notDownloaded\\\":false,\\\"downloaded\\\":false,\\\"seeding\\\":false,\\\"leeching\\\":false,\\\"incomplete\\\":false,\\\"perPage\\\":25,\\\"sortField\\\":\\\"bumped_at\\\",\\\"sortDirection\\\":\\\"desc\\\",\\\"view\\\":\\\"list\\\",\\\"paginators\\\":[{\\\"page\\\":1},{\\\"s\\\":\\\"arr\\\"}]},\\\"memo\\\":{\\\"id\\\":\\\"WFHiC4fmXCcpzZjXzKSG\\\",\\\"name\\\":\\\"torrent-search\\\",\\\"path\\\":\\\"torrents\\\",\\\"method\\\":\\\"GET\\\",\\\"children\\\":[],\\\"scripts\\\":[],\\\"assets\\\":[],\\\"errors\\\":[],\\\"locale\\\":\\\"it\\\"},\\\"checksum\\\":\\\"a3eff339bd8cf69bcc6caeb03f2d1b36db05d3c07f1386ac72c06fb22f52ec7a\\\"}\",\"updates\":{\"name\":\"zombieland\"},\"calls\":[]}]}";

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");



            var s = Client.Client.PostAsync("https://hdtorrents.eu/livewire/update", content).Result.Content.ReadAsStringAsync();



        }

        internal async Task<string> GetTorrentFile(string appDataDirectory, string torrentUrl)
        {
            Client.EnsureIsAuthenticated();
            string filePath = string.Empty;            
            var response = await base.Client.GetAsync(torrentUrl);
            response.EnsureSuccessStatusCode();
            var contentDisposition = response.Content.Headers.ContentDisposition;
            if (contentDisposition != null)
            {
                var fileName = contentDisposition?.FileName?.Replace("\"", string.Empty);
                filePath = Path.Combine(appDataDirectory, fileName ?? "torrentDownload.torrent");

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    using (FileStream fileStream = File.Create(filePath))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
            }
            return filePath;
        }
    }
}
