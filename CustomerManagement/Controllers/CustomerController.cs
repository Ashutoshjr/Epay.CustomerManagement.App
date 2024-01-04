using CustomerManagement.Application.Customers;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;


        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;

        }

        [HttpPost("addcustomers")]
        public async Task<IActionResult> AddCustomers([FromBody] List<CustomerDto> customerDtos)
        {
            try
            {
                await _customerService.AddCustomers(customerDtos);
                return Ok("Customers added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding customers: {ex.Message}");
            }
        }

        [HttpGet("getcustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                IEnumerable<CustomerDto>? customers = await _customerService.GetAllCustomers();

                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving customers: {ex.Message}");
            }
        }
    }
}
