using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using System.Globalization;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using WebApplicationB2C;

namespace XUnitTestProject1
{
    public class TestContext
    {

        private static string aadInstance = "https://login.microsoftonline.com/";
        // domain
        private static string tenantDomain = "vitalsigyn.com";
        // aka tenant guid?
        private static string tenant = "3bd0245c-cac9-4dc2-bb49-15371698af05";
        // aka Application ID
        private static string clientId = "d476fd33-4cfc-4aeb-9d4d-ea4978af5660";
        Uri redirectUri = new Uri("https://vitalsigyn.com/WebApplicationAAD");
        private static string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

        public HttpClient Client { get; set; }
        private TestServer _server;

        public TestContext()
        {
            SetupClient();
        }

        private void SetupClient()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            string token = "foo";
            var authContext = new AuthenticationContext(authority, false);


            Client = _server.CreateClient();
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        }

        //public static async Task<AuthenticationResult> getAccessToken()
        //{
        //    string hardcodedUsername = "integration_test@vitalsigyn.com";
        //    string hardcodedPassword = "";

        //    string tenant = "vitalsigyn.com";
        //    string clientId = "d476fd33-4cfc-4aeb-9d4d-ea4978af5660";
        //    string resourceHostUri = "https://management.azure.com/";
        //    string aadInstance = "https://login.microsoftonline.com/{0}";

        //    string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

        //    var postData = new List<KeyValuePair<string, string>>();
        //    postData.Add(new KeyValuePair<string, string>("grant_type", "password"));
        //    postData.Add(new KeyValuePair<string, string>("resource", "resource"));
        //    postData.Add(new KeyValuePair<string, string>("username", hardcodedUsername));
        //    postData.Add(new KeyValuePair<string, string>("password", hardcodedPassword));
        //    postData.Add(new KeyValuePair<string, string>("client_id", clientId));
        //    HttpContent content = new FormUrlEncodedContent(postData);

        //    HttpClient aadPost = new HttpClient();

        //    aadPost.PostAsync("https://login.microsoftonline.com/{tenant-id}/oauth2/token", content);

        //}
    }
}
