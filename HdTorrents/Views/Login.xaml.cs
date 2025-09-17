using HdTorrents.ViewModel;
using System.Text.Json;

namespace HdTorrents.Views;

public partial class LoginView : ContentPage
{        
    public LoginView(LoginViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;

        LoadCredentials();    
    }

    async void LoadCredentials()
    {
        try
        {                                  
            var usernameT = await SecureStorage.GetAsync("username");
            var passwordT = await SecureStorage.GetAsync("password");


            var username = usernameT;
            var password = passwordT;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                txtUserName.Text = username;
                txtPwd.Text = password;
            }
        }
        catch (Exception ex)
        {
            txtUserName.Text = string.Empty;
            txtPwd.Text = string.Empty;
            System.Diagnostics.Debug.WriteLine($"Error while loading credentials {ex}");
        }
    }

    void btnShowPwd_Pressed(object sender, EventArgs e)
    {
        txtPwd.IsPassword = !txtPwd.IsPassword;
    }
}