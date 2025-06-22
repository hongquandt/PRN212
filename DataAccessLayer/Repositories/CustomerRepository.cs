using BusinessObjects;
using DataAccessLayer.DataContext;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly InMemoryDataContext _context;

        public CustomerRepository()
        {
            _context = InMemoryDataContext.Instance;
        }

        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.Where(c => c.CustomerStatus == 1).ToList();
        }

        public Customer? GetCustomerById(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.CustomerID == id && c.CustomerStatus == 1);
        }

        public Customer? GetCustomerByEmail(string email)
        {
            return _context.Customers.FirstOrDefault(c => c.EmailAddress == email && c.CustomerStatus == 1);
        }

        public void AddCustomer(Customer customer)
        {
            customer.CustomerID = _context.GetNextCustomerId();
            customer.CustomerStatus = 1;
            _context.Customers.Add(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            var existingCustomer = _context.Customers.FirstOrDefault(c => c.CustomerID == customer.CustomerID);
            if (existingCustomer != null)
            {
                existingCustomer.CustomerFullName = customer.CustomerFullName;
                existingCustomer.Telephone = customer.Telephone;
                existingCustomer.EmailAddress = customer.EmailAddress;
                existingCustomer.CustomerBirthday = customer.CustomerBirthday;
                existingCustomer.Password = customer.Password;
            }
        }

        public void DeleteCustomer(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (customer != null)
            {
                customer.CustomerStatus = 2; // Soft delete
            }
        }

        public List<Customer> SearchCustomers(string searchTerm)
        {
            return _context.Customers
                .Where(c => c.CustomerStatus == 1 &&
                           (c.CustomerFullName.Contains(searchTerm) ||
                            c.EmailAddress.Contains(searchTerm) ||
                            c.Telephone.Contains(searchTerm)))
                .ToList();
        }
    }
}
