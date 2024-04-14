namespace Core.Models
{
    public class ProductInvoice
    {
        public int ProductInvoiceId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string InvoiceNumber { get; set; }
        public bool Deleted { get; set; }
    }
}