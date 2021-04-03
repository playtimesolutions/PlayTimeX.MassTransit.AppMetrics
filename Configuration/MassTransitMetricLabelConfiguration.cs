namespace PlayTimeX.MassTransit.AppMetrics.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class MassTransitMetricLabelConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public MassTransitMetricLabelConfiguration()
        {
            Endpoint = "endpoint_address";
            ConsumerType = "consumer_type";
            ExceptionType = "exception_type";
            MessageType = "message_type";
            ActivityName = "activity_name";
            ArgumentType = "argument_type";
            LogType = "log_type";
            ServiceName = "service_name";
            MessagesReceived = "msg_received";
            MessageFaultsReceived = "msg_fault_received";
            ReceiveDuration = "receive_duration";
            ReceiveInProgress = "receive_in_progress";
            MessagesConsumed = "msg_consumed";
            MessageFaultsConsumed = "msg_fault_consumed";
            ConsumeRetryTotal = "consume_retry_total";
            ConsumeDuration = "consume_duration";
            DeliveryDuration = "delivery_duration";
            MessagesPublished = "msg_published";
            MessagePublishFailure = "msg_publish_failure";
            MessagesSent = "msg_sent";
            MessageSendFailure = "msg_send_failure";
            ActivityExecuted = "activity_executed";
            ActivityExecutionFailure = "activity_execute_failure";
            ActivityExecutionDuration = "activity_execute_duration";
            ActivityCompensated = "activity_compensated";
            ActivityCompensationFailure = "activity_compensation_failure";
            ActivityCompensateDuration = "activity_compensate_duration";
            BusInstances = "bus_count";
            EndpointInstances = "endpoint_count";
            ConsumerInProgress = "consumer_in_progress";
            HandlerInProgress = "handler_in_progress";
            SagaInProgress = "saga_in_progress";
            ExecuteInProgress = "activity_execute_in_progress";
            CompensateInProgress = "activity_compensate_in_progress";
        }

        /// <summary>
        /// 
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConsumerType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ExceptionType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MessageType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ActivityName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ArgumentType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LogType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MessagesReceived { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MessageFaultsReceived { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiveDuration { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiveInProgress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MessagesConsumed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MessageFaultsConsumed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ConsumeRetryTotal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MessagesPublished { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MessagePublishFailure { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MessagesSent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MessageSendFailure { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ActivityExecuted { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ActivityExecutionFailure { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ActivityExecutionDuration { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ActivityCompensated { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ActivityCompensationFailure { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ActivityCompensateDuration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BusInstances { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EndpointInstances { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ConsumerInProgress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string HandlerInProgress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SagaInProgress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ExecuteInProgress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CompensateInProgress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConsumeDuration { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DeliveryDuration { get; set; }
    }
}