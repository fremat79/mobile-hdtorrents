using CommunityToolkit.Mvvm.Input;
using HdTorrents.Biz;
using HdTorrents.Views;
using System.Globalization;
using System.Text.Json;

namespace HdTorrents.ViewModel
{
    public class LoginInfo
    {
        public string Password { get; set; }
        public string UserName { get; set; }
    }

    public partial class LoginViewModel : HdTorrentsBaseView
    {
        public LoginViewModel(HdTorrentsSite site)
            : base(site)
        {
        }

        [RelayCommand]
        private async Task Login(string[] loginInfo)
        {
            var loginObj = new LoginInfo() { UserName = loginInfo[0], Password = loginInfo[1] };

            string loginObjJson = JsonSerializer.Serialize(loginObj);

            await StoreCredentials(loginObj.UserName, loginObj.Password);

            await Site.Login(loginObj.UserName, loginObj.Password);

            await Shell.Current.GoToAsync(nameof(TorrentsView));
        }

        private async Task StoreCredentials(string userName, string passWord)
        {
            try
            {
                // Remove specific keys if they exist
                SecureStorage.Default.Remove("username");
                SecureStorage.Default.Remove("password");

                await SecureStorage.Default.SetAsync("username", userName);
                await SecureStorage.Default.SetAsync("password", passWord);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error while store credentials {ex}");
            }
        }
    }

    public class MyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Cast<string>().ToArray();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}