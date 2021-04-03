using System;
using App.Metrics;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Internals.Extensions;
using PlayTimeX.MassTransit.AppMetrics.Configuration;

namespace PlayTimeX.MassTransit.AppMetrics.Observers
{
    /// <summary>
    /// 
    /// </summary>
    public class AppMetricsConsumerConfigurationObserver :
        IConsumerConfigurationObserver
    {
        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsConsumerConfigurationObserver(IMetrics metrics)
        {
            _metrics = metrics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TConsumer"></typeparam>
        /// <param name="configurator"></param>
        public void ConsumerConfigured<TConsumer>(IConsumerConfigurator<TConsumer> configurator)
            where TConsumer : class
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TConsumer"></typeparam>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="configurator"></param>
        public void ConsumerMessageConfigured<TConsumer, TMessage>(IConsumerMessageConfigurator<TConsumer, TMessage> configurator)
            where TConsumer : class
            where TMessage : class
        {
            if (typeof(TMessage).ClosesType(typeof(Batch<>), out Type[] types))
            {
                typeof(AppMetricsConsumerConfigurationObserver)
                    .GetMethod(nameof(BatchConsumerConfigured))
                    .MakeGenericMethod(typeof(TConsumer), types[0])
                    .Invoke(this, new object[] { configurator });
            }
            else
            {
                var specification = new AppMetricsConsumerSpecification<TConsumer, TMessage>(_metrics);

                configurator.AddPipeSpecification(specification);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TConsumer"></typeparam>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="configurator"></param>
        public void BatchConsumerConfigured<TConsumer, TMessage>(IConsumerMessageConfigurator<TConsumer, Batch<TMessage>> configurator)
            where TConsumer : class, IConsumer<Batch<TMessage>>
            where TMessage : class
        {
            var specification = new AppMetricsConsumerSpecification<TConsumer, Batch<TMessage>>(_metrics);

            configurator.AddPipeSpecification(specification);
        }
    }
}