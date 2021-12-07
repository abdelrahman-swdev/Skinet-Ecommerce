using System.Linq;
using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
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