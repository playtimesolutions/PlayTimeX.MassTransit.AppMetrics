using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text;
using App.Metrics;
using MassTransit.Metadata;

namespace PlayTimeX.MassTransit.AppMetrics
{
    /// <summary>
    /// 
    /// </summary>
    public class MassTransitMetricsTagProvider
    {
        private static readonly ConcurrentDictionary<string, string> LabelCache = new ConcurrentDictionary<string, string>();

        private static readonly char[] Delimiters = { '<', '>' };

        private static string _serviceName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="options"></param>
        public MassTransitMetricsTagProvider(string serviceName, MassTransitMetricsOptions options)
        {
            _serviceName = serviceName;

            ServiceLabel = options.Labels.ServiceName;

            EndpointLabels = new[] { options.Labels.ServiceName, options.Labels.Endpoint };

            EndpointFaultLabels = new[] { options.Labels.ServiceName, options.Labels.Endpoint, options.Labels.ExceptionType };

            MessageLabels = new[] { options.Labels.ServiceName, options.Labels.MessageType };

            MessageFaultLabels = new[] { options.Labels.ServiceName, options.Labels.MessageType, options.Labels.ExceptionType };

            ExecuteLabels = new[] { options.Labels.ServiceName, options.Labels.ActivityName, options.Labels.ArgumentType };

            ExecuteFaultLabels = new[] { options.Labels.ServiceName, options.Labels.ActivityName, options.Labels.ArgumentType, options.Labels.ExceptionType };

            CompensateLabels = new[] { options.Labels.ServiceName, options.Labels.ActivityName, options.Labels.LogType };

            CompensateFailureLabels = new[] { options.Labels.ServiceName, options.Labels.ActivityName, options.Labels.LogType, options.Labels.ExceptionType };

            ConsumerLabels = new[] { options.Labels.ServiceName, options.Labels.MessageType, options.Labels.ConsumerType };

            ConsumerFaultLabels = new[] { options.Labels.ServiceName, options.Labels.MessageType, options.Labels.ConsumerType, options.Labels.ExceptionType };
        }

        private string ServiceLabel { get; set; }

        private string[] EndpointLabels { get; set; }

        private string[] EndpointFaultLabels { get; set; }

        private string[] MessageLabels { get; set; }

        private string[] MessageFaultLabels { get; set; }

        private string[] ExecuteLabels { get; set; }

        private string[] ExecuteFaultLabels { get; set; }

        private string[] CompensateLabels { get; set; }

        private string[] CompensateFailureLabels { get; set; }

        private string[] ConsumerLabels { get; set; }

