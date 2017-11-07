using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Specialized;
using System.Text;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            string tenantId = "3bd0245c-cac9-4dc2-bb49-15371698af05";
            var userName = "integration_test@vitalsigyn.com";
            var password = "V1t4alS1gyn";
            string clientId = "a1d51587-bf4c-4915-b588-8df1d9fd7ac9";

            AuthenticationContext authenticationContext =
                new AuthenticationContext("https://login.windows.net/" + tenantId + "/", false);
            
            AuthenticationResult res = await authenticationContext.AcquireTokenAsync(
                "https://vitalsigyn.com/WebApplicationAAD", 
                clientId, 
                new UserPasswordCredential(userName, password)
                );

            Assert.IsFalse(string.IsNullOrEmpty(res.AccessToken));

        }

        [TestMethod]
        public async Task TestMethod2()
        {
            //3bd0245c-cac9-4dc2-bb49-15371698af05
            //  Constants
            var tenant = "vitalsigyn.onmicrosoft.com";
            var serviceUri = "https://vitalsigyn.com/WebApplicationAAD";
            var clientID = "a1d51587-bf4c-4915-b588-8df1d9fd7ac9";
            var userName = "integration_test@vitalsigyn.com";
            var password = "V1t4alS1gyn";

            //  Ceremony
            var authority = "https://login.microsoftonline.com/" + tenant;
            var authContext = new AuthenticationContext(authority);
            var credentials = new UserPasswordCredential(userName, password);
            var authResult = await authContext.AcquireTokenAsync(serviceUri, clientID, credentials);
        }

        [TestMethod]
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