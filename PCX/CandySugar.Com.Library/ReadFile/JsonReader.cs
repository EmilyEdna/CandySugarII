using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace CandySugar.Com.Library.ReadFile
{
    public class JsonReader
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static void JsonRead(string dir, List<string> jsonFile)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dir);
            var builder = new ConfigurationBuilder().SetBasePath(path);
            jsonFile.ForEach(item =>
            {
                builder = builder.AddJsonFile(item);
            });
            Configuration = builder.Build();
        }
    }
}
