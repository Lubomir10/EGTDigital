namespace EGTDigital.Entities
{
    public class Request : Entity
    {
        public string RequestId { get; set; }

        public long Timestamp { get; set; }

        public string Client { get; set; }

        public string Currency { get; set; }

        public string ServiceName { get; set; }

        public override string ToString()
        {
            return $"Request id: {RequestId} Timestamp: {Timestamp} Client: {Client} Currency: {Currency} Service Name: {ServiceName}";
        }
    }
}
