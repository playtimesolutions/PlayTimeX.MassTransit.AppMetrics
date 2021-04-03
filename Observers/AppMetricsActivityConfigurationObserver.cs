using System;
using App.Metrics;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Courier;
using PlayTimeX.MassTransit.AppMetrics.Configuration;

namespace PlayTimeX.MassTransit.AppMetrics.Observers
{
    /// <summary>
    /// 
    /// </summary>
    public class AppMetricsActivityConfigurationObserver :
        IActivityConfigurationObserver
    {
        readonly IActivityObserver _observer;

        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsActivityConfigurationObserver(IMetrics metrics)
        {
            _metrics = metrics;
            _observer = new AppMetricsActivityObserver(metrics);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TActivity"></typeparam>
        /// <typeparam name="TArguments"></typeparam>
        /// <param name="configurator"></param>
        /// <param name="compensateAddress"></param>
        public void ActivityConfigured<TActivity, TArguments>(IExecuteActivityConfigurator<TActivity, TArguments> configurator, Uri compensateAddress)
            where TActivity : class, IExecuteActivity<TArguments>
            where TArguments : class
        {
            var specification = new AppMetricsExecuteActivitySpecification<TActivity, TArguments>(_metrics);

            configurator.AddPipeSpecification(specification);

            configurator.ConnectActivityObserver(_observer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TActivity"></typeparam>
        /// <typeparam name="TArguments"></typeparam>
        /// <param name="configurator"></param>
        public void ExecuteActivityConfigured<TActivity, TArguments>(IExecuteActivityConfigurator<TActivity, TArguments> configurator)
            where TActivity : class, IExecuteActivity<TArguments>
            where TArguments : class
        {
            var specification = new AppMetricsExecuteActivitySpecification<TActivity, TArguments>(_metrics);

            configurator.AddPipeSpecification(specification);

            configurator.ConnectActivityObserver(_observer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TActivity"></typeparam>
        /// <typeparam name="TLog"></typeparam>
        /// <param name="configurator"></param>
        public void CompensateActivityConfigured<TActivity, TLog>(ICompensateActivityConfigurator<TActivity, TLog> configurator)
            where TActivity : class, ICompensateActivity<TLog>
            where TLog : class
        {
            var specification = new AppMetricsCompensateActivitySpecification<TActivity, TLog>(_metrics);

            configurator.AddPipeSpecification(specification);

            configurator.ConnectActivityObserver(_observer);
        }
    }
}