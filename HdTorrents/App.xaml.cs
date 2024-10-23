using HdTorrents.Views;

namespace HdTorrents
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            // use navigationpage to wrap initial page to enable 
            // navigation stack
            //MainPage = new NavigationPage(new MainPage())
            //{
            //    BarBackground = Colors.Chocolate
            //};
        }
    }
}