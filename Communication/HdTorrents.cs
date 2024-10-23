using HdTorrents.Biz.Providers;
using HdTorrents.Types.Models;

namespace HdTorrents.Biz
{
    public class HdTorrentsSite
    {
        AuthenticationProvider _authentication;
        TorrentsProvider? _torrents;

        public int PageNumber { get => _torrents!.PageNumber; set => _torrents!.PageNumber = value; }

        public int ItemsPerPage { get=> _torrents!.ItemsPerPage  ; set => _torrents!.ItemsPerPage = value; }

        public LayoutMode Layout { get => _torrents!.Layout; set => _torrents!.Layout = value; }

        public HdTorrentsSite()
        {
            _authentication = new AuthenticationProvider();
            
        }
        public async Task<bool> Login(string username, string password)
        {
            var loginSucceded = await _authentication.Authenticate(username, password);
            if (loginSucceded)
            {
                _torrents = _authentication.BuildTorrentProvider();
            }
            return loginSucceded;
        }
        public async Task<PagedTorrentDetailsView?> GetTorrentsViewAsync()
        {
            return await _torrents!.GetTorrentsView();
        }

        public async Task<TorrentDetails?> LoadDetails(string torrentId)
        {
            _torrents.TestPost();
            
            return await _torrents!.GetDetails(torrentId);
        }

        public async Task<PersonDetails?> LoadPersonDetails(string url)
        {
            return await _torrents!.GetPersonDetails(url);
        }

        public async Task<string> DownloadTorrentFile(string appDataDirectory, string downloadUrl)
        {
            return await _torrents!.GetTorrentFile(appDataDirectory, downloadUrl);
        }
    }
}
