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

    private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is TorrentsViewModel vm)
        {
            var newText = e.NewTextValue ?? string.Empty;

            // IAsyncRelayCommand has ExecuteAsync generated for async methods:
            if (vm.SearchCommand is CommunityToolkit.Mvvm.Input.IAsyncRelayCommand asyncCmd)
            {
                await asyncCmd.ExecuteAsync(newText);
            }
            else
            {
                // fallback: non-async execute (still fire it)
                if (vm.SearchCommand.CanExecute(newText))
                    vm.SearchCommand.Execute(newText);
            }

            // Read the results the ViewModel stored
            var results = vm.LastSearchResults;
            if (results != null)
            {
                // do something with results in the view (example: debug)
                System.Diagnostics.Debug.WriteLine($"Found {results?.Results?.Count ?? 0} torrents");
            }
        }
    }
}