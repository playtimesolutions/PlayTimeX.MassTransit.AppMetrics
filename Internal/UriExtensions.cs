using System;

namespace PlayTimeX.MassTransit.AppMetrics.Internal
{
    /// <summary>
    /// 
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetExchangeName(this Uri value)
        {
            var exchange = value.LocalPath;
            var messageType = exchange.Substring(exchange.LastIndexOf('/') + 1);
            return messageType;
        }
    }
}