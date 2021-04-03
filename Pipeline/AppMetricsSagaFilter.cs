using System.Threading.Tasks;
using App.Metrics;
using GreenPipes;
using MassTransit;
using MassTransit.Saga;

namespace PlayTimeX.MassTransit.AppMetrics.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSaga"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    public class AppMetricsSagaFilter<TSaga, TMessage> : AppMetricsProbeSite, 
        IFilter<SagaConsumeContext<TSaga, TMessage>>
        where TSaga : class, ISaga
        where TMessage : class
    {
        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsSagaFilter(IMetrics metrics)
        {
            _metrics = metrics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task Send(SagaConsumeContext<TSaga, TMessage> context, IPipe<SagaConsumeContext<TSaga, TMessage>> next)
        {
            using var inProgress = _metrics.TrackSagaInProgress<TSaga, TMessage>();

            await next.Send(context).ConfigureAwait(false);
        }
    }
}