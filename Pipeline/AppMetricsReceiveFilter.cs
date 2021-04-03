using System.Threading.Tasks;
using App.Metrics;
using GreenPipes;
using MassTransit;

namespace PlayTimeX.MassTransit.AppMetrics.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    public class AppMetricsReceiveFilter : AppMetricsProbeSite,
        IFilter<ReceiveContext>
    {
        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsReceiveFilter(IMetrics metrics)
        {
            _metrics = metrics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task Send(ReceiveContext context, IPipe<ReceiveContext> next)
        {
            using var inProgress = _metrics.TrackReceiveInProgress(context);

            await next.Send(context).ConfigureAwait(false);
        }
    }
}