using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using MassTransit;
using STech.Domain.Interfaces;
using STech.Infrastructure.Context;
using STech.Services.Employee.Api.Contracts;
using STech.Services.Employee.Api.Common;
using System.Text.Json;

namespace STech.Services.Employee.Api.BackgroundServices
{

    // // refere to https://www.programmingwithwolfgang.com/rabbitmq-in-an-asp-net-core-3-1-microservice/
    public class CreateEmployeeBackgroundService: BaseBackgroundService
    {

        protected static IList<Task> _clientTasks;

        public CreateEmployeeBackgroundService(
          
            IServiceScopeFactory serviceScopeFactory,
            IBus busControl,
            ILogger<CreateEmployeeBackgroundService> logger)
            : base(serviceScopeFactory, busControl, logger)
        {
            
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _logger.LogInformation($"RabbitMqReceiver Queue Tasks are Starting...");

            try
            {

               await Task.Factory.StartNew( () =>
                    {
                         Publish(stoppingToken);

                        _logger.LogInformation($"Publish Task is Started...");
                    } , TaskCreationOptions.LongRunning);


              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }


            await Task.CompletedTask;
        }

        private async void Publish(CancellationToken stoppingToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {

                IUnitOfWork<PortEmployeesDbContext> _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork<PortEmployeesDbContext>>();

                while (true)
                {
                    var msgs = await _unitOfWork.GetRepository<STech.Domain.Entities.EmployeeEntities.EmployeeCreatedEvent>().GetAsync(x =>
                       new STech.Domain.Entities.EmployeeEntities.EmployeeCreatedEvent
                       {
                           Id = x.Id,
                           OccurredOn = x.OccurredOn,
                           State = x.State,
                           Data=x.Data,
                           EventId = x.EventId,

                       },
                       filter => filter.State == (byte)EmployeeCreationStates.Initiated 
                       && filter.ProcessedOn == null
                   ).ContinueWith(async x =>
                   {
                       if (x.Result != null)
                       {
                           foreach (var item in x.Result)
                           {

                               _unitOfWork.GetRepository<STech.Domain.Entities.EmployeeEntities.EmployeeCreatedEvent>().UpdateFields(
                                                  new STech.Domain.Entities.EmployeeEntities.EmployeeCreatedEvent()
                                                  {
                                                      Id = item.Id,
                                                      //OccurredOn = DateTimeOffset.UtcNow,
                                                      //State = (int)Employee.ApiCreationStates.Created,
                                                      ProcessedOn = DateTimeOffset.UtcNow,
                                                      //UserId = context.Message.UserId.Value
                                                  },
                                                  //o => o.OccurredOn,
                                                  //o => o.State,
                                                  o => o.ProcessedOn
                                                  //o => o.UserId
                                                  );

                               var saved = await _unitOfWork.SaveChangesAsync();

                               if (saved > 0)
                               {


                                   var data = JsonSerializer.Deserialize<EmployeeCreationEventData>(item.Data);

                                   var EmployeeCreated = new EmployeeInitiated()
                                   {
                                       //Id = item.Id,
                                       OccurredOn = item.OccurredOn,
                                       //State = item.State,
                                       Data = data,
                                       //UserId = item.UserId
                                       EventId = item.EventId,
                                   };

                                   await _busControl.Publish(EmployeeCreated, stoppingToken);
                               }
                           }

                       }

                   });


                   Task.Delay(1000 * 2).Wait(); // Delay 2 seconds.

                }
            }

        }

    }
}
