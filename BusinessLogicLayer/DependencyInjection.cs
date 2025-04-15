using BusinessLogicLayer.Mappers;
using BusinessLogicLayer.RabbitMQ;
using BusinessLogicLayer.ServiceContracts;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;


namespace BusinessLogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProductAddRequestToProductMappingProfile).Assembly);
            services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();
            return services;
        }
    }
}
