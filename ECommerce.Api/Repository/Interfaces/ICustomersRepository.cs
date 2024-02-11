using ECommerce.Api.Entities;

namespace ECommerce.Api.Repository.Interfaces;

public interface ICustomersRepository
{
    Task<IEnumerable<Customers>> GetCustomers();
    Task<Customers> GetCustomerById(string customerId);
    Task<Customers> GetCustomerByEmailAndIdAsync(string email, string customerId);
    Task<int> AddCustomer(Customers customer);
    Task<int> UpdateCustomer(Customers customer);
    Task<int> DeleteCustomer(string customerId);
    Task<IEnumerable<Orders>> GetOrdersByCustomerId(string customerId);
}
