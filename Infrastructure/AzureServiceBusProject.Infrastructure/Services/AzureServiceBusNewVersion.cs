using Azure.Messaging.ServiceBus;
using AzureServiceBusProject.Application.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace AzureServiceBusProject.Infrastructure.Services
{
    public class AzureServiceBusNewVersion: IServiceBus
    {
        private readonly ServicesModel servicesModel;
        static ServiceBusClient client;
        static ServiceBusSender sender;
        static ServiceBusProcessor processor;
        public AzureServiceBusNewVersion(ServicesModel serviceModel)
        {
            this.servicesModel = serviceModel;
            client = new ServiceBusClient(servicesModel.AzureServices.AzureConnectionString);
        }
        public AzureServiceBusNewVersion()
        {
           client.DisposeAsync();
        }

        public async void SendMessageToCreateQueueAsync(object messageContent)
        {

            sender = client.CreateSender(servicesModel.AzureServices.OrderCreatedQueue);

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            messageBatch.TryAddMessage(new ServiceBusMessage(JsonConvert.SerializeObject(messageContent)));
            await sender.SendMessagesAsync(messageBatch);

            await sender.DisposeAsync();
        }

        public async void SendMessageToDeleteQueueAsync(object messageContent)
        {

            sender = client.CreateSender(servicesModel.AzureServices.OrderDeletedQueue);

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            messageBatch.TryAddMessage(new ServiceBusMessage(JsonConvert.SerializeObject(messageContent)));
            await sender.SendMessagesAsync(messageBatch);

            await sender.DisposeAsync();
        }

        public async void GetMessageFromCreateQueue()
        {
           
            processor = client.CreateProcessor(servicesModel.AzureServices.OrderCreatedQueue, new ServiceBusProcessorOptions());
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;
            await processor.StartProcessingAsync();
            await processor.StopProcessingAsync();

            await processor.DisposeAsync();
           

        }

        public async void GetMessageFromDeleteQueue()
        {
            processor = client.CreateProcessor(servicesModel.AzureServices.OrderDeletedQueue, new ServiceBusProcessorOptions());
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;
            await processor.StartProcessingAsync().ConfigureAwait(false);

            //await processor.StopProcessingAsync();

            //await processor.CloseAsync();
            //await processor.DisposeAsync(); 
        }

   
        static Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body}");
            
            // complete the message. messages is deleted from the queue. 
            //await args.CompleteMessageAsync(args.Message);
            return Task.CompletedTask;
        }

        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
         
            return Task.CompletedTask;

        }

    }
}
