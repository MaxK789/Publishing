using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Publishing.Core.Interfaces
{
    public interface IStatisticRepository
    {
        Task<List<string[]>> GetAuthorNamesAsync();
        Task<List<string[]>> GetOrdersPerMonthAsync();
        Task<List<string[]>> GetOrdersPerAuthorAsync();
        Task<List<string[]>> GetOrdersPerAuthorAsync(string fullName);
        Task<List<string[]>> GetOrdersPerMonthAsync(DateTime start, DateTime end);
        Task<List<string[]>> GetTiragePerAuthorAsync();
    }
}
