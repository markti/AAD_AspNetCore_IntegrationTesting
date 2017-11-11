using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestProject1
{
    public static class AzureAdConfig
    {
        public static string TenantId = "3bd0245c-cac9-4dc2-bb49-15371698af05";
        public static string TenantDomain = "vitalsigyn.onmicrosoft.com";
        public static string TenantShortDomain = "vitalsigyn.com";

        public static class WebApiAppDetails
        {
            public static string AppId = "d476fd33-4cfc-4aeb-9d4d-ea4978af5660";
            public static string AppUriId = "https://vitalsigyn.com/WebApplicationAAD";
            public static string AccessKey = "LojHZrAT3lZkjQZp/wDzf8b5TW/8R1thfJ4uAvt/i+w=";
        }

        public static class NativeAppDetails
        {
            public static string AppId = "a1d51587-bf4c-4915-b588-8df1d9fd7ac9";
        }

        public static class TestUser
        {
            public static string UserName = "integration_test@vitalsigyn.com";
            public static string Password = "V1t4alS1gyn";
        }
    }
}
