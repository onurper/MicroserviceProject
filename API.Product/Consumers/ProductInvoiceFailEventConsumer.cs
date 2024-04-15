using Core.Services;
using MassTransit;
using SharedLibrary.Events;

namespace API.Product.Consumers
{
    public class ProductInvoiceFailEventConsumer : IConsumer<ProductInvoiceNotWorkingEvent>
    {
        private readonly IProductService _productService;

        private readonly ILogger<ProductInvoiceFailEventConsumer> _logger;

        public ProductInvoiceFailEventConsumer(ILogger<ProductInvoiceFailEventConsumer> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task Consume(ConsumeContext<ProductInvoiceNotWorkingEvent> context)
        {
            var resultProduct = await _productService.Where(x => x.ProductId == context.Message.ProductId);

            if (resultProduct.IsSuccessful && resultProduct.Data.Any())
            {
                Core.Models.Product product = resultProduct.Data.Single();

                product.Deleted = true;

                await _productService.Update(product, product.ProductId);

                _logger.LogInformation($"Oluşan hata sebebiyle ürün silindi. Ürün : {product.ProductId}");
            }
            else
            {
                _logger.LogInformation($"Ürün bulunamadı. Ürün : {context.Message.ProductId}");
            }
        }
    }
}