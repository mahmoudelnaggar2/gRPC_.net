using GRPCCustomers.Server.Data;
using GRPCCustomers.Server.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GRPCCustomers.Server.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICustomerRepository customerRepository;

        public IndexModel(ILogger<IndexModel> logger, ICustomerRepository customerRepository)
        {
            _logger = logger;
            this.customerRepository = customerRepository;
        }

        public IList<Customer>? Customers { get; set; } = default;

        public async Task OnGetAsync()
        {
            var customersResult =  await customerRepository.GetCustomersAsync();

            Customers = customersResult.ToList();
        }
    }
}