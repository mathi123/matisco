using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Autofac.Extensions.DependencyInjection;

namespace Matisco.Server.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost<FrameworkServerStartup>(args).Run();
        }

        public static void Main<T>(string[] args) where T : FrameworkServerStartup
        {
            BuildWebHost<T>(args).Run();
        }

        public static IWebHost BuildWebHost<T>(string[] args) where T : FrameworkServerStartup =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<T>()
                .Build();
    }
}
