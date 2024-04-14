using API.ProductInvoice.Consumers;
using Core.Repositories;
using Core.Services;
using Core.UnitOfWork;
using Data;
using Data.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Service.Services;
using SharedLibrary.Configurations;
using SharedLibrary.Events;
using SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product.Invoice.API", Version = "v1" });
});
builder.Services.AddHttpClient();
builder.Services.AddScoped<IProductInvoiceRepository<AppProductInvoiceContext>, ProductInvoiceRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IProductInvoiceService, ProductInvoiceService>();
builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped(typeof(IServiceGeneric<,>), typeof(ServiceGeneric<,>));
builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

builder.Services.AddDbContext<AppProductInvoiceContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppProductInvoiceDbCon"), sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("Data");
    });
});

builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));
builder.Services.AddCustomTokenAuth(builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>());

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProductInvoiceCreatedEventConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ReceiveEndpoint(RabbitMQSettings.ProductInvoiceCreatedEventQueueName, e =>
        {
            e.ConfigureConsumer<ProductInvoiceCreatedEventConsumer>(context);
        });
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.Run();