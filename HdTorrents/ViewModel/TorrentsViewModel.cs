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
    public partial class TorrentsViewModel : HdTorrentsBaseView
    {
        [ObservableProperty]
        ObservableCollection<Torrent> items;

        [ObservableProperty]
        ObservableCollection<BasePage> pages;

        [ObservableProperty]
        bool isRefreshing;

        public LayoutMode Layout { get => Site.Layout; 
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
        async Task RefreshTorrents()
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
        async Task GoToDetailView(Torrent item)
        {
            await Shell.Current.GoToAsync($"{nameof(TorrentDetailsView)}?Url={item.DetailsUrl}");
        }

        [RelayCommand]
        async Task ChangeLayout(string s)
        {
            Layout = Enum.Parse<LayoutMode>(s);
            await RefreshTorrents();
        }

        [RelayCommand]
        async Task NavigateToPage(BasePage page)
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
        async Task LoadMore()
        {
            var nextPage = Site.PageNumber + 1;
            Site.PageNumber = nextPage;           
            var tv = await Site.GetTorrentsViewAsync();
            Pages.Clear();
            tv.Pages.AvailablePages.ForEach(p => Pages.Add(p));
            tv.Torrents.ForEach(t => Items.Add(t));
        }
    }

    public class TorrentsViewTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Details { get; set; }
        public DataTemplate Poster { get; set; }
        public DataTemplate Card { get; set; }

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

    public class HDTorrentCollectionView  : CollectionView
    {
        public static readonly BindableProperty TorrentsLayoutProperty = BindableProperty.Create(nameof(TorrentsLayout),
            typeof(LayoutMode), typeof(HDTorrentCollectionView));
                
        public LayoutMode TorrentsLayout 
        { 
            get => (LayoutMode)GetValue(TorrentsLayoutProperty);
            set => SetValue(TorrentsLayoutProperty, value);
        }
    }
}
