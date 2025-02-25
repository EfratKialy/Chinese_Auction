using project.DAL;
using project.Models;
using project.Models.DTO;
using System.Drawing;

namespace project.BLL
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerDAL customerDAL;
        private readonly EmailValidator validator;

        public CustomerService(ICustomerDAL customerDAL, EmailValidator validator) 
        {
            this.customerDAL = customerDAL;
            this.validator = validator;
        }
        public async Task<Customer> AddCustomer(Customer customer)
        {
            if (!validator.IsValidEmail(customer.Email))
            {
                throw new ArgumentException();
            }
            return await customerDAL.AddCustomer(customer);
        }

        public async Task DeleteCustomer(int id)
        {
            customerDAL.DeleteCustomer(id);
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await customerDAL.GetCustomers();
        }

        public async Task<Customer> Login(string name, string password)
        {
            return await customerDAL.Login(name, password);
        }

        public async Task<Customer> UpdateCustomer(Customer customer, int id)
        {
            customer.Id=id;
            return await customerDAL.UpdateCustomer(customer);
        }
    }
}
