using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Counter;
using App.Metrics.Gauge;

namespace PlayTimeX.MassTransit.AppMetrics.Internal
{
    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        private sealed class InProgressTracker : IDisposable
        {
            public InProgressTracker(ICounter counter)
            {
                _counter = counter;
            }

            public void Dispose()
            {
                _counter.Decrement();
            }
            
            private readonly ICounter _counter;
        }

        /// <summary>
        /// Tracks the number of in-progress operations taking place.
        /// 
        /// Calling this increments the gauge. Disposing of the returned instance decrements it again.
        /// </summary>
        /// <remarks>
        /// It is safe to track the sum of multiple concurrent in-progress operations with the same gauge.
        /// </remarks>
        public static IDisposable TrackInProgress(this ICounter counter)
        {
            if (counter == null)
                throw new ArgumentNullException(nameof(counter));

            counter.Increment();

            return new InProgressTracker(counter);
        }
    }
}