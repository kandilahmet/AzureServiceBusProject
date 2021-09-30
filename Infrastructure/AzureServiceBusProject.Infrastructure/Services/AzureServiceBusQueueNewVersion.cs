using Azure.Messaging.ServiceBus;
using AzureServiceBusProject.Application.Interfaces;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;


namespace AzureServiceBusProject.Infrastructure.Services
{
    public class AzureServiceBusQueueNewVersion : IServiceBusQueue
    {
        private readonly ServicesModel servicesModel;
        static ServiceBusClient client;
        static ServiceBusSender sender;
        static ServiceBusProcessor processor;
        public AzureServiceBusQueueNewVersion(ServicesModel serviceModel)
        {
            this.servicesModel = serviceModel;
            client = new ServiceBusClient(servicesModel.AzureServices.AzureConnectionString);
        }
        ~AzureServiceBusQueueNewVersion()
        {
            client.DisposeAsync();
            processor.DisposeAsync();
        }


        public async Task SendMessageToCreateQueueAsync(object messageContent)
        {

            sender = client.CreateSender(servicesModel.AzureServices.OrderCreatedQueue);

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            messageBatch.TryAddMessage(new ServiceBusMessage(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent))));

            await sender.SendMessagesAsync(messageBatch);

            await sender.CloseAsync();
            await sender.DisposeAsync();

        }

        public async Task SendMessageToDeleteQueueAsync(object messageContent)
        {

            sender = client.CreateSender(servicesModel.AzureServices.OrderDeletedQueue);

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            messageBatch.TryAddMessage(new ServiceBusMessage(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent))));

            await sender.SendMessagesAsync(messageBatch);


            await sender.DisposeAsync();
        }


        public async Task<T> GetMessageFromCreateQueueAsync<T>()
        {
            ////Processor ile çalıştığımızda kullanıyoruz
            //processor = client.CreateProcessor(servicesModel.AzureServices.OrderCreatedQueue, new ServiceBusProcessorOptions());
            //processor.ProcessMessageAsync +=  MessageHandler<GetMessageCreatreDeleteQueueViewModel>;
            //processor.ProcessErrorAsync += ErrorHandler;
            //await processor.StartProcessingAsync();
            //await processor.StopProcessingAsync();

            //await processor.DisposeAsync();


            ServiceBusReceiver receiver = client.CreateReceiver(servicesModel.AzureServices.OrderCreatedQueue);
            ServiceBusReceivedMessage serviceBusReceivedMessage = await receiver.ReceiveMessageAsync();
            await receiver.CompleteMessageAsync(serviceBusReceivedMessage);
            await receiver.CloseAsync();
            await receiver.DisposeAsync();
            return await Task.FromResult<T>(JsonConvert.DeserializeObject<T>(UTF8Encoding.UTF8.GetString(serviceBusReceivedMessage.Body)));
        }

        public async Task<T> GetMessageFromDeleteQueueAsync<T>()
        {
            ServiceBusReceiver receiver = client.CreateReceiver(servicesModel.AzureServices.OrderCreatedQueue);
            ServiceBusReceivedMessage serviceBusReceivedMessage = await receiver.ReceiveMessageAsync();

            await receiver.CompleteMessageAsync(serviceBusReceivedMessage);
            await receiver.CloseAsync();
            await receiver.DisposeAsync();
            return await Task.FromResult<T>(JsonConvert.DeserializeObject<T>(UTF8Encoding.UTF8.GetString(serviceBusReceivedMessage.Body)));
        }


        public async void GetStartMessageFromDeleteQueue<T>()
        {
            //Processor ile çalıştığımızda kullanıyoruz
            processor = client.CreateProcessor(servicesModel.AzureServices.OrderDeletedQueue, new ServiceBusProcessorOptions());
            processor.ProcessMessageAsync += MessageHandler<T>;
            processor.ProcessErrorAsync += ErrorHandler;
            await processor.StartProcessingAsync().ConfigureAwait(false);

            //await processor.StopProcessingAsync();

            //await processor.CloseAsync();
        }

        public async void GetStartMessageFromCreateQueue<T>()
        {
            //Processor ile çalıştığımızda kullanıyoruz
            processor = client.CreateProcessor(servicesModel.AzureServices.OrderCreatedQueue, new ServiceBusProcessorOptions());
            processor.ProcessMessageAsync += MessageHandler<T>;
            processor.ProcessErrorAsync += ErrorHandler;
            await processor.StartProcessingAsync().ConfigureAwait(false);

            //await processor.StopProcessingAsync();

            //await processor.CloseAsync();
        }
        static Task<T> MessageHandler<T>(ProcessMessageEventArgs args)
        { 
            T result = JsonConvert.DeserializeObject<T>(UTF8Encoding.UTF8.GetString(args.Message.Body));
            //  string body = args.Message.Body.ToString();

            Console.WriteLine($"Received messge : {UTF8Encoding.UTF8.GetString(args.Message.Body)}"); 

            // complete the message. messages is deleted from the queue. 
            //await args.CompleteMessageAsync(args.Message);
            return Task.FromResult<T>(result);
        }
        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());

            return Task.CompletedTask;

        }

         
    }
}
