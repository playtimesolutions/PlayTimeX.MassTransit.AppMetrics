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
    public class AppMetricsReceiveSpecification :
        IPipeSpecification<ReceiveContext>
    {

        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsReceiveSpecification(IMetrics metrics)
        {
            _metrics = metrics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Apply(IPipeBuilder<ReceiveContext> builder)
        {
            builder.AddFilter(new AppMetricsReceiveFilter(_metrics));
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