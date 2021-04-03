using System.Collections.Generic;
using System.Linq;
using App.Metrics;
using GreenPipes;
using MassTransit;
using MassTransit.Saga;
using PlayTimeX.MassTransit.AppMetrics.Pipeline;

namespace PlayTimeX.MassTransit.AppMetrics.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSaga"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    public class AppMetricsSagaSpecification<TSaga, TMessage> :
        IPipeSpecification<SagaConsumeContext<TSaga, TMessage>>
        where TSaga : class, ISaga
        where TMessage : class
    {
        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsSagaSpecification(IMetrics metrics)
        {
            _metrics = metrics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Apply(IPipeBuilder<SagaConsumeContext<TSaga, TMessage>> builder)
        {
            builder.AddFilter(new AppMetricsSagaFilter<TSaga, TMessage>(_metrics));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}