using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entites;
using OrderApi.Infrastructure.Data;
using SharedLibrary.Logs;
using SharedLibrary.Responses;
using System.Linq.Expressions;

namespace OrderApi.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext dbContext) : IOrder
    {
        public async Task<Response> CreateAsync(Order entity, CancellationToken cancellationToken)
        {
            try
            {
                var order = dbContext.Add(entity).Entity;
                await dbContext.SaveChangesAsync(cancellationToken);
                return order.Id > 0 ? new Response(true, "Order placed successfully") :
                    new Response(false, "Error occured while placing order");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occured while placing order");
            }
        }

        public async Task<Response> DeleteAsync(Order entity, CancellationToken cancellationToken)
        {
            try
            {
                var order = await FindByIdAsync(entity.Id, cancellationToken);
                if (order is null)
                    return new Response(false, "Order not found");

                dbContext.Orders.Remove(order);
                await dbContext.SaveChangesAsync(cancellationToken);
                return new Response(true, "Order successfully deleted");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occured while placing order");
            }
        }

        public async Task<Order> FindByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var order = await dbContext.Orders.FindAsync(id);
                return order ?? null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occured while retrieving order");
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                var orders = await dbContext.Orders.AsNoTracking().ToListAsync(cancellationToken);
                return orders ?? null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occured while retrieving orders");
            }
        }

        public async Task<Order> GetByAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order = await dbContext.Orders.Where(predicate).FirstOrDefaultAsync();
                return order ?? null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occured while retrieving order");
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var orders = await dbContext.Orders.Where(predicate).AsNoTracking().ToListAsync();
                return orders ?? null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occured while retrieving order");
            }
        }

        public async Task<Response> UpdateAsync(Order entity, CancellationToken cancellationToken)
        {
            try
            {
                var order = await FindByIdAsync(entity.Id, cancellationToken);
                if (order is null)
                    return new Response(false, "Order noto found");

                dbContext.Entry(order).State = EntityState.Detached;
                await dbContext.SaveChangesAsync(cancellationToken);
                return new Response(true, "Order updated successfully");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occured while updating order");
            }
        }
    }
}
