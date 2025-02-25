using project.Models.DTO;
using project.Models;
using Microsoft.AspNetCore.Mvc;

namespace project.DAL
{
    public interface ICustomerDAL
    {
        Task<Customer> AddCustomer(Customer customer);
        Task<Customer> Login(string name, string password);
        Task<Customer> UpdateCustomer(Customer customer);
        Task DeleteCustomer(int id);
        Task<IEnumerable<Customer>> GetCustomers();

    }
}
