using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Voa.Cross.IoC;

namespace Voa.Robo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(NativeInjectorBootStrapper.RegisterServices)
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}