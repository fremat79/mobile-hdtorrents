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

    void LoadCredentials()
    {
        try
        {                                  
            var usernameT = SecureStorage.GetAsync("username");
            var passwordT = SecureStorage.GetAsync("password");
            usernameT.Wait();
            passwordT.Wait();
            
            var username = usernameT.Result;
            var password = passwordT.Result;

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