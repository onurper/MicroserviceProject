using System;

namespace Core.DTOs
{
    public class ProductDto
    {
        public string Name { get; set; }
        public Decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
    }
}