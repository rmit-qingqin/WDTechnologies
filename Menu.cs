using Microsoft.Extensions.Configuration;

namespace WebServiceAndDatabaseExample
{
    public static class Program
    {
        /*private static IConfigurationRoot Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        private static string ConnectionString { get; } = Configuration["ConnectionString"];*/

        private static void Main()
        {
            Menu menu = new Menu();
            menu.Run();

        }
    }
}
