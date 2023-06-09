﻿using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;

namespace STech.Services.Employee.Api.BackgroundServices
{
    public class BackgroundServiceStarter<T> : IHostedService where T : IHostedService
    {
        readonly T backgroundService;

        public BackgroundServiceStarter(T backgroundService)
        {
            this.backgroundService = backgroundService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return backgroundService.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return backgroundService.StopAsync(cancellationToken);
        }
    }
}
