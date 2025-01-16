﻿using OrderApi.Domain.Entites;
using SharedLibrary.Interface;
using System.Linq.Expressions;

namespace OrderApi.Application.Interfaces
{
    public interface IOrder : IGenericInterface<Order>
    {
        Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate);
    }
}
