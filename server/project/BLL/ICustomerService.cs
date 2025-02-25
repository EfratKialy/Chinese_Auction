using project.Models.DTO;
using project.Models;
using Microsoft.AspNetCore.Mvc;

namespace project.BLL
{
    public interface ICustomerService
    {
        Task<Customer> AddCustomer(Customer customer);
        Task<Customer> Login(string name, string password);
        Task<Customer> UpdateCustomer(Customer customer, int id);
        Task DeleteCustomer(int id);
        Task<IEnumerable<Customer>> GetCustomers();

    }
}
