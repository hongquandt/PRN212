using BusinessObjects;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    public interface ICustomerService
    {
        List<Customer> GetAllCustomers();
        Customer? GetCustomerById(int id);
        Customer? AuthenticateCustomer(string email, string password);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);
        List<Customer> SearchCustomers(string searchTerm);
    }
}
