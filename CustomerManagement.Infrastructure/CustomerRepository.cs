using CustomerManagement.Domain.Customers;
//using Newtonsoft.Json;
using System.Text.Json;

namespace CustomerManagement.Infrastructure
{
    public class CustomerRepository : ICustomerRepository
    {
        private List<Customer> _customers;
        private readonly string _filePath = "customers.json";
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);


        public CustomerRepository()
        {
            LoadCustomersFromJsonFileAsync().Wait();
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            await _semaphore.WaitAsync();
            try
            {
                // Return a copy of _customers to avoid external modifications
                return _customers.ToList();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task AddCustomer(List<Customer> customers)
        {

            await _semaphore.WaitAsync();

            try
            {

                foreach (var customer in customers)
                {
                    InsertCustomerSorted(customer);
                }


                await SaveCustomersToJsonFileAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private void InsertCustomerSorted(Customer customer)
        {
            Task.Delay(1000);

            int index = 0;
            while (index < _customers.Count &&
                   String.Compare(customer.LastName, _customers[index].LastName, StringComparison.Ordinal) > 0)
            {
                index++;
            }

            while (index < _customers.Count &&
                   String.Compare(customer.LastName, _customers[index].LastName, StringComparison.Ordinal) == 0 &&
                   String.Compare(customer.FirstName, _customers[index].FirstName, StringComparison.Ordinal) > 0)
            {
                index++;
            }

            _customers.Insert(index, customer);
        }


        private async Task LoadCustomersFromJsonFileAsync()
        {
            await Task.Delay(1000);

            using (var fileStream = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                try
                {

                    _customers = await JsonSerializer.DeserializeAsync<List<Customer>>(fileStream, options);

                }
                catch (JsonException)
                {
                    _customers = new List<Customer>();
                }
            }


        }


        private async Task SaveCustomersToJsonFileAsync()
        {

            await Task.Delay(500);
            using (var fileStream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                await System.Text.Json.JsonSerializer.SerializeAsync(fileStream, _customers, options);
            }
        }
    }
}
