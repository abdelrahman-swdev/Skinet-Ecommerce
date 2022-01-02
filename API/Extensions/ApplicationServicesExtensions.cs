using System.Linq;
using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // add Product Repository service to container
            services.AddScoped<IProductRepository, ProductRepository>();
            
            // add basket repository
            services.AddScoped<IBasketRepository, BasketRepository>();

            // add Generic Repository service to container
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // add token service to container
            services.AddScoped<ITokenService, TokenService>();

            // add order service to container
            services.AddScoped<IOrderService, OrderService>();

            // add payment service to container
            services.AddScoped<IPaymentService, PaymentService>();

            // add caching service to container
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();

            // add unit of work service to container with the same lifetime of repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // override the api controller atribute behavior
            services.Configure<ApiBehaviorOptions>(op => 
            {
                op.InvalidModelStateResponseFactory = actionResult => 
                {
                    var errors = actionResult.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(e => e.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();
                    var errorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });

            return services;
        }
    }
}