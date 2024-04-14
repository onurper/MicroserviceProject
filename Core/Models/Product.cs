using System;

namespace Core.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public Decimal Price { get; set; }
        public int Stock { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public bool Deleted { get; set; }
    }
}