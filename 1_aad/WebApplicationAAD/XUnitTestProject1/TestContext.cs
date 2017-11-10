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
        // domain
        private static string tenantDomain = "vitalsigyn.com";
        // aka tenant guid?
        private static string tenant = "3bd0245c-cac9-4dc2-bb49-15371698af05";
        // aka Application ID
        private static string clientId = "d476fd33-4cfc-4aeb-9d4d-ea4978af5660";
        Uri redirectUri = new Uri("https://vitalsigyn.com/WebApplicationAAD");
        private static string authority;

        public HttpClient Client { get; set; }
        public TestServer Server { get; set; }
        public string AccessToken { get; set; }

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

            Server = new TestServer(
                new WebHostBuilder()
                .UseStartup<Startup>()
                .UseConfiguration(configuration)
                );

            authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

            AccessToken = GetAccessToken();
            var authContext = new AuthenticationContext(authority, false);
            
            Client = Server.CreateClient();
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
        }

        public static string GetAccessToken()
        {
            string token = null;
            //  Constants
            var tenant = "vitalsigyn.onmicrosoft.com";
            var serviceUri = "https://vitalsigyn.com/WebApplicationAAD";
            var clientID = "a1d51587-bf4c-4915-b588-8df1d9fd7ac9";
            var userName = "integration_test@vitalsigyn.com";
            var password = "V1t4alS1gyn";
            var appClientId = "d476fd33-4cfc-4aeb-9d4d-ea4978af5660";

            using (var webClient = new WebClient())
            {
                var requestParameters = new NameValueCollection();

                requestParameters.Add("grant_type", "password");
                requestParameters.Add("client_id", clientID);
                requestParameters.Add("username", userName);
                requestParameters.Add("password", password);
                requestParameters.Add("resource", appClientId);
                //requestParameters.Add("scope", "openid");

                var url = $"https://login.microsoftonline.com/" + tenant + "/oauth2/token";
                var responsebytes = webClient.UploadValues(url, "POST", requestParameters);
                var responsebody = Encoding.UTF8.GetString(responsebytes);
                var jsonresult = JObject.Parse(responsebody);
                token = (string)jsonresult["access_token"];
            }

            return token;
        }
    }
}
