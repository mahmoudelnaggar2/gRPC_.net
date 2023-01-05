using Grpc.Net.Client;
using GRPCCustomers;
using gRPCWorkerService;

namespace CustomerWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private CustomerService.CustomerServiceClient _gRPCclient;

        private CustomerService.CustomerServiceClient gRPCClient
        {
            get
            {
                if (_gRPCclient == null)
                {
                    var grpcChannel = GrpcChannel.ForAddress("https://localhost:7032");

                    _gRPCclient = new CustomerService.CustomerServiceClient(grpcChannel);
                }
                return _gRPCclient;
            }
        }

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                //await AddCustomersList();

                await AddCustomersStream();


                await Task.Delay(5000, stoppingToken);
            }
        }

        async Task AddCustomersList()
        {
            var grpcRequest = new CustomersPacket();

            var customersList = CustomersData.GetCustomers();

            foreach (var item in customersList)
            {
                grpcRequest.Customers.Add(new AddCustomerMessage
                {

                    Name = item.Name,
                    CompanyName = item.CompanyName,
                    PhoneNumber = item.PhoneNumber
                });
            }

            var replay = await gRPCClient.AddCustomersAsync(grpcRequest);

            if (replay.Status == ServiceStatus.Success)
            {
                Console.WriteLine(replay.Notes);
            }
        }

        async Task AddCustomersStream()
        {
            var customersList = CustomersData.GetCustomers();
            var stream = gRPCClient.AddCustomersStream();

            foreach (var item in customersList)
            {
                var currentCustomer = new AddCustomerMessage
                {
                    Name = item.Name,
                    CompanyName = item.CompanyName,
                    PhoneNumber = item.PhoneNumber
                };

                await stream.RequestStream.WriteAsync(currentCustomer);

                await Task.Delay(500);
            }

            await stream.RequestStream.CompleteAsync();

            while (await stream.ResponseStream.MoveNext(new CancellationToken()))
            {
                Console.WriteLine($"{stream.ResponseStream.Current.Status} : " +
                    $"{stream.ResponseStream.Current.Notes}");
            }
        }
    }
}