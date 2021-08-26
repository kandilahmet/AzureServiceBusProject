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
    public class AzureServiceBusOldVersion : IServiceBus

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
        public AzureServiceBusOldVersion(ServicesModel serviceModel, ManagementClient manageClient)
        {
            //IoC yapılanmasında nesne havuzundan alınan ServiceModel nesnesi
            this.servicesModel = serviceModel;
            this.manageClient = manageClient;
        }

        /// <summary>
        /// Verilen Queue'ya mesajı gönderir
        /// </summary>
        /// <param name="queueName">hangi queue'ya mesaj gönderilecek</param>
        /// <param name="messageContent">gönderilecek mesaj</param>
        /// <returns></returns>
        public async Task SendMessageToQueueAsync(string queueName, object messageContent)
        {

            IQueueClient queueClient = new QueueClient(this.servicesModel.AzureServices.AzureConnectionString, queueName);

            var byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent));

            var message = new Message(byteArray);

            await queueClient.SendAsync(message);
        }
       
        public async void SendMessageToCreateQueueAsync(object messageContent)
        {
            //Belirtilen Queue var mı ? kontrol edilir.
            var ifExists = await this.manageClient.QueueExistsAsync(this.servicesModel.AzureServices.OrderCreatedQueue);

            if (!ifExists)
            {   //yok ise servisBus'da oluşturulur
                await this.manageClient.CreateQueueAsync(this.servicesModel.AzureServices.OrderCreatedQueue);
            }
            //Mesaj queue'ya gönderilir.
            await SendMessageToQueueAsync(this.servicesModel.AzureServices.OrderCreatedQueue, messageContent);
        }
        public async void SendMessageToDeleteQueueAsync(object messageContent)
        {
            //Belirtilen Queue var mı ? kontrol edilir.
            var ifExists = await this.manageClient.QueueExistsAsync(this.servicesModel.AzureServices.OrderDeletedQueue);

            if (!ifExists)
            {//yok ise servisBus'da oluşturulur
                await this.manageClient.CreateQueueAsync(this.servicesModel.AzureServices.OrderDeletedQueue);
            }
            //Mesaj queue'ya gönderilir.
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

     /// <summary>
     /// Mesaj alımı ıle ılgılı ayarlamaların yapıldığı kısım
     /// </summary>
     /// <returns></returns>
        static private MessageHandlerOptions GetMessageHandlerOptions()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                AutoComplete = false,//Mesaj alındıgında tüketildi olarak gözükmesin. üretilen mesaj 10(default rakam) denemede tüketilmez ise Dead letter kısmına atılacak.
                MaxConcurrentCalls = 1,
            };

            return messageHandlerOptions;
        }
        
        
        /// <summary>
        /// Mesaj'ın serviceBus'dan alınması sırasında hata alırsak çalışacak metod
        /// </summary>
        /// <param name="exceptionReceivedEventArgs"></param>
        /// <returns></returns>
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
      
       /// <summary>
       /// Bir mesajı işleyecek metod
       /// </summary>
       /// <typeparam name="T">Gelen mesajın dönüştürüleceği tip</typeparam>
       /// <param name="message">Gelen mesaj</param>
       /// <param name="cancellationToken"></param>
       /// <returns></returns>
        private static Task OnProcess<T>(Message message, CancellationToken cancellationToken)
        {
            var model = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));

            return Task.CompletedTask;
        }
    
    }
}
