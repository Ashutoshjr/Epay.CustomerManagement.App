using CustomerManagement.Simulator.Customer;
using System.Net.Http.Json;

namespace CustomerManagement.Simulator
{
    public class Simulator
    {
        private readonly HttpClient _httpClient;

        public Simulator()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri("https://localhost:7257/api/Customer/");

        }

        public async Task RunSimulation(int numberOfRequests)
        {
            // Generate a list of requests to be sent in parallel
            var requests = GenerateRequests(numberOfRequests);

            // Send requests in parallel
            await Task.WhenAll(requests);

            Console.WriteLine("parallel execuation is complete ");
        }

        private List<Task> GenerateRequests(int numberOfRequests)
        {
            var requests = new List<Task>();

            for (int i = 0; i < numberOfRequests; i++)
            {
                var customers = GenerateCustomers();
                var postTask = SendPostRequest(customers);
                var getTask = SendGetRequest();

                requests.Add(postTask);
                requests.Add(getTask);
            }

            return requests;
        }

        private List<CustomerDto> GenerateCustomers()
        {
            var customers = new List<CustomerDto>();
            var random = new Random();

            // Generate at least 2 different customers
            for (int i = 0; i < 2; i++)
            {
                var customer = new CustomerDto
                {
                    FirstName = GetRandomFirstName(),
                    LastName = GetRandomLastName(),
                    Age = random.Next(10, 91), 
                    Id = i + 1 
                };

                customers.Add(customer);
            }

            return customers;
        }

        private async Task SendPostRequest(List<CustomerDto> customers)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("addcustomers", customers);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task SendGetRequest()
        {
            try
            {
                var response = await _httpClient.GetAsync("getcustomers");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string GetRandomFirstName()
        {
            var firstNames = new List<string> { "Leia", "Sadie", "Jose", "Sara", "Frank", "Dewey", "Tomas", "Joel", "Lukas", "Carlos" };
            var random = new Random();
            return firstNames[random.Next(0, firstNames.Count)];
        }

        private string GetRandomLastName()
        {
            var lastNames = new List<string> { "Liberty", "Ray", "Harrison", "Ronan", "Drew", "Powell", "Larsen", "Chan", "Anderson", "Lane" };
            var random = new Random();
            return lastNames[random.Next(0, lastNames.Count)];
        }
    }
}