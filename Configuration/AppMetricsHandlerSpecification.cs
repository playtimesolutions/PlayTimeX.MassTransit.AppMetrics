using System.Collections.Generic;
using System.Linq;
using App.Metrics;
using GreenPipes;
using MassTransit;
using PlayTimeX.MassTransit.AppMetrics.Pipeline;

namespace PlayTimeX.MassTransit.AppMetrics.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class AppMetricsHandlerSpecification<TMessage> :
        IPipeSpecification<ConsumeContext<TMessage>>
        where TMessage : class
    {
        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsHandlerSpecification(IMetrics metrics)
        {
            _metrics = metrics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Apply(IPipeBuilder<ConsumeContext<TMessage>> builder)
        {
            builder.AddFilter(new AppMetricsHandlerFilter<TMessage>(_metrics));
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