using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProduct(Guid productID)
        {
            _context.Products.Remove(_context?.Products?.Find(productID));
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionexpression)
        {
            return await _context.Products.FirstOrDefaultAsync(conditionexpression);

        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionexpression)
        {
            return await _context.Products.Where(conditionexpression).ToListAsync();
        }

        public async Task<Product?> UpdateProduct(Product product)
        {
            Product? existingProduct = await _context.Products.FirstOrDefaultAsync(k => k.ProductID == product.ProductID);
            if(existingProduct == null)
            {
                return null;
            }
            else
            {
                existingProduct.ProductName = product.ProductName;
                existingProduct.Category = product.Category;
                existingProduct.UnitPrice = product.UnitPrice;
                existingProduct.QuantityInStock = product.QuantityInStock;
                await _context.SaveChangesAsync();
                return existingProduct;
            }
        }
    }
}
