using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace CustomerInvoice.Customers
{
    /// <summary>
    /// Application service interface for managing customers
    /// </summary>
    public interface ICustomerAppService : ICrudAppService<
        CustomerDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateCustomerDto,
        UpdateCustomerDto>
    {
        /// <summary>
        /// Get a customer by email address
        /// </summary>
        Task<CustomerDto> GetByEmailAsync(string email);

        /// <summary>
        /// Search customers by name or email
        /// </summary>
        Task<List<CustomerDto>> SearchAsync(string searchTerm);
    }
}
