using Microsoft.Extensions.Configuration;
using BusinessObjects;

namespace BusinessLogicLayer.Services
{
    public class AuthenticationService
    {
        private readonly ICustomerService _customerService;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IConfiguration configuration)
        {
            _customerService = new CustomerService();
            _configuration = configuration;
        }

        public (bool IsAuthenticated, string Role, Customer? Customer) Authenticate(string email, string password)
        {
            // Check admin credentials
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            if (email == adminEmail && password == adminPassword)
            {
                return (true, "Admin", null);
            }

            // Check customer credentials
            var customer = _customerService.AuthenticateCustomer(email, password);
            if (customer != null)
            {
                return (true, "Customer", customer);
            }

            return (false, string.Empty, null);
        }
    }
}
