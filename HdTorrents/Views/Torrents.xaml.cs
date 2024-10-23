using HdTorrents.ViewModel;

namespace HdTorrents.Views;

public partial class TorrentsView : ContentPage
{
	public TorrentsView(TorrentsViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
		// this call the reload command on TorrentViewModel
		vm.IsRefreshing = true;
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
}