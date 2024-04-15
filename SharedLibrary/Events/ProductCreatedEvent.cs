namespace SharedLibrary.Events
{
    public class ProductCreatedEvent
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string InvoiceNumber { get; set; }
        public ProductInvoiceMessage ProductInvoiceMessage { get; set; }
    }
}