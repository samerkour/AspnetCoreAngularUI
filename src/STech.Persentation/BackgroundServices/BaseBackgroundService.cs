using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace STech.Presentation.Api.BackgroundServices
{
    public abstract class BaseBackgroundService : BackgroundService
    {
        protected readonly IServiceScopeFactory _serviceScopeFactory;
        protected readonly IBus _busControl;
        protected readonly ILogger<BaseBackgroundService> _logger;

        public BaseBackgroundService(
        
          IServiceScopeFactory serviceScopeFactory,
          IBus busControl,
          ILogger<BaseBackgroundService> logger)
        {
           
            _serviceScopeFactory = serviceScopeFactory;
            _busControl = busControl;
            _logger = logger;
  
        }

      

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            
            //await _busControl.StartAsync(cancellationToken);

            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            //await _busControl.StopAsync(cancellationToken);

            await base.StopAsync(cancellationToken);
        }
    }
}
