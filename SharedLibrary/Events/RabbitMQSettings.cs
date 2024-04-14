namespace SharedLibrary.Events
{
    public static class RabbitMQSettings
    {
        public const string ProductInvoiceCreatedEventQueueName = "product-created-queue";
        public const string ProductInvoiceWorkingEventQueueName = "product-invoice-working-queue";
        public const string ProductInvoiceNotWorkingEventQueueName = "product-not-invoice-working-queue";
    }
}
