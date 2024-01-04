using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.Application.Customers
{
    public record CustomerDto
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(18, int.MaxValue, ErrorMessage = "Age must be above 18.")]
        public int Age { get; set; }


    }
}
