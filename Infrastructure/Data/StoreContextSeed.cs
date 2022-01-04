using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if(!context.ProductBrands.Any())
                {
                    var brandsData = 
                        File.ReadAllText(path + @"/Data/SeedData/brands.json");
                    
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    context.ProductBrands.AddRange(brands);
                    await context.SaveChangesAsync();
                }

                if(!context.ProductTypes.Any())
                {
                    var typesData = 
                        File.ReadAllText(path + @"/Data/SeedData/types.json");
                    
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    context.ProductTypes.AddRange(types);
                    await context.SaveChangesAsync();
                }

                if(!context.Products.Any())
                {
                    var productsData = 
                        File.ReadAllText(path + @"/Data/SeedData/products.json");
                    
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    context.Products.AddRange(products);
                    await context.SaveChangesAsync();
                }

                if(!context.DeliveryMethods.Any())
                {
                    var dmData = 
                        File.ReadAllText(path + @"/Data/SeedData/delivery.json");
                    
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);
                    context.DeliveryMethods.AddRange(deliveryMethods);
                    await context.SaveChangesAsync();
                }
            }
            catch(Exception e)
            {
                var logger = loggerFactory.CreateLogger<StoreContext>();
                logger.LogError(e.Message);
            }
        }
    }
}