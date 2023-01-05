using System.Text;

namespace gRPCWorkerService
{
    internal class CustomersData
    {
        static List<Customer> _Customers;
        static Random random;

        static CustomersData()
        {
            random = new Random();

            FillCustomersData();
        }

        public static List<Customer> GetCustomers() => _Customers;

        private static void FillCustomersData()
        {
            _Customers = new List<Customer>();

            int i = 0;

            do
            {
                i++;

                _Customers.Add(new Customer
                {
                    Name = $"Customer {GenerateName(10)} {i}",
                    CompanyName = $"{GenerateName(10)} Company",
                    PhoneNumber = GetRandomTelNo()
                });
            }

            while (i < 20);
        }

        static string GetRandomTelNo()
        {
            StringBuilder telNo = new StringBuilder(12);

            int number;

            for (int i = 0; i < 3; i++)
            {
                number = random.Next(0, 8);
                telNo = telNo.Append(number.ToString());
            }

            telNo = telNo.Append("-");
            number = random.Next(0, 743);
            telNo = telNo.Append(String.Format("{0:D3}", number));
            telNo = telNo.Append("-");
            number = random.Next(0, 10000);
            telNo = telNo.Append(String.Format("{0:D4}", number));

            return telNo.ToString();
        }

        static string GenerateName(int len)
        {
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[random.Next(consonants.Length)].ToUpper();
            Name += vowels[random.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[random.Next(consonants.Length)];
                b++;
                Name += vowels[random.Next(vowels.Length)];
                b++;
            }

            return Name;
        }
    }

    public record Customer
    {
        public string Name { get; init; }
        public string CompanyName { get; init; }
        public string PhoneNumber { get; init; }
    }
}
