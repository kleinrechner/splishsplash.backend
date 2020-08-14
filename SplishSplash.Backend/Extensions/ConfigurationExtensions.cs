using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Kleinrechner.SplishSplash.Backend.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddMultipleJsonFiles(this IConfigurationBuilder configurationBuilder, string path)
        {
            string[] files = System.IO.Directory.GetFiles(path, "*.json");

            foreach (var item in files)
            {
                configurationBuilder.AddJsonFile(item);
            }
            return configurationBuilder;
        }
    }
}
