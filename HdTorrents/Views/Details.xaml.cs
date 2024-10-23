using HdTorrents.ViewModel;

namespace HdTorrents.Views;


public partial class TorrentDetailsView : ContentPage, IQueryAttributable
{
	public TorrentDetailsView(TorrentDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;                
	}

    TorrentDetailsViewModel ViewModel => BindingContext as TorrentDetailsViewModel;
    
    string TorrentUrl { get; set; }
    bool LoadEnd { get; set; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
            var url = (string)query["Url"];
            if (url != TorrentUrl) 
            {
                TorrentUrl = url;
                LoadEnd = false;
            }            
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        if (!LoadEnd)
        {
            await ViewModel.LoadDetails(TorrentUrl);
            LoadEnd = true;
        }
    }
}