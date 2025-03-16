using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IValidator<ProductAddRequest> _productAddRequestValidator;
        private readonly IValidator<ProductUpdateRequest> _productUpdateRequestValidator;
        private readonly IMapper _mapper;
        private readonly IProductsRepository _productsRepository;

        public ProductsService(IValidator<ProductAddRequest> productAddRequestValidator, IValidator<ProductUpdateRequest> productUpdateRequestValidator, IMapper mapper, IProductsRepository productsRepository)
        {
            _productAddRequestValidator = productAddRequestValidator;
            _productUpdateRequestValidator = productUpdateRequestValidator;
            _mapper = mapper;
            _productsRepository = productsRepository;
        }

        public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
        {
            if (productAddRequest == null)
            {
                throw new ArgumentNullException(nameof(productAddRequest));
            }

            ValidationResult validationResult = await _productAddRequestValidator.ValidateAsync(productAddRequest);

            if (!validationResult.IsValid)
            {
                string errors = String.Join(", ", validationResult.Errors.Select(k => new { k.ErrorMessage }));
                throw new ArgumentException(errors);
            }

            Product product = _mapper.Map<Product>(productAddRequest);
            Product? addedProduct = await _productsRepository.AddProduct(product);

            if (addedProduct == null)
            {
                return null;
            }

            ProductResponse productResponse = _mapper.Map<ProductResponse>(addedProduct);
            return productResponse;
        }

        public async Task<bool> DeleteProduct(Guid productID)
        {
            Product? product = await _productsRepository.GetProductByCondition(k => k.ProductID == productID);

            if(product == null)
            {
                return false;
            }
            else
            {
                bool isDeleted = await _productsRepository.DeleteProduct(productID);
                return isDeleted;
            }
        }

        public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionexpression)
        {
            Product? product = await _productsRepository.GetProductByCondition(conditionexpression);
            if (product == null) 
            {
                return null;
            }
            else
            {
                ProductResponse productResponse = _mapper.Map<ProductResponse>(product);
                return productResponse;
            }
        }

        public async Task<List<ProductResponse?>> GetProducts()
        {
            IEnumerable<Product?> products = await _productsRepository.GetProducts();
            if (products == null)
            {
                return null;
            }
            else
            {
                IEnumerable<ProductResponse?> productsResponse = _mapper.Map<IEnumerable<ProductResponse>>(products);
                return productsResponse.ToList();
            }
        }

        public async Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionexpression)
        {
            IEnumerable<Product?> products = await _productsRepository.GetProductsByCondition(conditionexpression);
            if (products == null)
            {
                return null;
            }
            else
            {
                IEnumerable<ProductResponse?> productsResponse = _mapper.Map<IEnumerable<ProductResponse>>(products);
                return productsResponse.ToList();
            }
        }

        public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
        {
            Product? existingProduct = await _productsRepository.GetProductByCondition(k => k.ProductID == productUpdateRequest.ProductID);

            if (existingProduct == null) 
            {
                throw new ArgumentException("Product does not exist");
            }
            else
            {
                ValidationResult validationResult = await _productUpdateRequestValidator.ValidateAsync(productUpdateRequest);
                if (!validationResult.IsValid)
                {
                    string errors = String.Join(", ", validationResult.Errors.Select(k => new { k.ErrorMessage }));
                    throw new ArgumentException(errors);
                }
                Product product = _mapper.Map<Product>(productUpdateRequest);
                Product? updatedProduct = await _productsRepository.UpdateProduct(product);
                if (updatedProduct == null)
                {
                    return null;
                }
                ProductResponse productResponse = _mapper.Map<ProductResponse>(updatedProduct);
                return productResponse;
            }
        }
    }
}
