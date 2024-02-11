using Dapper;
using ECommerce.Api.Entities;
using ECommerce.Api.Repository.Interfaces;
using System.Data;

namespace ECommerce.Api.Repository
{
    public class CustomersRepository : ICustomersRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<CustomersRepository> _logger;

        public CustomersRepository(IDbConnection dbConnection, ILogger<CustomersRepository> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Customers>> GetCustomers()
        {
            try
            {
                if (_dbConnection.State == ConnectionState.Closed)
                {
                    _dbConnection.Open();
                }

                var customers = await _dbConnection.QueryAsync<Customers>("SELECT * FROM Customers");

                if (customers.Any())
                {
                    foreach (var customer in customers)
                    {
                        var orders = await GetOrdersByCustomerId(customer.CustomerId);
                        customer.Orders = orders.ToList<Orders>();
                    }
                }
                return customers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCustomers");
                throw;
            }
            finally
            {
                _dbConnection.Close();
            }
        }

        public async Task<Customers> GetCustomerById(string customerId)
        {
            try
            {
                if (_dbConnection.State == ConnectionState.Closed)
                {
                    _dbConnection.Open();
                }
                var customer = await _dbConnection.QueryFirstOrDefaultAsync<Customers>("SELECT * FROM Customers WHERE CustomerId = @CustomerId", new { CustomerId = customerId });
                if (customer != null)
                {
                    var orders = await GetOrdersByCustomerId(customer.CustomerId);
                    customer.Orders = orders.ToList<Orders>();
                }
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCustomerById");
                throw;
            }
            finally
            {
                _dbConnection.Close();
            }
        }

        public async Task<Customers> GetCustomerByEmailAndIdAsync(string email, string customerId)
        {
            try
            {
                if (_dbConnection.State == ConnectionState.Closed)
                {
                    _dbConnection.Open();
                }
                var customer = await _dbConnection.QueryFirstOrDefaultAsync<Customers>("SELECT * FROM Customers WHERE Email = @Email AND CustomerId = @CustomerId", new { Email = email, CustomerId = customerId });
                if (customer != null)
                {
                    var orders = await GetOrdersByCustomerId(customer.CustomerId);
                    customer.Orders = orders.ToList<Orders>();
                }
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCustomerByEmailAndIdAsync");
                throw;
            }
            finally
            {
                _dbConnection.Close();
            }
        }

        public async Task<int> AddCustomer(Customers customer)
        {
            try
            {
                if (_dbConnection.State == ConnectionState.Closed)
                {
                    _dbConnection.Open();
                }
                return await _dbConnection.ExecuteAsync("INSERT INTO Customers (CustomerId, FirstName, LastName, Email, HouseNo, Street, Town, PostCode) VALUES (@CustomerId, @FirstName, @LastName, @Email, @HouseNo, @Street, @Town, @PostCode)", customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddCustomer");
                throw;
            }
            finally
            {
                _dbConnection.Close();
            }
        }

        public async Task<int> UpdateCustomer(Customers customer)
        {
            try
            {
                if (_dbConnection.State == ConnectionState.Closed)
                {
                    _dbConnection.Open();
                }
                return await _dbConnection.ExecuteAsync("UPDATE Customers SET FirstName = @FirstName, LastName = @LastName, Email = @Email, HouseNo = @HouseNo, Street = @Street, Town = @Town, PostCode = @PostCode WHERE CustomerId = @CustomerId", customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateCustomer");
                throw;
            }
            finally
            {
                _dbConnection.Close();
            }
        }

        public async Task<int> DeleteCustomer(string customerId)
        {
            try
            {
                if (_dbConnection.State == ConnectionState.Closed)
                {
                    _dbConnection.Open();
                }
                return await _dbConnection.ExecuteAsync("DELETE FROM Customers WHERE CustomerId = @CustomerId", new { CustomerId = customerId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteCustomer");
                throw;
            }
            finally
            {
                _dbConnection.Close();
            }
        }

        public async Task<IEnumerable<Orders>> GetOrdersByCustomerId(string customerId)
        {
            try
            {
                if (_dbConnection.State == ConnectionState.Closed)
                {
                    _dbConnection.Open();
                }
                return await _dbConnection.QueryAsync<Orders>("SELECT * FROM Orders WHERE CustomerId = @CustomerId", new { CustomerId = customerId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetOrdersByCustomerId");
                throw;
            }
            finally
            {
                _dbConnection.Close();
            }
        }
    }
}
