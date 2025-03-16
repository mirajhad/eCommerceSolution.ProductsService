using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsMicroservice.API.APIEndpoints
{
    public static class ProductAPIEndpoints
    {
        public static IEndpointRouteBuilder MapProductAPIEndpoints(this IEndpointRouteBuilder endpointRouteBuilder) 
        {
            endpointRouteBuilder.MapGet("/api/products", async (IProductsService productsService) =>
            {
                List<ProductResponse?> products = await productsService.GetProducts();
                return Results.Ok(products);
            });

            endpointRouteBuilder.MapGet("/api/products/search/product-id/{productID:guid}", async (IProductsService productsService, Guid productID) =>
            {
                ProductResponse? product = await productsService.GetProductByCondition(k => k.ProductID == productID);
                if (product == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(product);
            });

            endpointRouteBuilder.MapGet("/api/products/search/{searchString}", async (IProductsService productsService, string searchString) =>
            {
                List<ProductResponse?> productsByProductName =
                await productsService.GetProductsByCondition(k => k.ProductName != null &&  k.ProductName.Contains(searchString, StringComparison.OrdinalIgnoreCase));

                List<ProductResponse?> productsByCategory =
                await productsService.GetProductsByCondition(k => k.Category != null && k.Category.Contains(searchString, StringComparison.OrdinalIgnoreCase));

                var products = productsByProductName.Union(productsByCategory);
                return Results.Ok(products);
            });

            endpointRouteBuilder.MapPost("/api/products", async (IProductsService productsService,IValidator<ProductAddRequest> ProductAddRequestValidator, ProductAddRequest productAddRequest) =>
            {
                ValidationResult validationResult = await ProductAddRequestValidator.ValidateAsync(productAddRequest);

                if (!validationResult.IsValid)
                {
                    Dictionary<string, string[]> errors=
                    validationResult.Errors.GroupBy(k=> k.PropertyName).ToDictionary(l=>l.Key, m=> m.Select(err=>err.ErrorMessage).ToArray());
                    return Results.ValidationProblem(errors);
                }
                
                var addedProductResponse = await productsService.AddProduct(productAddRequest);
                if(addedProductResponse != null)
                {
                    return Results.Created($"/api/products/search/product-id/{addedProductResponse.ProductID}", addedProductResponse);
                }
                else
                {
                    return Results.Problem("Failed to add product");
                }

            });


            endpointRouteBuilder.MapPut("/api/products", async (IProductsService productsService, IValidator<ProductUpdateRequest> ProductUpdateRequestValidator, ProductUpdateRequest productUpdateRequest) =>
            {
                ValidationResult validationResult = await ProductUpdateRequestValidator.ValidateAsync(productUpdateRequest);

                if (!validationResult.IsValid)
                {
                    Dictionary<string, string[]> errors =
                    validationResult.Errors.GroupBy(k => k.PropertyName).ToDictionary(l => l.Key, m => m.Select(err => err.ErrorMessage).ToArray());
                    return Results.ValidationProblem(errors);
                }

                var updatedProductResponse = await productsService.UpdateProduct(productUpdateRequest);
                if (updatedProductResponse != null)
                {
                    return Results.Ok(updatedProductResponse);
                }
                else
                {
                    return Results.Problem("Failed to update product");
                }

            });

            //Delete Product
            endpointRouteBuilder.MapDelete("/api/products/{ProductID:guid}", async (IProductsService productsService, Guid ProductID) =>
            {
                

                bool isDeleted = await productsService.DeleteProduct(ProductID);
                if (isDeleted)
                {
                    return Results.Ok(true);
                }
                else
                {
                    return Results.Problem("Failed to delete product");
                }

            });

            return endpointRouteBuilder;
        }
    }
}
