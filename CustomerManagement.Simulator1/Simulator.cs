using CustomerManagement.Simulator.Customer;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace CustomerManagement.Simulator
{
    public class Simulator
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress = "https://localhost:7257/api/Customer/";

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
            var postRequests = GeneratePostRequests(numberOfRequests);
            var getRequests = GenerateGetRequests(numberOfRequests);

            // Send POST and GET requests in parallel
            await Task.WhenAll(postRequests);
            await Task.WhenAll(getRequests);

            Console.WriteLine("Parallel execution is complete.");
        }


        private IEnumerable<Task> GeneratePostRequests(int numberOfRequests)
        {
            var tasks = Enumerable.Range(0, numberOfRequests).Select(async _ =>
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_baseAddress);
                    var customers = GenerateCustomers();
                    await SendPostRequest(httpClient, customers);
                }
            });

            return tasks;
        }

        private IEnumerable<Task> GenerateGetRequests(int numberOfRequests)
        {
            var tasks = Enumerable.Range(0, numberOfRequests).Select(async _ =>
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_baseAddress);
                    await SendGetRequest(httpClient);
                }
            });

            return tasks;
        }

        //private List<Task> GeneratePostRequests(int numberOfRequests)
        //{
        //    var requests = new List<Task>();

        //    for (int i = 0; i < numberOfRequests; i++)
        //    {
        //        var customers = GenerateCustomers();

        //        // Start the POST request without waiting for its completion

        //        var postTask = SendPostRequest(customers);


        //        requests.Add(postTask);
        //    }

        //    return requests;
        //}

        //private List<Task> GenerateGetRequests(int numberOfRequests)
        //{
        //    var requests = new List<Task>();

        //    for (int i = 0; i < numberOfRequests; i++)
        //    {

        //        var getTask = SendGetRequest();
        //        requests.Add(getTask);
        //    }

        //    return requests;
        //}

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

        private async Task SendPostRequest(HttpClient httpClient, List<CustomerDto> customers)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("addcustomers", customers);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();


                    Console.WriteLine(responseContent);
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }




        private async Task SendGetRequest(HttpClient httpClient)
        {
            try
            {
                var response = await _httpClient.GetAsync("getcustomers");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var exchangeRates = JsonConvert.DeserializeObject<List<CustomerDto>>(responseContent);


                    Console.WriteLine(exchangeRates?.Count);
                }


            }
            catch (Exception ex)
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