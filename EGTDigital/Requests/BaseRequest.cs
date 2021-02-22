namespace EGTDigital.Requests
{
    public class BaseRequest
    {
        public string RequestId { get; set; }

        public long Timestamp { get; set; }

        public string Client { get; set; }

        public string Currency { get; set; }

        public string ServiceName { get; set; }
    }
}
