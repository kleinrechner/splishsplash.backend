using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions
{
    public interface IHubClientConnectionService : IDisposable
    {
        Task StartConnectionAsync(CancellationToken cancellationToken);

        Task StopConnectionAsync(CancellationToken cancellationToken);
    }
}
