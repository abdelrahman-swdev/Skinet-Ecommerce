using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
            
        }

        public async Task<ProductBrand> GetProductBrandByIdAsync(int id) =>
            await _context.ProductBrands.FindAsync(id);

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync() =>
            await _context.ProductBrands.ToListAsync();

        public async Task<Product> GetProductByIdAsync(int id) =>
            await _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(p => p.Id == id);
        
        public async Task<IReadOnlyList<Product>> GetProductsAsync() =>
            await _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .ToListAsync();

        public async Task<ProductType> GetProductTypeByIdAsync(int id) =>
            await _context.ProductTypes.FindAsync(id);

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync() =>
            await _context.ProductTypes.ToListAsync();
    }
}