using GRPCCustomers.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GRPCCustomers.Server.Data;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CustomerRepository> _logger;

    public CustomerRepository(ApplicationDbContext context, ILogger<CustomerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        var result = await _context.Customers
          .Include(c => c.Address)
          .OrderBy(c => c.Name)
          .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Customer>> GetCustomersWithReadingsAsync()
    {
        var result = await _context.Customers
          .Include(c => c.Address)          
          .OrderBy(c => c.Name)
          .ToListAsync();

        return result;
    }

    public async Task<Customer?> GetCustomerAsync(int id)
    {
        var result = await _context.Customers
          .Include(c => c.Address)
          .OrderBy(c => c.Name)
          .Where(c => c.Id == id)
          .FirstOrDefaultAsync();

        return result;
    }

    public async Task<Customer?> GetCustomerWithReadingsAsync(int id)
    {
        var result = await _context.Customers
          .Include(c => c.Address)          
          .OrderBy(c => c.Name)
          .Where(c => c.Id == id)
          .FirstOrDefaultAsync();

        return result;
    }

    public async Task<Address?> GetCustomerAddressAsync()
    {
        int randomeIndex = new Random().Next(1, 2);

        return await _context.Addresses.FindAsync(randomeIndex);
    }

    public void AddEntity<T>(T model) where T : notnull
    {
        _context.Add(model);
    }

    public void DeleteEntity<T>(T model) where T : notnull
    {
        _context.Remove(model);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}

