using System.Threading.Tasks;
using App.Metrics;
using MassTransit;

namespace PlayTimeX.MassTransit.AppMetrics.Observers
{
    /// <summary>
    /// 
    /// </summary>
    public class AppMetricsReceiveEndpointObserver :
        IReceiveEndpointObserver
    {
        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsReceiveEndpointObserver(IMetrics metrics)
        {
            _metrics = metrics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ready"></param>
        /// <returns></returns>
        public Task Ready(ReceiveEndpointReady ready)
        {
            _metrics.EndpointReady(ready);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stopping"></param>
        /// <returns></returns>
        public Task Stopping(ReceiveEndpointStopping stopping)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="completed"></param>
        /// <returns></returns>
        public Task Completed(ReceiveEndpointCompleted completed)
        {
            _metrics.EndpointCompleted(completed);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="faulted"></param>
        /// <returns></returns>
        public Task Faulted(ReceiveEndpointFaulted faulted)
        {
            return Task.CompletedTask;
        }
    }
}