
using AzureServiceBusProject.Application.Interfaces;
using AzureServiceBusProject.Application.ViewModels;
using AzureServiceBusProject.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System; 
namespace AzureServiceBusProject.Consumer.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddInfrastructureServices();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            IServiceBus serviceBus = serviceProvider.GetService<IServiceBus>();

            serviceBus.GetStartMessageFromDeleteQueue<GetMessageFromDeleteQueueViewModel>();


            serviceBus.GetStartMessageFromCreateQueue<GetMessageFromCreateQueueViewModel>();

            System.Console.ReadLine();
        }
    }
}