        private string[] ConsumerFaultLabels { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MetricTags ServiceTags()
        {
            return new MetricTags(ServiceLabel, _serviceName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointLabel"></param>
        /// <returns></returns>
        public MetricTags EndpointTags(string endpointLabel)
        {
            return new MetricTags(EndpointLabels, new[] { _serviceName, endpointLabel });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointLabel"></param>
        /// <param name="exceptionType"></param>
        /// <returns></returns>
        public MetricTags EndpointFaultTags(string endpointLabel, string exceptionType)
        {
            return new MetricTags(EndpointFaultLabels, new[] { _serviceName, endpointLabel, exceptionType });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="cleanConsumerType"></param>
        /// <returns></returns>
        public MetricTags ConsumerTags(string messageType, string cleanConsumerType)
        {
            return new MetricTags(ConsumerLabels, new[] { _serviceName, messageType, cleanConsumerType });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="cleanConsumerType"></param>
        /// <param name="exceptionType"></param>
        /// <returns></returns>
        public MetricTags ConsumerFaultTags( string messageType, string cleanConsumerType, string exceptionType)
        {
            return new MetricTags(ConsumerFaultLabels, new[] { _serviceName, messageType, cleanConsumerType, exceptionType });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityName"></param>
        /// <param name="argumentType"></param>
        /// <returns></returns>
        public MetricTags ExecuteTags(string activityName, string argumentType)
        {
            return new MetricTags(ExecuteLabels, new[] { _serviceName, activityName, argumentType });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityName"></param>
        /// <param name="argumentType"></param>
        /// <param name="exceptionType"></param>
        /// <returns></returns>
        public MetricTags ExecuteFaultTags(string activityName, string argumentType, string exceptionType)
        {
            return new MetricTags(ExecuteFaultLabels, new[] { _serviceName, activityName, argumentType, exceptionType });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityName"></param>
        /// <param name="logType"></param>
        /// <returns></returns>
        public MetricTags CompensateTags(string activityName, string logType)
        {
            return new MetricTags(CompensateLabels, new[] { _serviceName, activityName, logType });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityName"></param>
        /// <param name="logType"></param>
        /// <param name="exceptionType"></param>
        /// <returns></returns>
        public MetricTags CompensateFailureTags(string activityName, string logType, string exceptionType)
        {
            return new MetricTags(CompensateFailureLabels, new[] { _serviceName, activityName, logType, exceptionType });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public MetricTags MessageTags(string messageType)
        {
            return new MetricTags(MessageLabels, new[] { _serviceName, messageType });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="exceptionType"></param>
        /// <returns></returns>
        public MetricTags MessageFaultTags(string messageType, string exceptionType)
        {
            return new MetricTags(MessageFaultLabels, new[] { _serviceName, messageType, exceptionType });
        }

        internal static string GetConsumerTypeLabel(string consumerType, string messageType, string messageLabel)
        {
            return LabelCache.GetOrAdd(consumerType, type =>
            {
                if (type.StartsWith("MassTransit.MessageHandler<"))
                    return "Handler";

                var genericMessageType = "<" + messageType + ">";
                if (type.IndexOf(genericMessageType, StringComparison.Ordinal) >= 0)
                    type = type.Replace(genericMessageType, "_" + messageLabel);

                return CleanupLabel(type);
            });
        }

        internal static string CleanupLabel(string label)
        {
            static string SimpleClean(string text)
            {
                return text.Split('.', '+').Last();
            }

            var indexOf = label.IndexOfAny(Delimiters);
            if (indexOf >= 0)
            {
                if (label[indexOf] == '<')
                    return SimpleClean(label.Substring(0, indexOf)) + "_" + CleanupLabel(label.Substring(indexOf + 1));

                if (label[indexOf] == '>')
                    return SimpleClean(label.Substring(0, indexOf)) + CleanupLabel(label.Substring(indexOf + 1));

                return SimpleClean(label);
            }

            return SimpleClean(label);
        }

        internal static string GetArgumentTypeLabel<TArguments>()
        {
            return LabelCache.GetOrAdd(TypeMetadataCache<TArguments>.ShortName, type => FormatTypeName(new StringBuilder(), typeof(TArguments))
                .Replace("Arguments", ""));
        }

        internal static string GetLogTypeLabel<TLog>()
        {
            return LabelCache.GetOrAdd(TypeMetadataCache<TLog>.ShortName, type => FormatTypeName(new StringBuilder(), typeof(TLog)).Replace("Log", ""));
        }

        internal static string GetEndpointLabel(Uri inputAddress)
        {
            return inputAddress?.AbsolutePath.Split('/').LastOrDefault()?.Replace(".", "_").Replace("/", "_");
        }

        internal static string GetMessageTypeLabel<TMessage>()
        {
            return LabelCache.GetOrAdd(TypeMetadataCache<TMessage>.ShortName, type => FormatTypeName(new StringBuilder(), typeof(TMessage)));
        }

        internal static string FormatTypeName(StringBuilder sb, Type type)
        {
            if (type.IsGenericParameter)
                return "";

            if (type.GetTypeInfo().IsGenericType)
            {
                var name = type.GetGenericTypeDefinition().Name;

                //remove `1
                var index = name.IndexOf('`');
                if (index > 0)
                    name = name.Remove(index);

                sb.Append(name);
                sb.Append('_');
                var arguments = type.GetTypeInfo().GenericTypeArguments;
                for (var i = 0; i < arguments.Length; i++)
                {
                    if (i > 0)
                        sb.Append('_');

                    FormatTypeName(sb, arguments[i]);
                }
            }
            else
                sb.Append(type.Name);

            return sb.ToString();
        }
    }
}