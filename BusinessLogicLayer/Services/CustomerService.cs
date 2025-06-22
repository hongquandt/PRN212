using BusinessObjects;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
        }

        public List<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAllCustomers();
        }

        public Customer? GetCustomerById(int id)
        {
            return _customerRepository.GetCustomerById(id);
        }

        public Customer? AuthenticateCustomer(string email, string password)
        {
            var customer = _customerRepository.GetCustomerByEmail(email);
            if (customer != null && customer.Password == password)
            {
                return customer;
            }
            return null;
        }

        public void AddCustomer(Customer customer)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(customer.CustomerFullName))
                throw new ArgumentException("Customer name is required");

            if (string.IsNullOrWhiteSpace(customer.EmailAddress))
                throw new ArgumentException("Email address is required");

            // Check if email already exists
            var existingCustomer = _customerRepository.GetCustomerByEmail(customer.EmailAddress);
            if (existingCustomer != null)
                throw new ArgumentException("Email address already exists");

            _customerRepository.AddCustomer(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(customer.CustomerFullName))
                throw new ArgumentException("Customer name is required");

            if (string.IsNullOrWhiteSpace(customer.EmailAddress))
                throw new ArgumentException("Email address is required");

            _customerRepository.UpdateCustomer(customer);
        }

        public void DeleteCustomer(int id)
        {
            _customerRepository.DeleteCustomer(id);
        }

        public List<Customer> SearchCustomers(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return GetAllCustomers();

            return _customerRepository.SearchCustomers(searchTerm);
        }
    }
}
