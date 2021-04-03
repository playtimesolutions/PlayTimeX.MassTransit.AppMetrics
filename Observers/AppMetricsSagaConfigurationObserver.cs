using App.Metrics;
using Automatonymous;
using MassTransit;
using MassTransit.Saga;
using MassTransit.SagaConfigurators;
using PlayTimeX.MassTransit.AppMetrics.Configuration;

namespace PlayTimeX.MassTransit.AppMetrics.Observers
{
    /// <summary>
    /// 
    /// </summary>
    public class AppMetricsSagaConfigurationObserver :
        ISagaConfigurationObserver
    {
        private readonly IMetrics _metrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public AppMetricsSagaConfigurationObserver(IMetrics metrics)
        {
            _metrics = metrics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configurator"></param>
        public void SagaConfigured<T>(ISagaConfigurator<T> configurator)
            where T : class, ISaga
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        /// <param name="configurator"></param>
        /// <param name="stateMachine"></param>
        public void StateMachineSagaConfigured<TInstance>(ISagaConfigurator<TInstance> configurator, SagaStateMachine<TInstance> stateMachine)
            where TInstance : class, ISaga, SagaStateMachineInstance
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="configurator"></param>
        public void SagaMessageConfigured<T, TMessage>(ISagaMessageConfigurator<T, TMessage> configurator)
            where T : class, ISaga
            where TMessage : class
        {
            var specification = new AppMetricsSagaSpecification<T, TMessage>(_metrics);

            configurator.AddPipeSpecification(specification);
        }
    }
}