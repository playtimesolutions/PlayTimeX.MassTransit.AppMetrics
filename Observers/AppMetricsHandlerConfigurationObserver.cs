using App.Metrics;
using MassTransit.ConsumeConfigurators;
using PlayTimeX.MassTransit.AppMetrics.Configuration;

namespace PlayTimeX.MassTransit.AppMetrics.Observers
{
    /// <summary>
    /// 
    /// </summary>
    public class AppMetricsHandlerConfigurationObserver :
        IHandlerConfigurationObserver
    {
        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsHandlerConfigurationObserver(IMetrics metrics)
        {
            _metrics = metrics;
        }

        void IHandlerConfigurationObserver.HandlerConfigured<T>(IHandlerConfigurator<T> configurator)
        {
            var specification = new AppMetricsHandlerSpecification<T>(_metrics);

            configurator.AddPipeSpecification(specification);
        }
    }
}