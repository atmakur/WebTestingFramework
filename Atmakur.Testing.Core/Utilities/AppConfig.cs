using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Atmakur.Testing.Core.Utilities
{
    public static class AppConfig
    {
        public static bool UseSauceLabs { get; private set; }
        public static List<string> Capabilities { get; private set; }
        public static string InitUrl { get; private set; }
        public static string ParentTunnelId { get; private set; }
        public static string TunnelId { get; private set; }
        public static TimeSpan ElementLoadWaitTime { get; private set; }
        public static TimeSpan SeleniumCommandWaitTime { get; private set; }
        public static string AssemblyDirectory { get; private set; }
        public static string SL_AccessKey { get; private set; }
        public static string SL_UserName { get; private set; }
        public static string BuildId { get; private set; }
        public static string BuildName { get; private set; }
        public static string SauceRemoteDriverUri { get; private set; }
        public static string SeleniumGridDriverUri { get; private set; }
        public static bool UseSeleniumGrid { get; private set; }
        public static bool OutputResultsToFile { get; private set; }
        public static bool OutputResultsToConsole { get; private set; }
        public static string DataSourceConnectionString { get; private set; }
        public static string CurrentEnvironment { get; private set; }
        internal static List<BrowserConfigEntity> BrowsersEnabled { get; set; }

        static AppConfig()
        {
            AssemblyDirectory = GetDirectoryName();

            DataSourceConnectionString = ConfigurationManager.AppSettings["TestDataSource"];
            //Gets all required environment variables
            GetEnvironmentVariables();
            //Gets all required Configurations from MongoDB
            var configResult = GetConfigurations();
            //Assign the value to AppConfig class
            AssignAllVariables(configResult);

            if (UseSeleniumGrid && UseSauceLabs)
            {
                throw new ArgumentException("Please specify either UseSeleniumGrid or UseSauceLabs, not both at the same time.");
            }
        }

        private static string GetDirectoryName()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        private static void AssignAllVariables(ConfigEntity configResult)
        {
            UseSauceLabs = configResult.AppConfig.UseSauceLabs;
            UseSeleniumGrid = configResult.AppConfig.UseSeleniumGrid;
            BuildName = configResult.AppConfig.BuildName;
            ElementLoadWaitTime = TimeSpan.FromSeconds(configResult.AppConfig.ElementLoadWaitTime);
            InitUrl = configResult.AppConfig.InitUrl;
            OutputResultsToConsole = configResult.AppConfig.OutputResultsToConsole;
            OutputResultsToFile = configResult.AppConfig.OutputResultsToFile;
            SauceRemoteDriverUri = configResult.AppConfig.SauceRemoteDriverUri;
            SeleniumCommandWaitTime = TimeSpan.FromSeconds(configResult.AppConfig.SeleniumCommandWaitTime);
            SeleniumGridDriverUri = configResult.AppConfig.SeleniumGridDriverUri;
            Capabilities = configResult.AppConfig.Capabilities;
            BrowsersEnabled = configResult.Browsers.Where(x => x.IsEnabled).ToList();

            var tunnelValue = Environment.GetEnvironmentVariable(configResult.AppConfig.SL_Tunnel).Split(':');
            ParentTunnelId = tunnelValue[0];
            TunnelId = tunnelValue[1];
        }

        private static ConfigEntity GetConfigurations()
        {
            var query = JsonConvert.SerializeObject(new { Environment = CurrentEnvironment });
            IDataProvider dataProvider = new MongoDataProvider(DataSourceConnectionString);
            return dataProvider.GetSingleDocument<ConfigEntity>("Configurations", query);
        }

        private static void GetEnvironmentVariables()
        {
            CurrentEnvironment = Environment.GetEnvironmentVariable("TestEnvironment");
            SL_AccessKey = Environment.GetEnvironmentVariable("SL_AccessKey");
            SL_UserName = Environment.GetEnvironmentVariable("SL_Username");
            BuildId = Environment.GetEnvironmentVariable("BuildId");
        }
    }

    public class ConfigEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Environment { get; set; }
        public AppConfigEntity AppConfig { get; set; }
        public List<BrowserConfigEntity> Browsers { get; set; }
    }

    public class AppConfigEntity
    {
        public bool UseSauceLabs { get; private set; }
        public List<string> Capabilities { get; private set; }
        public string InitUrl { get; private set; }
        public int ElementLoadWaitTime { get; private set; }
        public int SeleniumCommandWaitTime { get; private set; }
        public string BuildName { get; private set; }
        public string SauceRemoteDriverUri { get; private set; }
        public string SeleniumGridDriverUri { get; private set; }
        public bool UseSeleniumGrid { get; private set; }
        public bool OutputResultsToFile { get; private set; }
        public bool OutputResultsToConsole { get; private set; }
        public string SL_Tunnel { get; private set; }
    }

    public class BrowserConfigEntity
    {
        public string Browser { get; set; }
        public string Version { get; set; }
        public string Platform { get; set; }
        public bool IsEnabled { get; set; }
    }
}
