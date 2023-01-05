using GRPCCustomers.Server.Data.Entities;

namespace GRPCCustomers.Server.Data;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetCustomersAsync();
    Task<IEnumerable<Customer>> GetCustomersWithReadingsAsync();
    Task<Customer?> GetCustomerAsync(int id);
    Task<Customer?> GetCustomerWithReadingsAsync(int id);
    Task<Address?> GetCustomerAddressAsync();

    void AddEntity<T>(T model) where T : notnull;
    void DeleteEntity<T>(T model) where T : notnull;
    Task<bool> SaveAllAsync();
}
