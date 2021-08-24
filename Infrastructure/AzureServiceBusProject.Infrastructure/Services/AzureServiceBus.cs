using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AzureServiceBusProject.Application.Interfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Microsoft.Azure.ServiceBus.Management;
using AzureServiceBusProject.Application.Events;
using System.Threading;

namespace AzureServiceBusProject.Infrastructure.Services
{   /// Azure Queue/Topic kullanma(kayıt atma-kayıt okuma-dinleme) işleri ayrı bir dll(Microsoft.Azure.ServiceBus)
    /// Management işleri Bu Queue/Topic oluşturulması temizlenmesi var olup olmadığı bilgilerinin kontrol edilmesi
    /// ayr bir dll de toplanmıştır(Microsoft.Azure.Management.ServiceBus).
    public class AzureServiceBus : IServiceBus

    {
        private readonly ServicesModel servicesModel;
        private readonly ManagementClient manageClient;

        //Yöndetm 1 - Bu yöntemle de havuzdan nesne örneğini alabiliyoruz.
        //public AzureServiceBus(IServiceProvider serviceProvider)
        //{
        //    //IoC yapılanmasında nesne havuzundan alınan ServiceModel nesnesi
        //    this.servicesModel = serviceProvider.GetService<ServicesModel>();
        //}

        //Yöntem 2
        public AzureServiceBus(ServicesModel serviceModel, ManagementClient manageClient)
        {
            //IoC yapılanmasında nesne havuzundan alınan ServiceModel nesnesi
            this.servicesModel = serviceModel;
            this.manageClient = manageClient;
        }


        public async Task SendMessageToQueueAsync(string queueName, object messageContent)
        {

            IQueueClient queueClient = new QueueClient(this.servicesModel.AzureServices.AzureConnectionString, queueName);

            var byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent));

            var message = new Message(byteArray);

            await queueClient.SendAsync(message);
        }
        public async void SendMessageToCreateQueueAsync(object messageContent)
        {
            var ifExists = await this.manageClient.QueueExistsAsync(this.servicesModel.AzureServices.OrderCreatedQueue);

            if (!ifExists)
            {
                await this.manageClient.CreateQueueAsync(this.servicesModel.AzureServices.OrderCreatedQueue);
            }

            await SendMessageToQueueAsync(this.servicesModel.AzureServices.OrderCreatedQueue, messageContent);
        }
        public async void SendMessageToDeleteQueueAsync(object messageContent)
        {
            var ifExists = await this.manageClient.QueueExistsAsync(this.servicesModel.AzureServices.OrderDeletedQueue);

            if (!ifExists)
            {
                await this.manageClient.CreateQueueAsync(this.servicesModel.AzureServices.OrderDeletedQueue);
            }
            await SendMessageToQueueAsync(this.servicesModel.AzureServices.OrderDeletedQueue, messageContent);
        }
         

        public void GetMessageFromQueue<T>(string queueName)
        {
            IQueueClient queueClient = new QueueClient(servicesModel.AzureServices.AzureConnectionString, queueName);
            //  queueClient.RegisterMessageHandler(OnProcess<T>, ExceptionReceivedHandler);
            queueClient.RegisterMessageHandler(OnProcess<T>, GetMessageHandlerOptions());
        }
        public void GetMessageFromDeleteQueue()
        {
            GetMessageFromQueue<OrderDeletedEvent>(servicesModel.AzureServices.OrderDeletedQueue);
        }
        public void GetMessageFromCreateQueue()
        {
            GetMessageFromQueue<OrderCreatedEvent>(servicesModel.AzureServices.OrderCreatedQueue);
        }
        static private MessageHandlerOptions GetMessageHandlerOptions()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                AutoComplete = false,
                MaxConcurrentCalls = 1,
            };

            return messageHandlerOptions;
        }
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");

            return Task.CompletedTask;
        }
        private static Task OnProcess<T>(Message message, CancellationToken cancellationToken)
        {
            var model = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));
            return Task.CompletedTask;
        }
    }
}
