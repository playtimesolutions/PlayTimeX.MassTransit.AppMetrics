using System;
using System.Diagnostics;
using System.IO;
using App.Metrics;
using MassTransit;
using PlayTimeX.MassTransit.AppMetrics.Observers;

namespace PlayTimeX.MassTransit.AppMetrics.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class AppMetricsConfigurationExtensions
    {
        /// <summary>
        /// Configure the bus to capture metrics for publication using Prometheus.
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="metrics"></param>
        /// <param name="configureOptions"></param>
        /// <param name="serviceName">
        /// The service name for metrics reporting, defaults to the current process main module filename
        /// </param>
        public static void UseMassTransitAppMetrics(this IBusFactoryConfigurator configurator, IMetrics metrics,
            Action<MassTransitMetricsOptions> configureOptions = null,
            string serviceName = default)
        {
            var options = MassTransitMetricsOptions.CreateDefault();

            configureOptions?.Invoke(options);

            MassTransitMetricsRegistry.TryConfigure(GetServiceName(serviceName), options);

            configurator.ConnectConsumerConfigurationObserver(new AppMetricsConsumerConfigurationObserver(metrics));
            configurator.ConnectHandlerConfigurationObserver(new AppMetricsHandlerConfigurationObserver(metrics));
            configurator.ConnectSagaConfigurationObserver(new AppMetricsSagaConfigurationObserver(metrics));
            configurator.ConnectActivityConfigurationObserver(new AppMetricsActivityConfigurationObserver(metrics));
            configurator.ConnectEndpointConfigurationObserver(new AppMetricsReceiveEndpointConfiguratorObserver(metrics));
            configurator.ConnectBusObserver(new AppMetricsBusObserver(metrics));
        }

        static string GetServiceName(string serviceName)
        {
            return string.IsNullOrWhiteSpace(serviceName)
                ? Path.GetFileNameWithoutExtension(Process.GetCurrentProcess()?.MainModule?.FileName)
                : serviceName;
        }
    }
}