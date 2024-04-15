using Core.Models;

namespace UI.Models
{
    public class CreateProductViewModel
    {
        public string ProductName { get; set; }
        public Decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public List<Category> Categories { get; set; }
    }
}