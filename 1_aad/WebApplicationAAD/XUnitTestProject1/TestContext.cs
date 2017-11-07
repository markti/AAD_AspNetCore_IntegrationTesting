using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
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
        private TestServer _server;

        public TestContext()
        {
            SetupClient();
        }

        private void SetupClient()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

            string token = GetAccessToken();
            var authContext = new AuthenticationContext(authority, false);


            Client = _server.CreateClient();
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
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

            using (var webClient = new WebClient())
            {
                var requestParameters = new NameValueCollection();

                requestParameters.Add("resource", serviceUri);
                requestParameters.Add("client_id", clientID);
                requestParameters.Add("grant_type", "password");
                requestParameters.Add("username", userName);
                requestParameters.Add("password", password);
                requestParameters.Add("scope", "openid");

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
