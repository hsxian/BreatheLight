using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BreatheLight
{
    public class Program
    {
        public static void Main(string[] args)
        {

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            if (!File.Exists("appsettings.json"))
            {
                throw new FileNotFoundException();
            }
            var host = WebHost.CreateDefaultBuilder(args)
                //.UseUrls("http://localhost:4567")
                .UseStartup<Startup>();
            return host;
        }
    }
}
