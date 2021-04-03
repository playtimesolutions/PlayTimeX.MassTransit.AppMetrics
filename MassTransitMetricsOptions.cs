using PlayTimeX.MassTransit.AppMetrics.Configuration;

namespace PlayTimeX.MassTransit.AppMetrics
{
    /// <summary>
    /// 
    /// </summary>
    public class MassTransitMetricsOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public MassTransitMetricLabelConfiguration Labels { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool MonitorPublishEvents { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public bool MonitorConsumeEvents { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public bool MonitorSendEvents { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public bool MonitorReceiveEvents { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public bool MonitorRetryEvents { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public bool MonitorBusEvents { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public  bool MonitorRoutingSlips { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public bool MonitorSagas { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MassTransitMetricsOptions CreateDefault()
        {
            return new MassTransitMetricsOptions
            {
                Labels = new MassTransitMetricLabelConfiguration()
            };
        }
    }
}