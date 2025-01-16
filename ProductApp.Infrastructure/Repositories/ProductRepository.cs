using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using SharedLibrary.Logs;
using SharedLibrary.Responses;
using System.Linq.Expressions;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext dbContext) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity, CancellationToken cancellationToken)
        {
            try
            {
                var getProduct = await GetByAsync(x => x.Name!.Equals(entity.Name));
                if (getProduct is not null && !string.IsNullOrEmpty(getProduct.Name))
                    return new Response(false, $"{entity.Name} is already exists");

                var currentEntity = dbContext.Products.Add(entity).Entity;
                await dbContext.SaveChangesAsync(cancellationToken);
                if (currentEntity is not null && currentEntity.Id > 0)
                    return new Response(true, $"{entity.Name} added to database successfully");
                else
                    return new Response(false, $"Error occured while adding {entity.Name}");

            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error occured adding new product");
            }
        }

        public async Task<Response> DeleteAsync(Product entity, CancellationToken cancellationToken)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id, cancellationToken);
                if (product is null)
                    return new Response(false, $"{entity.Name} not found");

                dbContext.Products.Remove(entity);
                await dbContext.SaveChangesAsync(cancellationToken);
                return new Response(true, $"{entity.Name} is deleted successfully");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error occured deleting product");
            }
        }

        public async Task<Product> FindByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var product = await dbContext.Products.FindAsync(id, cancellationToken);
                return product ?? null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Error occured while retrieving products");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                var products = await dbContext.Products.AsNoTracking().ToListAsync(cancellationToken);
                return products ?? null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Error occured while retrieving products");
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var product = await dbContext.Products.Where(predicate).FirstOrDefaultAsync();
                return product ?? null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Error occured while retrieving products");
            }
        }

        public async Task<Response> UpdateAsync(Product entity, CancellationToken cancellationToken)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id, cancellationToken);
                if (product is null)
                    return new Response(false, $"{entity.Name} not found");

                dbContext.Entry(entity).State = EntityState.Detached;
                dbContext.Update(product);
                await dbContext.SaveChangesAsync(cancellationToken);
                return new Response(true, $"{product.Name} is updated successfully");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error occured updated existing product");
            }
        }
    }
}
