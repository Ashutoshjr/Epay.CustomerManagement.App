using AutoMapper;
using CustomerManagement.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Application.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(IMapper mapper, ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAllCustomers();
            var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            return customerDtos;
        }

        public async Task AddCustomers(List<CustomerDto> customersDto)
        {
           var customers = _mapper.Map<List<Customer>>(customersDto);
           await _customerRepository.AddCustomer(customers);
        }

       
    }
}
