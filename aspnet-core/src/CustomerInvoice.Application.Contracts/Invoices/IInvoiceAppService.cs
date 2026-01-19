using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace CustomerInvoice.Invoices
{
    /// <summary>
    /// Application service interface for managing invoices
    /// </summary>
    public interface IInvoiceAppService : ICrudAppService<
        InvoiceDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateInvoiceDto,
        UpdateInvoiceDto>
    {
        /// <summary>
        /// Updates the status of an invoice
        /// </summary>
        Task<InvoiceDto> UpdateStatusAsync(Guid id, UpdateInvoiceStatusDto input);

        /// <summary>
        /// Gets all invoices for a specific customer
        /// </summary>
        Task<List<InvoiceDto>> GetByCustomerIdAsync(Guid customerId);

        /// <summary>
        /// Adds a line item to an invoice
        /// </summary>
        Task<InvoiceDto> AddLineItemAsync(Guid invoiceId, CreateLineItemDto input);

        /// <summary>
        /// Updates a line item on an invoice
        /// </summary>
        Task<InvoiceDto> UpdateLineItemAsync(Guid invoiceId, Guid lineItemId, UpdateLineItemDto input);

        /// <summary>
        /// Removes a line item from an invoice
        /// </summary>
        Task<InvoiceDto> RemoveLineItemAsync(Guid invoiceId, Guid lineItemId);
    }
}
