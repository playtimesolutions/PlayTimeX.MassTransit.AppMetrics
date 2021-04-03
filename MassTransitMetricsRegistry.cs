using System;
using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Metadata;
using MassTransit.Saga;
using PlayTimeX.MassTransit.AppMetrics.Internal;
using MetricsFactory = PlayTimeX.MassTransit.AppMetrics.Internal.MetricsFactory;

namespace PlayTimeX.MassTransit.AppMetrics
{
    public static class ProcessMetrics
    {
        private static readonly string ContextName = "Process";

        public static GaugeOptions SystemNonPagedMemoryGauge = new GaugeOptions
        {
            Context = ContextName,
            Name = "System Non-Paged Memory",
            MeasurementUnit = Unit.Bytes
        };

        public static GaugeOptions ProcessVirtualMemorySizeGauge = new GaugeOptions
        {
            Context = ContextName,
            Name = "Process Virtual Memory Size",
            MeasurementUnit = Unit.Bytes
        };
    }


    /// <summary>
    /// 
    /// </summary>
    public static class MassTransitMetricsRegistry
    {
        private static bool _isConfigured;
        private static MassTransitMetricsTagProvider _metricsTagProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="options"></param>
        public static void TryConfigure(string serviceName, MassTransitMetricsOptions options)
        {
            if (_isConfigured)
                return;

            _metricsTagProvider = new MassTransitMetricsTagProvider(serviceName, options);

            //Meters 

            Meters.MessagesReceived = MetricsFactory.CreateMeter(
                options.Labels.MessagesReceived,
                "messages received", Unit.Custom("Message"));

            Meters.MessageFaultsReceived = MetricsFactory.CreateMeter(
                options.Labels.MessageFaultsReceived,
                "message faults received", Unit.Custom("Fault")); 

            Meters.MessagesConsumed = MetricsFactory.CreateMeter(
                options.Labels.MessagesConsumed,
                "messages consumed", Unit.Custom("Message"));

            Meters.MessageFaultsConsumed = MetricsFactory.CreateMeter(
                options.Labels.MessageFaultsConsumed,
                "message faults consumed", Unit.Custom("Fault"));

            Meters.ActivityExecuted = MetricsFactory.CreateMeter(
                options.Labels.ActivityExecuted,
                "activities executed", Unit.Custom("Activity"));

            Meters.ActivityFailure = MetricsFactory.CreateMeter(
                options.Labels.ActivityExecutionFailure,
                "activity execution failures", Unit.Errors);

            Meters.ActivityCompensated = MetricsFactory.CreateMeter(
                options.Labels.ActivityCompensated,
                "activities compensated", Unit.Custom("Rollback")); 

            Meters.ActivityCompensationFailure = MetricsFactory.CreateMeter(
                options.Labels.ActivityCompensationFailure,
                "activity compensation failures", Unit.Errors);

            Meters.MessagesPublished = MetricsFactory.CreateMeter(
                options.Labels.MessagesPublished,
                "messages published", Unit.Events);

            Meters.MessagePublishFailure = MetricsFactory.CreateMeter(
                options.Labels.MessagePublishFailure,
                "messages publish failures", Unit.Events);

            Meters.MessagesSent = MetricsFactory.CreateMeter(
                options.Labels.MessagesSent,
                "messages sent", Unit.Events); //messageLabels

            Meters.MessageSendFailure = MetricsFactory.CreateMeter(
                options.Labels.MessageSendFailure,
                "message send failures", Unit.Errors); //messageFaultLabels



            // Counters

            Counters.BusInstances = MetricsFactory.CreateCounter(
                options.Labels.BusInstances,
                "Number of bus instances", Unit.Items);

            Counters.EndpointInstances = MetricsFactory.CreateCounter(
                options.Labels.EndpointInstances,
                "Number of receive endpoint instances", Unit.Calls); 

            Counters.ReceiveInProgress = MetricsFactory.CreateCounter(
                options.Labels.ReceiveInProgress,
                "Number of messages being received", Unit.Events);

            Counters.HandlerInProgress = MetricsFactory.CreateCounter(
                options.Labels.HandlerInProgress,
                "Number of handlers in progress", Unit.Custom("Message"));

            Counters.ConsumerInProgress = MetricsFactory.CreateCounter(
                options.Labels.ConsumerInProgress,
                "Number of consumers in progress", Unit.Custom("Message"));

            Counters.SagaInProgress = MetricsFactory.CreateCounter(
                options.Labels.SagaInProgress,
                "Number of sagas in progress", Unit.Custom("Saga"));

            Counters.ExecuteInProgress = MetricsFactory.CreateCounter(
                options.Labels.ExecuteInProgress,
                "Number of activity executions in progress", Unit.Custom("Activity"));

            Counters.CompensateInProgress = MetricsFactory.CreateCounter(
                options.Labels.CompensateInProgress,
                "Number of activity compensations in progress", Unit.Custom("Activity"));


            Counters.ConsumeRetryTotal = MetricsFactory.CreateCounter(
                options.Labels.ConsumeRetryTotal,
                "Total number of message consume retries", Unit.Errors);


            // Histograms

            Histograms.ReceiveDuration = MetricsFactory.CreateHistogram(
                options.Labels.ReceiveDuration,
                "Elapsed time spent receiving a message", Unit.Custom("milliseconds"));

            Histograms.ConsumeDuration = MetricsFactory.CreateHistogram(
                options.Labels.ConsumeDuration,
                "Elapsed time spent consuming a message", Unit.Custom("milliseconds"));

            Histograms.DeliveryDuration = MetricsFactory.CreateHistogram(
                options.Labels.DeliveryDuration,
                "Elapsed time between when the message was sent and when it was consumed.", Unit.Custom("milliseconds"));

            Histograms.ActivityExecutionDuration = MetricsFactory.CreateHistogram(
                options.Labels.ActivityExecutionDuration,
                "Elapsed time spent executing an activity", Unit.Custom("milliseconds"));

            Histograms.ActivityCompensateDuration = MetricsFactory.CreateHistogram(
                options.Labels.ActivityCompensateDuration,
                "Elapsed time spent compensating an activity", Unit.Custom("milliseconds")); 

            _isConfigured = true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public static void BusStarted(this IMetrics metrics)
        {
            var tags = _metricsTagProvider.ServiceTags();
            metrics.Provider.Counter.Instance(Counters.BusInstances, tags).Increment();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        public static void BusStopped(this IMetrics metrics)
        {
            var tags = _metricsTagProvider.ServiceTags();
            metrics.Provider.Counter.Instance(Counters.BusInstances, tags).Decrement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        /// <param name="ready"></param>
        public static void EndpointReady(this IMetrics metrics, ReceiveEndpointReady ready)
        {
            var endpointLabel = MassTransitMetricsTagProvider.GetEndpointLabel(ready.InputAddress);
            var tags = _metricsTagProvider.EndpointTags(endpointLabel);
            metrics.Provider.Counter.Instance(Counters.EndpointInstances, tags).Increment();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        /// <param name="completed"></param>
        public static void EndpointCompleted(this IMetrics metrics, ReceiveEndpointCompleted completed)
        {
            var endpointLabel = MassTransitMetricsTagProvider.GetEndpointLabel(completed.InputAddress);
            var tags = _metricsTagProvider.EndpointTags( endpointLabel);

            metrics.Provider.Counter.Instance(Counters.EndpointInstances, tags).Decrement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        public static void MeasureReceived(this IMetrics metrics, ReceiveContext context, Exception exception = default)
        {
            var endpointLabel = MassTransitMetricsTagProvider.GetEndpointLabel(context.InputAddress);

            var tags = _metricsTagProvider.EndpointTags(endpointLabel);

            metrics.Provider.Meter.Instance(Meters.MessagesReceived, tags).Mark();
            metrics.Provider.Histogram.Instance(Histograms.ReceiveDuration, tags).Update(Convert.ToInt64(context.ElapsedTime.TotalMilliseconds));

            if (exception == null) return;

            var exTags = _metricsTagProvider.EndpointFaultTags(endpointLabel, exception.GetType().Name);

            metrics.Provider.Meter.Instance(Meters.MessageFaultsReceived, exTags).Mark();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metrics"></param>
        /// <param name="context"></param>
        /// <param name="duration"></param>
        /// <param name="consumerType"></param>
        /// <param name="exception"></param>
        public static void MeasureConsume<T>(this IMetrics metrics, ConsumeContext<T> context, TimeSpan duration, string consumerType, Exception exception = default)
            where T : class
        {
            var messageType = MassTransitMetricsTagProvider.GetMessageTypeLabel<T>();
            var cleanConsumerType = MassTransitMetricsTagProvider.GetConsumerTypeLabel(consumerType, TypeMetadataCache<T>.ShortName, messageType);

            var tags = _metricsTagProvider.ConsumerTags(messageType, cleanConsumerType);

            metrics.Provider.Meter.Instance(Meters.MessagesConsumed, tags).Mark();
            metrics.Provider.Histogram.Instance(Histograms.ConsumeDuration, tags).Update(Convert.ToInt64(duration.TotalMilliseconds));

            if (exception != null)
            {
                var exceptionType = exception.GetType().Name;
                var exTags = _metricsTagProvider.ConsumerFaultTags( messageType, cleanConsumerType, exceptionType);
                metrics.Provider.Meter.Instance(Meters.MessageFaultsConsumed, exTags).Mark();
            }

            var retryAttempt = context.GetRetryAttempt();
            if (retryAttempt > 0)
            {
                var reTags = _metricsTagProvider.ServiceTags();
                metrics.Provider.Counter.Instance(Counters.ConsumeRetryTotal, reTags).Increment();
            }

            if (!context.SentTime.HasValue)
                return;

            var deliveryDuration = DateTime.UtcNow - context.SentTime.Value;
            if (deliveryDuration < TimeSpan.Zero)
                deliveryDuration = TimeSpan.Zero;

            metrics.Provider.Histogram.Instance(Histograms.DeliveryDuration, tags).Update(Convert.ToInt64(deliveryDuration.TotalMilliseconds));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TActivity"></typeparam>
        /// <typeparam name="TArguments"></typeparam>
        /// <param name="metrics"></param>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        public static void MeasureExecute<TActivity, TArguments>(this IMetrics metrics, ExecuteActivityContext<TActivity, TArguments> context, Exception exception = default)
            where TActivity : class, IExecuteActivity<TArguments>
            where TArguments : class
        {
            var argumentType = MassTransitMetricsTagProvider.GetArgumentTypeLabel<TArguments>();
            var tags = _metricsTagProvider.ExecuteTags(context.ActivityName, argumentType);

            metrics.Provider.Meter.Instance(Meters.ActivityExecuted, tags).Mark();
            metrics.Provider.Histogram.Instance(Histograms.ActivityExecutionDuration, tags).Update(Convert.ToInt64(context.Elapsed.TotalMilliseconds));

            if (exception == null) return;
            
            var exTags = _metricsTagProvider.ExecuteFaultTags(context.ActivityName, argumentType, exception.GetType().Name);
            metrics.Provider.Meter.Instance(Meters.ActivityFailure, exTags).Mark();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TActivity"></typeparam>
        /// <typeparam name="TLog"></typeparam>
        /// <param name="metrics"></param>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        public static void MeasureCompensate<TActivity, TLog>(this IMetrics metrics, CompensateActivityContext<TActivity, TLog> context, Exception exception = default)
            where TActivity : class, ICompensateActivity<TLog>
            where TLog : class
        {
            var logType = MassTransitMetricsTagProvider.GetLogTypeLabel<TLog>();
            var tags = _metricsTagProvider.CompensateTags(context.ActivityName, logType);
            metrics.Provider.Meter.Instance(Meters.ActivityCompensated, tags).Mark();
            metrics.Provider.Histogram.Instance(Histograms.ActivityCompensateDuration, tags).Update(Convert.ToInt64(context.Elapsed.TotalSeconds));

            if (exception == null) return;
            
            var exTags = _metricsTagProvider.CompensateFailureTags(context.ActivityName, logType, exception.GetType().Name);
            metrics.Provider.Meter.Instance(Meters.ActivityCompensationFailure, exTags).Mark();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metrics"></param>
        /// <param name="exception"></param>
        public static void MeasurePublish<T>(this IMetrics metrics, Exception exception = default)
            where T : class
        {
            var messageType = MassTransitMetricsTagProvider.GetMessageTypeLabel<T>();
            var tags = _metricsTagProvider.MessageTags(messageType);
            metrics.Provider.Meter.Instance(Meters.MessagesPublished, tags).Mark();

            if (exception == null) return;
            
            var exTags = _metricsTagProvider.MessageFaultTags(messageType, exception.GetType().Name);
            metrics.Provider.Meter.Instance(Meters.MessagePublishFailure, exTags).Mark();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metrics"></param>
        /// <param name="exception"></param>
        public static void MeasureSend<T>(this IMetrics metrics, Exception exception = default)
            where T : class
        {
            var messageType = MassTransitMetricsTagProvider.GetMessageTypeLabel<T>();
            var tags = _metricsTagProvider.MessageTags(messageType);
            metrics.Provider.Meter.Instance(Meters.MessagesSent, tags).Mark();

            if (exception == null) return;
            
            var exTags = _metricsTagProvider.MessageFaultTags(messageType, exception.GetType().Name);
            metrics.Provider.Meter.Instance(Meters.MessagesSent, exTags).Mark();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDisposable TrackReceiveInProgress(this IMetrics metrics, ReceiveContext context)
        {
            var endpointLabel = MassTransitMetricsTagProvider.GetEndpointLabel(context.InputAddress);
            var tags = _metricsTagProvider.EndpointTags(endpointLabel);

            return metrics.Provider.Counter.Instance(Counters.ReceiveInProgress, tags).TrackInProgress();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TConsumer"></typeparam>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="metrics"></param>
        /// <returns></returns>
        public static IDisposable TrackConsumerInProgress<TConsumer, TMessage>(this IMetrics metrics)
            where TConsumer : class
            where TMessage : class
        {
            var messageType = MassTransitMetricsTagProvider.GetMessageTypeLabel<TMessage>();
            var cleanConsumerType = MassTransitMetricsTagProvider.GetConsumerTypeLabel(TypeMetadataCache<TConsumer>.ShortName, TypeMetadataCache<TMessage>.ShortName, messageType);
            var tags = _metricsTagProvider.ConsumerTags(messageType, cleanConsumerType);

            return metrics.Provider.Counter.Instance(Counters.ConsumerInProgress, tags).TrackInProgress();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSaga"></typeparam>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="metrics"></param>
        /// <returns></returns>
        public static IDisposable TrackSagaInProgress<TSaga, TMessage>(this IMetrics metrics)
            where TSaga : class, ISaga
            where TMessage : class
        {
            var messageType = MassTransitMetricsTagProvider.GetMessageTypeLabel<TMessage>();
            var cleanConsumerType = MassTransitMetricsTagProvider.GetConsumerTypeLabel(TypeMetadataCache<TSaga>.ShortName, TypeMetadataCache<TMessage>.ShortName, messageType);
            var tags = _metricsTagProvider.ConsumerTags(messageType, cleanConsumerType);

            return metrics.Provider.Counter.Instance(Counters.SagaInProgress, tags).TrackInProgress();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TActivity"></typeparam>
        /// <typeparam name="TArguments"></typeparam>
        /// <param name="metrics"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDisposable TrackExecuteActivityInProgress<TActivity, TArguments>(this IMetrics metrics, ExecuteActivityContext<TActivity, TArguments> context)
            where TActivity : class, IExecuteActivity<TArguments>
            where TArguments : class
        {
            var argumentType = MassTransitMetricsTagProvider.GetArgumentTypeLabel<TArguments>();
            var tags = _metricsTagProvider.ExecuteTags(context.ActivityName, argumentType);
            return metrics.Provider.Counter.Instance(Counters.ExecuteInProgress, tags).TrackInProgress();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TActivity"></typeparam>
        /// <typeparam name="TLog"></typeparam>
        /// <param name="metrics"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDisposable TrackCompensateActivityInProgress<TActivity, TLog>(this IMetrics metrics, CompensateActivityContext<TActivity, TLog> context)
            where TActivity : class, ICompensateActivity<TLog>
            where TLog : class
        {
            var argumentType = MassTransitMetricsTagProvider.GetArgumentTypeLabel<TLog>();
            var tags = _metricsTagProvider.CompensateTags(context.ActivityName, argumentType);
            return metrics.Provider.Counter.Instance(Counters.CompensateInProgress, tags).TrackInProgress();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="metrics"></param>
        /// <returns></returns>
        public static IDisposable TrackHandlerInProgress<TMessage>(this IMetrics metrics)
            where TMessage : class
        {
            var messageType = MassTransitMetricsTagProvider.GetMessageTypeLabel<TMessage>();
            var tags = _metricsTagProvider.MessageTags(messageType);
            return metrics.Provider.Counter.Instance(Counters.HandlerInProgress, tags).TrackInProgress();
        }

        /// <summary>
        /// 
        /// </summary>
        private static class Meters
        {
            public static MeterOptions MessagesReceived { get; set; }

            public static MeterOptions MessageFaultsReceived { get; set; }

            public static MeterOptions MessagesConsumed { get; set; }

            public static MeterOptions MessageFaultsConsumed { get; set; }

            public static MeterOptions MessagesPublished { get; set; }

            public static MeterOptions MessagePublishFailure { get; set; }

            public static MeterOptions MessagesSent { get; set; }

            public static MeterOptions MessageSendFailure { get; set; }

            public static MeterOptions ActivityExecuted { get; set; }

            public static MeterOptions ActivityFailure { get; set; }

            public static MeterOptions ActivityCompensated { get; set; }

            public static MeterOptions ActivityCompensationFailure { get; set; }

        }

        /// <summary>
        /// 
        /// </summary>
        private static class Histograms
        {
            public static HistogramOptions ReceiveDuration { get; set; }

            public static HistogramOptions ConsumeDuration { get; set; }

            public static HistogramOptions DeliveryDuration { get; set; }

            public static HistogramOptions ActivityExecutionDuration { get; set; }

            public static HistogramOptions ActivityCompensateDuration { get; set; }

        }

        /// <summary>
        /// 
        /// </summary>
        private static class Counters
        {
            public static CounterOptions BusInstances { get; set; }

            public static CounterOptions EndpointInstances { get; set; }

            public static CounterOptions ReceiveInProgress { get; set; }

            public static CounterOptions HandlerInProgress { get; set; }

            public static CounterOptions ConsumerInProgress { get; set; }

            public static CounterOptions SagaInProgress { get; set; }

            public static CounterOptions ExecuteInProgress { get; set; }

            public static CounterOptions CompensateInProgress { get; set; }

            public static CounterOptions ConsumeRetryTotal { get; set; }

            public static CounterOptions PublishTotal { get; set; }


            

        }
        /// <summary>
        /// 
        /// </summary>
        private static class Timers
        {
        }
    }
}