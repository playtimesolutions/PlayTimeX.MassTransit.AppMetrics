using System.Threading.Tasks;
using App.Metrics;
using GreenPipes;
using MassTransit;

namespace PlayTimeX.MassTransit.AppMetrics.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class AppMetricsHandlerFilter<TMessage> : AppMetricsProbeSite,
        IFilter<ConsumeContext<TMessage>>
        where TMessage : class
    {
        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsHandlerFilter(IMetrics metrics)
        {
            _metrics = metrics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task Send(ConsumeContext<TMessage> context, IPipe<ConsumeContext<TMessage>> next)
        {
            using var inProgress = _metrics.TrackHandlerInProgress<TMessage>();

            await next.Send(context).ConfigureAwait(false);
        }
    }
}