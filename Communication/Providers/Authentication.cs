using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using HdTorrents.Biz.Client;

namespace HdTorrents.Biz.Providers
{
    public class AuthenticationProvider : BaseProvider
    {
        string? _userName;
        string? _password;               
        Uri _loginUri;
        Uri? _lastRequest;

        public AuthenticationProvider()
        {
            _loginUri = new("https://hdtorrents.eu/login");
        }

        public async Task<bool> Authenticate(string? userName, string? password)
        {
            _userName = userName;
            _password = password;
            
            bool result = false;
            
            var loginResponse = await Client.GetAsync(_loginUri);
            
            var session = loginResponse.Headers.First(h => h.Key == "Set-Cookie");
            Client.DefaultRequestHeaders.Add("Cookie", session.Value);
            
            var loginHtml = await loginResponse.Content.ReadAsStringAsync();
            var authForm = AuthenticationForm(loginHtml);

            if (authForm != null)
            {
                List<IHtmlInputElement> authInputs = authForm.QuerySelectorAll("input[type='hidden']")
                                        .Cast<IHtmlInputElement>().ToList();

                IHtmlInputElement? lastInput = authInputs.LastOrDefault();

                var urlEncodedParameters = new Dictionary<string, string>
                {
                    { "Content-Type", "application/x-www-form-urlencoded" },
                    { "_token", authInputs.First(i => i.Name == "_token").Value },
                    { "username", userName ?? "" },
                    { "password", password ?? ""},
                    { "_captcha", authInputs!.First(i => i.Name == "_captcha").Value },
                    { "_username", string.Empty },
                    { lastInput?.Name ?? "", lastInput?.Value ?? "" }
                };

                var loginPayload = new FormUrlEncodedContent(urlEncodedParameters);
                loginResponse = await Client.PostAsync(_loginUri, loginPayload);

                var afterLoginHtml = await loginResponse.Content.ReadAsStringAsync();

                authForm = AuthenticationForm(afterLoginHtml);

                result = (authForm == null);
            }

            return result;
        }

        public async Task<string> GetAsync(string uri)
        {
            return await GetAsync(new Uri(uri));
        }

        public async Task<string> GetAsync(Uri uri)
        {
            _lastRequest = uri;
            var request = await Client.GetAsync(uri);
            var response = await request.Content.ReadAsStringAsync();

            if ( AuthenticationForm(response) != null )
            {
                await Authenticate(_userName, _password);
                
                request = await Client.GetAsync(uri);
                response = await request.Content.ReadAsStringAsync();
            }
            return response;
        }

        public async void EnsureIsAuthenticated()
        {
            if (_lastRequest != null)
            {
                await GetAsync(_lastRequest);
            }
        }

        IHtmlFormElement? AuthenticationForm(string html)
        {
            return Parser.ParseDocument(html)
                .QuerySelectorAll("form[class='auth-form__form']")
                .Cast<IHtmlFormElement>().FirstOrDefault();
        }
        public TorrentsProvider BuildTorrentProvider()
        {
            return new TorrentsProvider(this);
        }
    }
}