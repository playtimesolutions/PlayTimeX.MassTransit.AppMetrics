using System;
using System.Threading.Tasks;
using App.Metrics;
using MassTransit;

namespace PlayTimeX.MassTransit.AppMetrics.Observers
{
    /// <summary>
    /// 
    /// </summary>
    public class AppMetricsBusObserver :
        IBusObserver
    {
        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsBusObserver(IMetrics metrics)
        {
            _metrics = metrics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bus"></param>
        /// <returns></returns>
        public Task PostCreate(IBus bus)
        {
            bus.ConnectPublishObserver(new AppMetricsPublishObserver(_metrics));
            bus.ConnectSendObserver(new AppMetricsSendObserver(_metrics));
            bus.ConnectReceiveObserver(new AppMetricsReceiveObserver(_metrics));

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public Task CreateFaulted(Exception exception)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bus"></param>
        /// <returns></returns>
        public Task PreStart(IBus bus)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="busReady"></param>
        /// <returns></returns>
        public Task PostStart(IBus bus, Task<BusReady> busReady)
        {
            _metrics.BusStarted();

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public Task StartFaulted(IBus bus, Exception exception)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bus"></param>
        /// <returns></returns>
        public Task PreStop(IBus bus)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bus"></param>
        /// <returns></returns>
        public Task PostStop(IBus bus)
        {
            _metrics.BusStopped();

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public Task StopFaulted(IBus bus, Exception exception)
        {
            _metrics.BusStopped();

            return Task.CompletedTask;
        }
    }
}