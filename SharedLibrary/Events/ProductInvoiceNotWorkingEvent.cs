namespace SharedLibrary.Events
{
    public class ProductInvoiceNotWorkingEvent
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string ErrorMessage { get; set; }
    }
}