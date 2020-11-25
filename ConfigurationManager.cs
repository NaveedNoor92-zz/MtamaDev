using Microsoft.Extensions.Configuration;
using System.IO;

namespace Mtama
{
    public static class ConfigurationManager
    {
        public static IConfiguration AppSetting { get; }
        static ConfigurationManager()
        {
            AppSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
        }

        public static string GetAppSetting(string path)
        {
            //var env = AppSetting[Constants.Settings_Environment];
            //return AppSetting[env + ":" + path];
            return AppSetting[path];
        }
    }
}
