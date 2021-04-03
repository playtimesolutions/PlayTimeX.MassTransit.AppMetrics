using GreenPipes;

namespace PlayTimeX.MassTransit.AppMetrics.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AppMetricsProbeSite : IProbeSite
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("mt-AppMetrics");
        }
    }
}