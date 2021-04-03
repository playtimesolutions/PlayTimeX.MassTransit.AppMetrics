using System.Threading.Tasks;
using App.Metrics;
using GreenPipes;
using MassTransit.Courier;

namespace PlayTimeX.MassTransit.AppMetrics.Pipeline
{
    /// <summary>
    /// /
    /// </summary>
    /// <typeparam name="TActivity"></typeparam>
    /// <typeparam name="TLog"></typeparam>
    public class AppMetricsCompensateActivityFilter<TActivity, TLog> : AppMetricsProbeSite,
        IFilter<CompensateActivityContext<TActivity, TLog>>
        where TActivity : class, ICompensateActivity<TLog>
        where TLog : class
    {
        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsCompensateActivityFilter(IMetrics metrics)
        {
            _metrics = metrics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task Send(CompensateActivityContext<TActivity, TLog> context, IPipe<CompensateActivityContext<TActivity, TLog>> next)
        {
            using var inProgress = _metrics.TrackCompensateActivityInProgress(context);

            await next.Send(context).ConfigureAwait(false);
        }
    }
}