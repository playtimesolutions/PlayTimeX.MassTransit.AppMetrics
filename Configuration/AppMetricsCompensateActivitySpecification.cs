using System.Collections.Generic;
using System.Linq;
using App.Metrics;
using GreenPipes;
using MassTransit.Courier;
using PlayTimeX.MassTransit.AppMetrics.Pipeline;

namespace PlayTimeX.MassTransit.AppMetrics.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TActivity"></typeparam>
    /// <typeparam name="TLog"></typeparam>
    public class AppMetricsCompensateActivitySpecification<TActivity, TLog> :
        IPipeSpecification<CompensateActivityContext<TActivity, TLog>>
        where TActivity : class, ICompensateActivity<TLog>
        where TLog : class
    {
        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsCompensateActivitySpecification(IMetrics metrics)
        {
            _metrics = metrics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Apply(IPipeBuilder<CompensateActivityContext<TActivity, TLog>> builder)
        {
            builder.AddFilter(new AppMetricsCompensateActivityFilter<TActivity, TLog>(_metrics));
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
