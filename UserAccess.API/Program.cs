using Autofac.Extensions.DependencyInjection;
using System.Transactions;

namespace UserAccess.API
{
    public class Program
    {   
        public static void Main(string[] args)
        {
            TransactionManager.ImplicitDistributedTransactions = true;
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(
                    webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }

}
