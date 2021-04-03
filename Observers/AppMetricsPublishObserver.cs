using System;
using System.Threading.Tasks;
using App.Metrics;
using MassTransit;

namespace PlayTimeX.MassTransit.AppMetrics.Observers
{
    /// <summary>
    /// 
    /// </summary>
    public class AppMetricsPublishObserver : IPublishObserver
    {
        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsPublishObserver(IMetrics metrics)
        {
            _metrics = metrics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task PrePublish<T>(PublishContext<T> context)
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
        public Task PostPublish<T>(PublishContext<T> context)
            where T : class
        {
            _metrics.MeasurePublish<T>();

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public Task PublishFault<T>(PublishContext<T> context, Exception exception)
            where T : class
        {
            _metrics.MeasurePublish<T>(exception);

            return Task.CompletedTask;
        }
    }
}