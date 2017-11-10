using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Collections.Specialized;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        private readonly TestContext _sut;

        public UnitTest1()
        {
            _sut = new TestContext();
        }

        [Fact]
        public async Task Fails401NoBearerToken()
        {
            //var response = await _sut.Client.GetAsync("/api/Values");

            var req = _sut.Server.CreateRequest("/api/Values").AddHeader("Authorization", "Bearer " + _sut.AccessToken);
            var response = await req.GetAsync();

            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task SuccessWithBearerToken()
        {
            var response = await _sut.Client.GetAsync("/api/Values");

            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrEmpty(result));
        }

        [Fact]
        public async Task GetToken()
        {
            using (HttpClient client = new HttpClient())
            {
                string tenantId = "3bd0245c-cac9-4dc2-bb49-15371698af05";
                var tokenEndpoint = @"https://login.windows.net/" + tenantId + "/oauth2/token";
                var accept = "application/json";
                string clientId = "d476fd33-4cfc-4aeb-9d4d-ea4978af5660";

                client.DefaultRequestHeaders.Add("Accept", accept);
                string postBody = @"resource=https://vitalsigyn.com/WebApplicationAAD/&client_id=" + clientId + "&grant_type=password&username=integration_test@vitalsigyn.com&password=V1t4alS1gyn&scope=openid";

                using (var response = await client.PostAsync(tokenEndpoint, new StringContent(postBody, Encoding.UTF8, "application/x-www-form-urlencoded")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseAsString = await response.Content.ReadAsStringAsync();
                        var jsonresult = JObject.Parse(responseAsString);
                        var token = (string)jsonresult["access_token"];
                    }
                }
            }
        }

        [Fact]
        public async Task GetBearerTokenWithPassword()
        {

            string hardcodedUsername = "integration_test@vitalsigyn.com";
            string hardcodedPassword = "Wosu1257";

            string resourceId = "https://vitalsigyn.com/WebApplicationAAD";

            string tenant = "vitalsigyn.com";
            string tenantId = "3bd0245c-cac9-4dc2-bb49-15371698af05";
            string clientId = "d476fd33-4cfc-4aeb-9d4d-ea4978af5660";
            string resourceHostUri = "https://management.azure.com/";
            string aadInstance = "https://login.microsoftonline.com/{0}";

            string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("grant_type", "password"));
            postData.Add(new KeyValuePair<string, string>("resource", resourceId));
            postData.Add(new KeyValuePair<string, string>("username", hardcodedUsername));
            postData.Add(new KeyValuePair<string, string>("password", hardcodedPassword));
            postData.Add(new KeyValuePair<string, string>("client_id", clientId));
            HttpContent content = new FormUrlEncodedContent(postData);

            HttpClient aadPost = new HttpClient();

            var postUrl = "https://login.microsoftonline.com/" + tenantId + "/oauth2/token";

            var response = await aadPost.PostAsync(postUrl, content);
            
            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetTokenWithClientCredentials()
        {
            string tenantId = "3bd0245c-cac9-4dc2-bb49-15371698af05";
            string resourceId = "https://vitalsigyn.com/WebApplicationAAD";
            string clientId = "d476fd33-4cfc-4aeb-9d4d-ea4978af5660";
            string clientSecret = "LojHZrAT3lZkjQZp/wDzf8b5TW/8R1thfJ4uAvt/i+w=";

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            postData.Add(new KeyValuePair<string, string>("resource", resourceId));
            postData.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
            postData.Add(new KeyValuePair<string, string>("client_id", clientId));
            HttpContent content = new FormUrlEncodedContent(postData);

            using (HttpClient client = new HttpClient())
            {
                var tokenEndpoint = @"https://login.windows.net/" + tenantId + "/oauth2/token";
                var accept = "application/json";

                client.DefaultRequestHeaders.Add("Accept", accept);
                string postBody = @"resource=" + resourceId + "&client_id=" + clientId + "&grant_type=client_credentials&client_secret=" + clientSecret;

                using (var response = await client.PostAsync(tokenEndpoint, new StringContent(postBody, Encoding.UTF8, "application/x-www-form-urlencoded")))
                {

                    if (response.IsSuccessStatusCode)
                    {
                    }
                    var jsonresult = await response.Content.ReadAsStringAsync();
                }
            }
        }

        [Fact]
        public async Task TestMethod3()
        {
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
                var responsebytes = await webClient.UploadValuesTaskAsync(url, "POST", requestParameters);
                var responsebody = Encoding.UTF8.GetString(responsebytes);
            }
        }
    }
}
