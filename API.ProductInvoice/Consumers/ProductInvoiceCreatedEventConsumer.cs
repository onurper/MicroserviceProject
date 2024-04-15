using Core.Services;
using MassTransit;
using SharedLibrary.Events;

namespace API.ProductInvoice.Consumers
{
    public class ProductInvoiceCreatedEventConsumer : IConsumer<ProductCreatedEvent>
    {
        private readonly IProductInvoiceService productInvoiceService;
        private readonly ILogger<ProductInvoiceCreatedEventConsumer> logger;
        private readonly ISendEndpointProvider sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductInvoiceCreatedEventConsumer(ILogger<ProductInvoiceCreatedEventConsumer> logger, IProductInvoiceService productInvoiceService, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            this.logger = logger;
            this.productInvoiceService = productInvoiceService;
            this.sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
        {
            try
            {
                //throw new NotImplementedException();

                var result = await productInvoiceService.AddAsync(new Core.Models.ProductInvoice()
                {
                    Deleted = false,
                    ProductId = context.Message.ProductId,
                    UserId = context.Message.UserId,
                    InvoiceNumber = context.Message.InvoiceNumber
                });

                if (!result.IsSuccessful)
                {
                    await _publishEndpoint.Publish(new ProductInvoiceNotWorkingEvent()
                    {
                        ErrorMessage = "Ürün takibi ekleme aşamasında bir hata oluştu",
                        UserId = context.Message.UserId,
                        ProductId = context.Message.ProductId,
                    });

                    logger.LogInformation($"Ürün takibi tablosuna ekleme yapıldı. Kullanıcı : {context.Message.UserId}");
                }
                else
                {

                    var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.ProductInvoiceWorkingEventQueueName}"));

                    await sendEndpoint.Send(new ProductInvoiceWorkingEvent()
                    {
                        ProductInvoiceId = result.Data.ProductInvoiceId,
                        SuccessMessage = "Ürün takibi ekleme işlemi başarılı."
                    });

                    logger.LogInformation($"Ürün takibi tablosuna ekleme yapıldı. Kullanıcı : {context.Message.UserId}");
                }
            }
            catch (Exception)
            {
                await _publishEndpoint.Publish(new ProductInvoiceNotWorkingEvent()
                {
                    ErrorMessage = "Ürün takibi ekleme aşamasında bir hata oluştu",
                    UserId = context.Message.UserId,
                    ProductId = context.Message.ProductId,
                });

                logger.LogInformation($"Ürün takibi tablosuna ekleme yapıldı. Kullanıcı : {context.Message.UserId}");
            }

        }
    }
}
