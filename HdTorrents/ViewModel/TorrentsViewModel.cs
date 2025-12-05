using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HdTorrents.Biz;
using HdTorrents.Biz.Extension;
using HdTorrents.Types.Models;
using HdTorrents.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HdTorrents.ViewModel
{
    public class HDTorrentCollectionView : CollectionView
    {
        public static readonly BindableProperty TorrentsLayoutProperty = BindableProperty.Create(nameof(TorrentsLayout),
            typeof(LayoutMode), typeof(HDTorrentCollectionView));

        public LayoutMode TorrentsLayout
        {
            get => (LayoutMode)GetValue(TorrentsLayoutProperty);
            set => SetValue(TorrentsLayoutProperty, value);
        }
    }

    public partial class TorrentsViewModel : HdTorrentsBaseView
    {
        [ObservableProperty]
        private bool isRefreshing;

        [ObservableProperty]
        private ObservableCollection<Torrent> items;

        [ObservableProperty]
        private SearchResults? lastSearchResults;

        [ObservableProperty]
        private ObservableCollection<BasePage> pages;

        public LayoutMode Layout
        {
            get => Site.Layout;
            set
            {
                Site.Layout = value;
                OnPropertyChanged();
            }
        }

        public TorrentsViewModel(HdTorrentsSite site)
            : base(site)
        {
            items = new ObservableCollection<Torrent>();
            pages = new ObservableCollection<BasePage>();
        }

        [RelayCommand]
        private async Task ChangeLayout(string s)
        {
            Layout = Enum.Parse<LayoutMode>(s);
            await RefreshTorrents();
        }

        [RelayCommand]
        private async Task GoToDetailView(Torrent item)
        {
            await Shell.Current.GoToAsync($"{nameof(TorrentDetailsView)}?Url={item.DetailsUrl}");
        }

        [RelayCommand]
        private async Task LoadMore()
        {
            var nextPage = Site.PageNumber + 1;
            Site.PageNumber = nextPage;
            var tv = await Site.GetTorrentsViewAsync();
            Pages.Clear();
            tv.Pages.AvailablePages.ForEach(p => Pages.Add(p));
            tv.Torrents.ForEach(t => Items.Add(t));
        }

        [RelayCommand]
        private async Task NavigateToPage(BasePage page)
        {
            if (page.PageNumber.HasValue)
            {
                Items.Clear();
                Pages.Clear();
                Site.PageNumber = page.PageNumber.Value;
                await RefreshTorrents();
            }
        }

        [RelayCommand]
        private async Task RefreshTorrents()
        {
            IsRefreshing = true;
            Items.FillSkeleton();
            Pages.FillSkeleton();
            var tv = await Site.GetTorrentsViewAsync();
            Items.Clear();
            Pages.Clear();
            tv.Torrents?.ForEach(t => Items.Add(t));
            tv.Pages?.AvailablePages.ForEach(p => Pages.Add(p));
            IsRefreshing = false;
        }

        [RelayCommand]
        private async Task Search(string query)
        {
            var result = await Site.Search(query);

            // store result so callers can read it after the command completes
            LastSearchResults = result;

            // optional: update Items if you want the UI list updated from search results
            if (result?.Results != null)
            {
                Items.Clear();
                foreach (var t in result.Results)
                    LastSearchResults.Results.Add(t);
            }
        }
    }

    public class TorrentsViewTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Card { get; set; }
        public DataTemplate Details { get; set; }
        public DataTemplate Poster { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var layout = (container as HDTorrentCollectionView)?.TorrentsLayout;
            if (layout.HasValue)
            {
                return layout.Value switch
                {
                    LayoutMode.Details => Details,
                    LayoutMode.Poster => Poster,
                    LayoutMode.Card => Card,
                    _ => Details
                };
            }
            return Details;
        }
    }
}