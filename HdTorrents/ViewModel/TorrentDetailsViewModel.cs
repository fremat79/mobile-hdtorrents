using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HdTorrents.Biz;
using HdTorrents.Types.Models;
using HdTorrents.Views;


namespace HdTorrents.ViewModel
{
    public partial class TorrentDetailsViewModel : HdTorrentsBaseView
    {
        TorrentDetails _details;
        public TorrentDetailsViewModel(HdTorrentsSite site)
        : base(site)
        {
            
        }

        public TorrentDetails Details {
            get => _details;
            set{
                _details = value;
                HasRecommendations = _details?.Recommendations?.Any() ?? false;
                HasCrew = _details?.Crew?.Any() ?? false;
                HasCast = _details?.Cast?.Any() ?? false;
                HasExtras = _details?.Extras?.Any() ?? false;
                OnPropertyChanged(nameof(Details));
            }
        }

        [ObservableProperty]
        bool hasRecommendations;
        [ObservableProperty]
        bool hasCast;
        [ObservableProperty]
        bool hasExtras;
        [ObservableProperty]
        bool hasCrew;

        public bool CanOpenTorrent {  
            get 
            {
                try
                {
                    var ofr = new OpenFileRequest();
                    ofr.File = new ReadOnlyFile("try.torrent");
                    var openTorrent = Launcher.CanOpenAsync("try.torrent").Result;
                    return openTorrent;
                }
                catch (Exception)
                {
                    return false;
                }
            } 
        }

        public async Task LoadDetails(string id)
        {
            Details = await Site.LoadDetails(id);
        }

        [RelayCommand]
        async Task GoToDetailView(Recommendation item)
        {
            var details = new TorrentDetailsView( new TorrentDetailsViewModel(Site));
            details.ApplyQueryAttributes( new Dictionary<string, object>() { { "Url", item.DetailsUrl } });
            await Application.Current.MainPage.Navigation.PushAsync(details);
        }

        [RelayCommand]
        async Task GoToPersonDetails(Chip item)
        {
            var pDetails = new PersonDetailsView(new PersonDetailsViewModel(Site));
            pDetails.ApplyQueryAttributes(new Dictionary<string, object>() { { "Url", item.DetailslUrl } });
            await Application.Current.MainPage.Navigation.PushAsync(pDetails);
        }

        [RelayCommand]
        async Task DownloadTorrent(string downloadUrl)
        {
            var appDataDirectory = FileSystem.AppDataDirectory;
            var filePath = await base.Site.DownloadTorrentFile(appDataDirectory, downloadUrl);

            await Launcher.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(filePath) });
                        
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(downloadUrl))
                {
                    using (Stream stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (FileStream fileStream = File.Create(filePath))
                        {
                            await stream.CopyToAsync(fileStream);
                        }
                    }
                }
            }
        }
    }

    public class DetailsDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ImageUrlTemplate { get; set; }
        public DataTemplate FATemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {            
            Chip details = (Chip)item;
            return string.IsNullOrEmpty(details.ImageUrl) ? FATemplate : ImageUrlTemplate;
        }
    }
}
