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

            IQueueClient queueClient = new QueueClient(this.servicesModel.AzureConnectionString, queueName);

            var byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent));

            var message = new Message(byteArray);

            await queueClient.SendAsync(message);
        }

        public async Task CreateQueueIfNotExits(string QueueName)
        {
            var ifExists = await this.manageClient.QueueExistsAsync(QueueName);

            if (!ifExists)
            {
                await this.manageClient.CreateQueueAsync(QueueName);
            }
        }

        public async Task DeleteQueueIfNotExits(string QueueName)
        {
            var ifExists = await this.manageClient.QueueExistsAsync(QueueName);

            if (!ifExists)
            {
                await this.manageClient.CreateQueueAsync(QueueName);
            }
        }

        public async Task GetMessageFromQueue<T>(string queueName, Action<T> receiveAction)
        {
            IQueueClient queueClient = new QueueClient(servicesModel.AzureConnectionString, queueName);
            queueClient.RegisterMessageHandler(async (message, cancellationToken) =>
            {
                var model = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));
                receiveAction(model);
                await Task.CompletedTask;
            },
            new MessageHandlerOptions(i => Task.CompletedTask)
            );  
        }

        public async Task GetMessageFromDeleteQueue(string queueName)
        {
            GetMessageFromQueue<OrderDeletedEvent>(queueName, i => Console.WriteLine(i.Id));
        }
    }
}
