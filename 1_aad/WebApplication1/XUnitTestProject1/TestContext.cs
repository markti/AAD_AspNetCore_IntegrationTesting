using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebApplication1;

namespace XUnitTestProject1
{
    public class TestContext
    {

        private static string aadInstance = "https://login.microsoftonline.com/";
        private static string tenant = "vitalsigyn.com";
        private static string clientId = "870a7afc-7c74-45e4-bfcd-e4396e250900";
        Uri redirectUri = new Uri(ConfigurationManager.AppSettings["ida:RedirectUri"]);

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

            Client = _server.CreateClient();
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        }
    }
}
