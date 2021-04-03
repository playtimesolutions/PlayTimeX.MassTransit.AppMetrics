using App.Metrics;
using MassTransit;
using MassTransit.EndpointConfigurators;
using PlayTimeX.MassTransit.AppMetrics.Configuration;

namespace PlayTimeX.MassTransit.AppMetrics.Observers
{
    /// <summary>
    /// 
    /// </summary>
    public class AppMetricsReceiveEndpointConfiguratorObserver :
        IEndpointConfigurationObserver
    {

        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsReceiveEndpointConfiguratorObserver(IMetrics metrics)
        {
            _metrics = metrics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configurator"></param>
        public void EndpointConfigured<T>(T configurator)
            where T : IReceiveEndpointConfigurator
        {
            var specification = new AppMetricsReceiveSpecification(_metrics);

            configurator.ConfigureReceive(r => r.AddPipeSpecification(specification));

            configurator.ConnectReceiveEndpointObserver(new AppMetricsReceiveEndpointObserver(_metrics));
        }
    }
}