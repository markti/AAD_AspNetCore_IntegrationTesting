using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace XUnitTestProject1
{
    public class OAuthTests
    {

        [Fact]
        public async Task ObtainToken_HttpClientPost_WebApi_ClientCredentialGrantType()
        {
            string tenantId = AzureAdConfig.TenantId;
            string resourceId = AzureAdConfig.WebApiAppDetails.AppUriId;
            string clientId = AzureAdConfig.WebApiAppDetails.AppId;
            string clientSecret = AzureAdConfig.WebApiAppDetails.AccessKey;

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


                    response.EnsureSuccessStatusCode();
                    response.StatusCode.Should().Be(HttpStatusCode.OK);

                    if (response.IsSuccessStatusCode)
                    {
                    }
                    var jsonresult = await response.Content.ReadAsStringAsync();
                }
            }
        }

        [Fact]
        public async Task ObtainToken_WebClientPost_NativeClient_PasswordGrantType()
        {
            //  Constants
            var tenant = AzureAdConfig.TenantDomain;
            var serviceUri = AzureAdConfig.WebApiAppDetails.AppUriId;
            // native app "IntegrationTest"
            var clientID = AzureAdConfig.NativeAppDetails.AppId;
            var userName = AzureAdConfig.TestUser.UserName;
            var password = AzureAdConfig.TestUser.Password;

            using (var webClient = new WebClient())
            {
                var requestParameters = new NameValueCollection();

                requestParameters.Add("resource", serviceUri);
                requestParameters.Add("client_id", clientID);
                requestParameters.Add("grant_type", "password");
                requestParameters.Add("username", userName);
                requestParameters.Add("password", password);
                //requestParameters.Add("scope", "openid");

                var url = $"https://login.microsoftonline.com/" + tenant + "/oauth2/token";
                var responsebytes = await webClient.UploadValuesTaskAsync(url, "POST", requestParameters);
                var responsebody = Encoding.UTF8.GetString(responsebytes);
            }
        }

        [Fact]
        public async Task ObtainToken_WebClientPost_NativeClient_PasswordGrantType2()
        {
            using (HttpClient client = new HttpClient())
            {
                string tenantId = AzureAdConfig.TenantId;
                var tokenEndpoint = @"https://login.windows.net/" + tenantId + "/oauth2/token";
                var accept = "application/json";
                string clientId = AzureAdConfig.WebApiAppDetails.AppId;

                client.DefaultRequestHeaders.Add("Accept", accept);
                string postBody = @"resource=" + AzureAdConfig.WebApiAppDetails.AppUriId + "&client_id=" + clientId + "&grant_type=password&username=" + AzureAdConfig.TestUser.UserName + "&password=" + AzureAdConfig.TestUser.Password + "&scope=openid";

                using (var response = await client.PostAsync(tokenEndpoint, new StringContent(postBody, Encoding.UTF8, "application/x-www-form-urlencoded")))
                {

                    response.EnsureSuccessStatusCode();
                    response.StatusCode.Should().Be(HttpStatusCode.OK);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseAsString = await response.Content.ReadAsStringAsync();
                        var jsonresult = JObject.Parse(responseAsString);
                        var token = (string)jsonresult["access_token"];

                        token.Should().NotBeNullOrWhiteSpace();
                  
                    }
                }
            }
        }


        [Fact]
        public async Task ObtainToken_WebClientPost_NativeClient_PasswordGrantType3()
        {

            string hardcodedUsername = AzureAdConfig.TestUser.UserName;
            string hardcodedPassword = AzureAdConfig.TestUser.Password;

            string resourceId = "https://vitalsigyn.com/WebApplicationAAD";

            string tenant = AzureAdConfig.TenantDomain;
            string tenantId = AzureAdConfig.TenantId;
            string clientId = AzureAdConfig.NativeAppDetails.AppId;
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
        public async Task GetAccessToken()
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
                requestParameters.Add("scope", "openid");

                var url = $"https://login.microsoftonline.com/" + AzureAdConfig.TenantDomain + "/oauth2/token";
                var responsebytes = webClient.UploadValues(url, "POST", requestParameters);
                var responsebody = Encoding.UTF8.GetString(responsebytes);
                var jsonresult = JObject.Parse(responsebody);
                token = (string)jsonresult["access_token"];
            }

            token.Should().NotBeNullOrWhiteSpace();
        }

    }
}
