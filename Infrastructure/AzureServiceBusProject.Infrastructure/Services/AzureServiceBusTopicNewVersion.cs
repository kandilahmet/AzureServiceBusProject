using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AzureServiceBusProject.Application.Interfaces;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace AzureServiceBusProject.Infrastructure.Services
{
    public class AzureServiceBusTopicNewVersion : IServiceBusTopic
    {
        private readonly ServicesModel servicesModel;
        private static ServiceBusClient client;
        private static ServiceBusAdministrationClient adminClient;
        private static ServiceBusSender sender;
        public AzureServiceBusTopicNewVersion(ServicesModel servicesModel)
        {
            this.servicesModel = servicesModel;
            adminClient = new ServiceBusAdministrationClient(connectionString: servicesModel.AzureServices.AzureConnectionString);
            client = new ServiceBusClient(servicesModel.AzureServices.AzureConnectionString);
        }

        public async Task SendMessageToTopicAsync(string topicName,string filter ,object messageContent)
        {
            sender = client.CreateSender(topicName);

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
            ServiceBusMessage message = new();

            if (!string.IsNullOrEmpty(filter))
            {
                message.ApplicationProperties.Add("OrderType", filter);
                
            }
            message.Body = BinaryData.FromBytes(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent)));

            messageBatch.TryAddMessage(message); 

            await sender.SendMessagesAsync(messageBatch);
            await sender.CloseAsync();
            await sender.DisposeAsync();
        }
        public async Task SendMessageToOrderTopicCreatedSubscriptionAsync(object messageContent)
        {
            await this.CreateTopicIfNotExistsAsync(servicesModel.AzureServices.OrderTopic);

            await this.CreateSubscriptionIfNotExistsAsync(servicesModel.AzureServices.OrderTopic
                , servicesModel.AzureServices.OrderCreatedSubscription);

            await this.CreateFilteIfNotExistsAsync(servicesModel.AzureServices.OrderTopic
                , servicesModel.AzureServices.OrderCreatedSubscription
                , servicesModel.AzureServices.OrderCreatedFilter);

            await SendMessageToTopicAsync(servicesModel.AzureServices.OrderTopic
               // , servicesModel.AzureServices.OrderCreatedSubscription
                , servicesModel.AzureServices.OrderCreatedFilter
                , messageContent
                );
        }
        public async Task SendMessageToOrderTopicDeletedSubscriptionAsync(object messageContent)
        {
            await this.CreateTopicIfNotExistsAsync(servicesModel.AzureServices.OrderTopic);

            await this.CreateSubscriptionIfNotExistsAsync(servicesModel.AzureServices.OrderTopic
                , servicesModel.AzureServices.OrderDeletedSubscription);

            await this.CreateFilteIfNotExistsAsync(servicesModel.AzureServices.OrderTopic
                , servicesModel.AzureServices.OrderDeletedSubscription
                , servicesModel.AzureServices.OrderDeletedFilter);

            await SendMessageToTopicAsync(servicesModel.AzureServices.OrderTopic
               // , servicesModel.AzureServices.OrderDeletedSubscription
                , servicesModel.AzureServices.OrderDeletedFilter
                , messageContent
                );
        }
        public async Task CreateTopicIfNotExistsAsync(string topicName)
        {
            if (!await adminClient.TopicExistsAsync(topicName))
            {
                await adminClient.CreateTopicAsync(topicName);
            }
        }
        public async Task CreateSubscriptionIfNotExistsAsync(string topicName, string subscription)
        {
            if (!await adminClient.SubscriptionExistsAsync(topicName, subscription))
            {
                await adminClient.CreateSubscriptionAsync(topicName, subscription);
            }
        }
        public async Task CreateFilteIfNotExistsAsync(string topicName, string subscription, string filter)
        {
            CorrelationRuleFilter correlaCorrelationRuleFilter = new();
            correlaCorrelationRuleFilter.ApplicationProperties.Add("OrderType", filter);

            CreateRuleOptions rule = new();
            rule.Name = filter + "Rule";
            rule.Filter = correlaCorrelationRuleFilter;
            if (!await adminClient.RuleExistsAsync(topicName, subscription, rule.Name))
            {
                await adminClient.CreateRuleAsync(topicName, subscription, rule);
            }

        }
        public async Task<T> GetMessageFromTopicAsync<T>(string topic, string subscription)
        {
            var options = new ServiceBusReceiverOptions()
            {

            };
            ServiceBusReceiver serviceBusReceiver = client.CreateReceiver(topic, subscription, options);
            ServiceBusReceivedMessage serviceBusReceivedMessage = await serviceBusReceiver.ReceiveMessageAsync();
            await serviceBusReceiver.CloseAsync();
            await serviceBusReceiver.DisposeAsync();
            return await Task.FromResult(JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(serviceBusReceivedMessage.Body)));

        }
        public async Task<T> GetMessageFromOrderTopicCreatedSubscriptionAsync<T>()
        {
            return await GetMessageFromTopicAsync<T>(servicesModel.AzureServices.OrderTopic
                  , servicesModel.AzureServices.OrderCreatedSubscription);
        }
        public async Task<T> GetMessageFromOrderTopicDeletedSubscriptionAsync<T>()
        {
            return await GetMessageFromTopicAsync<T>(servicesModel.AzureServices.OrderTopic
                  , servicesModel.AzureServices.OrderDeletedSubscription);
        }
    }
}
