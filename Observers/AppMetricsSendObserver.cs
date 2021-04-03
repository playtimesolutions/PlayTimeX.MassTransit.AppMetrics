using System;
using System.Threading.Tasks;
using App.Metrics;
using MassTransit;

namespace PlayTimeX.MassTransit.AppMetrics.Observers
{
    /// <summary>
    /// 
    /// </summary>
    public class AppMetricsSendObserver :
        ISendObserver
    {
        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsSendObserver(IMetrics metrics)
        {
            _metrics = metrics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task PreSend<T>(SendContext<T> context)
            where T : class
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task PostSend<T>(SendContext<T> context)
            where T : class
        {
            _metrics.MeasureSend<T>();

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public Task SendFault<T>(SendContext<T> context, Exception exception)
            where T : class
        {
            _metrics.MeasureSend<T>(exception);

            return Task.CompletedTask;
        }
    }
}