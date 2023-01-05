using Grpc.Core;
using GRPCCustomers.Server.Data;
using GRPCCustomers.Server.Data.Entities;

namespace GRPCCustomers.Server.gRPCServiceImplmentation
{
    public class CustomerService : GRPCCustomers.CustomerService.CustomerServiceBase
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ILogger<CustomerService> logger;

        public CustomerService(ICustomerRepository readingRepository, ILogger<CustomerService> logger)
        {
            this.customerRepository = readingRepository;
            this.logger = logger;
        }

        public async override Task<StatusMessage> AddCustomers(CustomersPacket request, ServerCallContext context)
        {
            foreach (var item in request.Customers)
            {
                var address = await customerRepository.GetCustomerAddressAsync();

                var newCustomer = new Customer
                {
                    Name = item.Name,
                    CompanyName = item.CompanyName,
                    PhoneNumber = item.PhoneNumber,
                    Address = address,
                };

                customerRepository.AddEntity(newCustomer);
            }

            if (await customerRepository.SaveAllAsync())
            {
                return new StatusMessage
                {
                    Notes = "Reading Added Successfully",
                    Status = ServiceStatus.Success
                };
            }

            return new StatusMessage
            {
                Status = ServiceStatus.Failure,
                Notes = "Faild to add customer Reading"
            };
        }

        public async override Task AddCustomersStream(IAsyncStreamReader<AddCustomerMessage> requestStream, 
            IServerStreamWriter<StatusMessage> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                var currentCustomer = requestStream.Current;
                var address = await customerRepository.GetCustomerAddressAsync();

                var newCustomer = new Customer
                {
                    Name = currentCustomer.Name,
                    CompanyName = currentCustomer.CompanyName,
                    PhoneNumber = currentCustomer.PhoneNumber,
                    Address = address
                };

                logger.LogInformation($"Adding {newCustomer.Name} from Stream");

                customerRepository.AddEntity(newCustomer);

                if (await customerRepository.SaveAllAsync())
                {
                    await responseStream.WriteAsync(new StatusMessage()
                    {
                        Notes = $"New Customer '{currentCustomer.Name}' Added",
                        Status = ServiceStatus.Success
                    });
                }
                else
                {
                    await responseStream.WriteAsync(new StatusMessage()
                    {
                        Notes = $"Failed to add Customer '{currentCustomer.Name}'",
                        Status = ServiceStatus.Failure
                    });
                }
            }
        }
    }
}
