
using HdTorrents.Views;

namespace HdTorrents
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();


            Routing.RegisterRoute(nameof(TorrentsView), typeof(TorrentsView));
            Routing.RegisterRoute(nameof(TorrentDetailsView), typeof(TorrentDetailsView));
        }
    }
}