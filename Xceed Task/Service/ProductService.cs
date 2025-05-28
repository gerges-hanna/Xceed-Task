using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Xceed_Task.Data;
using Xceed_Task.Models;

namespace Xceed_Task.Service
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync(int? categoryId)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();
            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId);
            return await query.ToListAsync();
        }

        public async Task<List<Product>> GetPublicProductsAsync(int? categoryId)
        {
            var now = DateTime.Now;
            var query = _context.Products
                .Include(p => p.Category)
                .Where(p => now >= p.StartDate && now <= p.StartDate.AddHours(p.DurationInHours));
            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId);
            return await query.ToListAsync();
        }

        public async Task<Product> GetProductDetailsAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.CreatedByUser)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task CreateProductAsync(Product product, string userId)
        {
            product.CreatedByUserId = userId;
            product.CreationDate = DateTime.Now;
            _context.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var existing = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == product.Id);
            if (existing == null)
                return false;

            product.CreatedByUserId = existing.CreatedByUserId;
            product.CreationDate = existing.CreationDate;
            _context.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<SelectList> GetCategoriesSelectListAsync(int? selectedId = null)
        {
            var categories = await _context.Categories.ToListAsync();
            return new SelectList(categories, "Id", "Name", selectedId);
        }
    }
}
