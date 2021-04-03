using System;
using System.Threading.Tasks;
using App.Metrics;
using MassTransit;
using MassTransit.Courier;

namespace PlayTimeX.MassTransit.AppMetrics.Observers
{
    /// <summary>
    /// 
    /// </summary>
    public class AppMetricsActivityObserver :
        IActivityObserver
    {
        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsActivityObserver(IMetrics metrics)
        {
            _metrics = metrics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TActivity"></typeparam>
        /// <typeparam name="TArguments"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task PreExecute<TActivity, TArguments>(ExecuteActivityContext<TActivity, TArguments> context)
            where TActivity : class, IExecuteActivity<TArguments>
            where TArguments : class
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TActivity"></typeparam>
        /// <typeparam name="TArguments"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task PostExecute<TActivity, TArguments>(ExecuteActivityContext<TActivity, TArguments> context)
            where TActivity : class, IExecuteActivity<TArguments>
            where TArguments : class
        {
            _metrics.MeasureExecute(context);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TActivity"></typeparam>
        /// <typeparam name="TArguments"></typeparam>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public Task ExecuteFault<TActivity, TArguments>(ExecuteActivityContext<TActivity, TArguments> context, Exception exception)
            where TActivity : class, IExecuteActivity<TArguments>
            where TArguments : class
        {
            _metrics.MeasureExecute(context, exception);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TActivity"></typeparam>
        /// <typeparam name="TLog"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task PreCompensate<TActivity, TLog>(CompensateActivityContext<TActivity, TLog> context)
            where TActivity : class, ICompensateActivity<TLog>
            where TLog : class
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TActivity"></typeparam>
        /// <typeparam name="TLog"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task PostCompensate<TActivity, TLog>(CompensateActivityContext<TActivity, TLog> context)
            where TActivity : class, ICompensateActivity<TLog>
            where TLog : class
        {
            _metrics.MeasureCompensate(context);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TActivity"></typeparam>
        /// <typeparam name="TLog"></typeparam>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public Task CompensateFail<TActivity, TLog>(CompensateActivityContext<TActivity, TLog> context, Exception exception)
            where TActivity : class, ICompensateActivity<TLog>
            where TLog : class
        {
            _metrics.MeasureCompensate(context, exception);

            return Task.CompletedTask;
        }
    }
}