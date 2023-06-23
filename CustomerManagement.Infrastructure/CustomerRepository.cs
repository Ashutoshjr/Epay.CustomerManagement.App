using CustomerManagement.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomerManagement.Infrastructure
{
    public class CustomerRepository : ICustomerRepository
    {
        private List<Customer> _customers;
        private readonly string _filePath = "customers.json";
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);


        public CustomerRepository()
        {
            _filePath = "customers.json";
            LoadCustomersFromJsonFileAsync().GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await Task.FromResult(_customers);
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
            using (var fileStream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                await JsonSerializer.SerializeAsync(fileStream, _customers, options);
            }
        }
    }
}
