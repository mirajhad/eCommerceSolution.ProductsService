

using DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace DataAccessLayer.RepositoryContracts
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionexpression);
        Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionexpression);
        Task<Product?> AddProdcut(Product product);
        Task<Product?> UpdateProduct(Product product);
        Task<bool> DeleteProduct(Guid productID);
    }
}
