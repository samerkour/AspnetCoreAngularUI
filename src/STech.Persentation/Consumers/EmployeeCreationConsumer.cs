using MassTransit;
using System.Threading.Tasks;
using System;
using STech.Presentation.Api.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using STech.Presentation.Api.Common;
using Microsoft.Extensions.DependencyInjection;
using STech.Domain.Interfaces;
using STech.Infrastructure.Context;
using System.Text.Json;
using STech.Presentation.Api.Dtos.Employee.Api;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using STech.Infrastructure;
using System.Threading;
using System.Diagnostics;

namespace STech.Presentation.Api.Consumers
{
    public class EmployeeCreationConsumer : IConsumer<EmployeeCreated>
    {

        protected readonly IServiceScopeFactory _serviceScopeFactory;
        protected readonly IDistributedCache _cache;
        protected ILogger<EmployeeCreationConsumer> _logger;

        public EmployeeCreationConsumer(IServiceScopeFactory serviceScopeFactory, IDistributedCache cache, ILogger<EmployeeCreationConsumer> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _cache = cache;
            _logger = logger;
        }


        /// <summary>
        /// Recieve Employee.ApiCreationEvent from Blockchain service.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<EmployeeCreated> context)
        {


            //var jsonMessage = JsonSerializer.Serialize(context.Message);
            //Console.WriteLine($"Employee.ApiCreated message: {jsonMessage}");

            using (var scope = _serviceScopeFactory.CreateScope())
            {

                IUnitOfWork<PortEmployeesDbContext> _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork<PortEmployeesDbContext>>();


                if (context.Message.Meta.isError)
                {
                    _logger.LogError($"{context.Message.Meta.ResponseException?.message}");

                //ToDo
                //Add events to database tables
                }
                else
                    {


                        Guid EmployeeId = Guid.NewGuid();

                        //ToDo
                        //Add events to database tables
                       
                        var saved = await _unitOfWork.SaveChangesAsync();

                        //in case of success, you can do other operation like data cashing
                        if (saved > 0)
                        {
                            var content = Encoding.UTF8.GetBytes(
                                JsonSerializer.Serialize(
                                    new EmployeeCacheDto
                                    {
                                        EmployeeId = EmployeeId,
                                        EmployeeAddress = context.Message.Data.result.EmployeeAddress,
                                    }));

                            await _cache.SetAsync(
                                    "Employee_" + context.Message.Data.result.Id, 
                                    content, 
                                    new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromDays(30) }
                                );
                        }


                    }


            }

           

        }


    }


    public class EmployeeCreationConsumerDefinition : ConsumerDefinition<EmployeeCreationConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, 
            IConsumerConfigurator<EmployeeCreationConsumer> consumerConfigurator)
        {
            //base.ConfigureConsumer(endpointConfigurator, consumerConfigurator);

            endpointConfigurator.ConfigureConsumeTopology = false;

            endpointConfigurator.ClearSerialization();
            endpointConfigurator.UseRawJsonSerializer();

            if(endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rabbit)
            {
                rabbit.Bind("ExternalService.Employee.ApiService.EmployeeCreated"); 
            }
        }
    }

      
}
