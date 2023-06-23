using CustomerManagement.Simulator.Customer;
using System.Net.Http.Json;

namespace CustomerManagement.Simulator
{
    public class Simulator
    {
        private readonly HttpClient _httpClient;

        public Simulator()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:7257/api/Customer/");

        }

        public async Task RunSimulation(int numberOfRequests)
        {
            // Generate a list of requests to be sent in parallel
            var requests = GenerateRequests(numberOfRequests);

            // Send requests in parallel
            await Task.WhenAll(requests);
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
                    Age = random.Next(10, 91), // Randomize age between 10 and 90
                    Id = i + 1 // Incremental ID
                };

                customers.Add(customer);
            }

            return customers;
        }

        private async Task SendPostRequest(List<CustomerDto> customers)
        {
            var response = await _httpClient.PostAsJsonAsync("addcustomers", customers);
            response.EnsureSuccessStatusCode();
        }

        private async Task SendGetRequest()
        {
            var response = await _httpClient.GetAsync("getcustomers");
            response.EnsureSuccessStatusCode();
        }

        private string GetRandomFirstName()
        {
            // Replace with your logic to generate random first names
            var firstNames = new List<string> { "Leia", "Sadie", "Jose", "Sara", "Frank", "Dewey", "Tomas", "Joel", "Lukas", "Carlos" };
            var random = new Random();
            return firstNames[random.Next(0, firstNames.Count)];
        }

        private string GetRandomLastName()
        {
            // Replace with your logic to generate random last names
            var lastNames = new List<string> { "Liberty", "Ray", "Harrison", "Ronan", "Drew", "Powell", "Larsen", "Chan", "Anderson", "Lane" };
            var random = new Random();
            return lastNames[random.Next(0, lastNames.Count)];
        }
    }
}