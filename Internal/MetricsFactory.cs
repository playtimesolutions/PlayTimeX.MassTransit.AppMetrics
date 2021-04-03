using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;

namespace PlayTimeX.MassTransit.AppMetrics.Internal
{
    /// <summary>
    /// 
    /// </summary>
    public class MetricsFactory
    {
        /// <summary>
        /// 
        /// </summary>
        internal const string ContextName = "messaging";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="help"></param>
        /// <param name="measure"></param>
        /// <returns></returns>
        public static CounterOptions CreateCounter(string name, string help, Unit measure)
        {
            return new CounterOptions
            {
                Context = ContextName,
                Name = name,
                MeasurementUnit = measure
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="help"></param>
        /// <param name="measure"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static MeterOptions CreateMeter(string name, string help, Unit measure, TimeUnit rate = TimeUnit.Minutes)
        {
            return new MeterOptions
            {
                Context = ContextName,
                Name = name,
                MeasurementUnit = measure,
                RateUnit = rate
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="help"></param>
        /// <param name="measure"></param>
        /// <returns></returns>
        public static HistogramOptions CreateHistogram(string name, string help, Unit measure)
        {
            return new HistogramOptions
            {
                Context = ContextName,
                Name = name,
                MeasurementUnit = measure
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="help"></param>
        /// <param name="measure"></param>
        /// <returns></returns>
        public static GaugeOptions CreateGauge(string name, string help, Unit measure)
        {
            return new GaugeOptions
            {
                Context = ContextName,
                Name = name,
                MeasurementUnit = measure
            };
        }
    }
}