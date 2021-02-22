namespace EGTDigital.RabbitMq
{
    public static class MessageSenderService
    {
        private static string rabbitMqExchange;

        private static MessageSender messageSender;

        public static MessageSender GetMessageSender()
        {
            if(messageSender == null)
            {
                messageSender = new MessageSender(rabbitMqExchange);
            }
            return messageSender;
        }

        public static void SetRabbitMqExchange(string exchange)
        {
            rabbitMqExchange = exchange;
        }
    }
}
