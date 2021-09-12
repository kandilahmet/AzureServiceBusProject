using AzureServiceBusProject.Application.Interfaces;
using AzureServiceBusProject.Infrastructure.Services;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text; 

namespace AzureServiceBusProject.Infrastructure
{
    public static class ServiceRegistration
    {
       public static void AddInfrastructureServices(this IServiceCollection services)
        {
            //services.AddSingleton<IServiceBus, AzureServiceBusOldVersion>();//Eski versiyon mesaj gönderim yapısı
            services.AddSingleton<IServiceBus, AzureServiceBusNewVersion>();//Yeni versiyon mesaj gönderim yapısı

            //Farklı bir Proje/Class Library den appsettings.json dosyasındaki verileri okuma eylemi

            // .json(farklı uzanltılı .xml dosyada olabilir) dosyasını ele alması için bir ConfigurationBuilder örneği oluşturuyoruz. 
            //string directory = Directory.GetCurrentDirectory();
            string directory = @"C:\MyPersonalWorkspace\NetCore\AzureServiceBusProject\Presentation\AzureServiceBusProject.API";
            
            var configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(directory, "../AzureServiceBusProject.API"))
                    .AddJsonFile("appsettings.json");
            //Build çağrısı sonucu IConfigurationRoot arayüzü üzerinden taşınabilecek bir nesne örneği elde ediyoruz.
            IConfigurationRoot configurationRoot = configurationBuilder.Build();
            //GetSection ile alınan veriler IoC yapılanmasına ekleniyor/enjekte ediliyor. 
            //(AzureServiceBus.cs de enjecte edilen ServiceMode'in kullanmı var.)
            var serviceModel = configurationRoot.GetSection("Services").Get<ServicesModel>();
            //Yöntem 1
             services.AddSingleton(serviceModel);
            //Yöntem 2
            //Bu yöntemde oluyor fakat sanırım zaten .Get<ServicesModel>() ile yeni bir instance üretiliyor.
            //services.AddSingleton<ServicesModel>(x => new ServicesModel
            //{
            //    AzureConnectionString = serviceModel.AzureConnectionString
            //});

            services.AddSingleton<ManagementClient>(x => new ManagementClient(serviceModel.AzureServices.AzureConnectionString));
        }
    }
}
