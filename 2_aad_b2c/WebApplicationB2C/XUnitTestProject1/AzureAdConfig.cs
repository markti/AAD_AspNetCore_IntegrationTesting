using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestProject1
{
    public static class AzureAdConfig
    {
        public static string TenantId = "bb9b3927-77a8-4c77-bc4d-474d919910d2";
        public static string TenantDomain = "vitalsigynusers.onmicrosoft.com";

        public static class WebApiAppDetails
        {
            public static string AppId = "2cba42dc-e482-4d97-a370-c5abb5b33a79";
            public static string AppUriId = "https://vitalsigynusers.onmicrosoft.com/WebApplicationB2C";
            public static string AccessKey = "LojHZrAT3lZkjQZp/wDzf8b5TW/8R1thfJ4uAvt/i+w=";
        }

        public static class NativeAppDetails
        {
            public static string AppId = "deb4b552-5a71-4e10-b611-23777b1c5140";
        }

        public static class TestUser
        {
            public static string UserName = "integration_test@vitalsigynusers.onmicrosoft.com";
            public static string Password = "t5TE;$jVyPww{9JZ";
        }
    }
}