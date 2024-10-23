using HdTorrents.Biz;
using HdTorrents.ViewModel;
using HdTorrents.Views;
using Microsoft.Extensions.Logging;

namespace HdTorrents
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fa-solid-900.ttf", "FA");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<LoginView>();
            builder.Services.AddSingleton<TorrentsView>();
            builder.Services.AddSingleton<TorrentDetailsView>();

            builder.Services.AddSingleton<ViewModel.LoginViewModel>();
            builder.Services.AddSingleton<ViewModel.TorrentsViewModel>();
            builder.Services.AddSingleton<ViewModel.TorrentDetailsViewModel>();

            builder.Services.AddSingleton(typeof(HdTorrentsSite), new  HdTorrentsSite());

            return builder.Build();
        }
    }
}