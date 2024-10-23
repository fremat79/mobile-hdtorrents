using HdTorrents.Biz.Providers;
using System.Security;

namespace Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            AuthenticationProvider auth = new AuthenticationProvider();

            string user = "";
            string password = "";

            auth.Authenticate(user, password);

            auth.BuildTorrentProvider().GetTorrentsView();                                 
        }
    }
}