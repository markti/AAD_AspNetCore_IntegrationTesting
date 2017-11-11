using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApplicationAAD;

namespace XUnitTestProject1
{
    public class TestContext
    {

        private static string aadInstance = "https://login.microsoftonline.com/{0}";
        private static string authority;

        public HttpClient Client { get; set; }
        private TestServer _server;
        private string _accessToken;

        public TestContext()
        {
            SetupClient();
        }

        private void SetupClient()
        {
            var fullPath = Path.GetFullPath("@../../../../../../WebApplicationAAD/");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(fullPath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddUserSecrets<Startup>()
                .Build();

            _server = new TestServer(
                new WebHostBuilder()
                .UseStartup<Startup>()
                .UseConfiguration(configuration)
                );

            authority = String.Format(CultureInfo.InvariantCulture, aadInstance, AzureAdConfig.TenantId);

            _accessToken = GetAccessToken();
            
            Client = _server.CreateClient();
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
        }

        public static string GetAccessToken()
        {
            string token = null;

            using (var webClient = new WebClient())
            {
                var requestParameters = new NameValueCollection();

                requestParameters.Add("grant_type", "password");
                requestParameters.Add("client_id", AzureAdConfig.NativeAppDetails.AppId);
                requestParameters.Add("username", AzureAdConfig.TestUser.UserName);
                requestParameters.Add("password", AzureAdConfig.TestUser.Password);
                requestParameters.Add("resource", AzureAdConfig.WebApiAppDetails.AppId);
                //requestParameters.Add("scope", "openid");

                var url = $"https://login.microsoftonline.com/" + AzureAdConfig.TenantDomain + "/oauth2/token";
                var responsebytes = webClient.UploadValues(url, "POST", requestParameters);
                var responsebody = Encoding.UTF8.GetString(responsebytes);
                var jsonresult = JObject.Parse(responsebody);
                token = (string)jsonresult["access_token"];
            }

            return token;
        }
    }
}
