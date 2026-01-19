using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerInvoice.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerInvoice.Customers
{
    /// <summary>
    /// Application service for managing customers
    /// </summary>
    public class CustomerAppService : CrudAppService<
        Customer,
        CustomerDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateCustomerDto,
        UpdateCustomerDto>, ICustomerAppService
    {
        public CustomerAppService(IRepository<Customer, Guid> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Get a customer by email address
        /// </summary>
        public async Task<CustomerDto> GetByEmailAsync(string email)
        {
            var customer = await Repository.FirstOrDefaultAsync(c => c.Email == email);
            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }

        /// <summary>
        /// Get all invoices for a specific customer
        /// </summary>
        public async Task<List<CustomerDto>> SearchAsync(string searchTerm)
        {
            var queryable = await Repository.GetQueryableAsync();

            var query = queryable
                .WhereIf(!string.IsNullOrWhiteSpace(searchTerm),
                    c => c.Name.Contains(searchTerm) || c.Email.Contains(searchTerm));

            var customers = query.ToList();
            return ObjectMapper.Map<List<Customer>, List<CustomerDto>>(customers);
        }
    }
}
