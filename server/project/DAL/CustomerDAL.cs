using project.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace project.DAL
{
    public class CustomerDAL : ICustomerDAL
    {
        private readonly Context context;
        public CustomerDAL(Context context) 
        {
            this.context = context;
        }
        public async Task<Customer> AddCustomer(Customer customer)
        {
            Customer c = await context.Customers.FirstOrDefaultAsync(x => x.Name == customer.Name && x.Password == customer.Password);
            if (c != null)
            {
                throw new Exception("There is a user with this password. Try another password");
            }
            try
            {
                await context.Customers.AddAsync(customer);
                await context.SaveChangesAsync();
                return customer;
            }
            catch (Exception ex)
            {
                // לוג את החריגה
                throw new Exception("An error occurred while adding the customer", ex);
            }
        }

        public async Task DeleteCustomer(int id)
        {
            try
            {
                var d = await context.Customers.FirstOrDefaultAsync(dd => dd.Id == id);
                if (d == null)
                {
                    throw new Exception($"Customer {id} not found");
                }
                context.Customers.Remove(d);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            try
            {
                return await context.Customers.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Customer> Login(string name, string password)
        {
            return await context.Customers.FirstAsync(x=>x.Name == name && x.Password == password);
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            try
            {
                context.Customers.Update(customer);

                await context.SaveChangesAsync();

                return customer;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
