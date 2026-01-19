using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerInvoice.Entities;
using CustomerInvoice.Services;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace CustomerInvoice.Invoices
{
    /// <summary>
    /// Application service for managing invoices
    /// </summary>
    public class InvoiceAppService : CrudAppService<
        Invoice,
        InvoiceDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateInvoiceDto,
        UpdateInvoiceDto>, IInvoiceAppService
    {
        private readonly IRepository<Invoice, Guid> _invoiceRepository;
        private readonly IRepository<LineItem, Guid> _lineItemRepository;
        private readonly IRepository<Customer, Guid> _customerRepository;
        private readonly IInvoiceNumberGenerator _invoiceNumberGenerator;

        public InvoiceAppService(
            IRepository<Invoice, Guid> repository,
            IRepository<LineItem, Guid> lineItemRepository,
            IRepository<Customer, Guid> customerRepository,
            IInvoiceNumberGenerator invoiceNumberGenerator)
            : base(repository)
        {
            _invoiceRepository = repository;
            _lineItemRepository = lineItemRepository;
            _customerRepository = customerRepository;
            _invoiceNumberGenerator = invoiceNumberGenerator;
        }

        /// <summary>
        /// Creates a new invoice with line items
        /// </summary>
        public override async Task<InvoiceDto> CreateAsync(CreateInvoiceDto input)
        {
            // Verify customer exists
            var customer = await _customerRepository.GetAsync(input.CustomerId);

            // Generate invoice number
            var invoiceNumber = await _invoiceNumberGenerator.GenerateAsync();

            // Create invoice
            var invoice = new Invoice(
                GuidGenerator.Create(),
                invoiceNumber,
                input.CustomerId,
                input.InvoiceDate ?? DateTime.Now,
                input.DueDate,
                InvoiceStatus.Draft
            )
            {
                TaxAmount = input.TaxAmount
            };

            // Add line items
            foreach (var lineItemDto in input.LineItems)
            {
                var lineItem = new LineItem(
                    GuidGenerator.Create(),
                    invoice.Id,
                    lineItemDto.Description,
                    lineItemDto.Quantity,
                    lineItemDto.UnitPrice
                );
                invoice.AddLineItem(lineItem);
            }

            // Save invoice
            await _invoiceRepository.InsertAsync(invoice);

            // Save changes to ensure the invoice is persisted before querying
            await CurrentUnitOfWork.SaveChangesAsync();

            return await GetAsync(invoice.Id);
        }

        /// <summary>
        /// Gets invoice with line items included
        /// </summary>
        public override async Task<InvoiceDto> GetAsync(Guid id)
        {
            var queryable = await _invoiceRepository.WithDetailsAsync(i => i.LineItems, i => i.Customer);
            var invoice = await AsyncExecuter.FirstOrDefaultAsync(queryable, i => i.Id == id);

            if (invoice == null)
            {
                throw new EntityNotFoundException(typeof(Invoice), id);
            }

            return ObjectMapper.Map<Invoice, InvoiceDto>(invoice);
        }

        /// <summary>
        /// Gets a paged list of invoices
        /// </summary>
        public override async Task<PagedResultDto<InvoiceDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _invoiceRepository.WithDetailsAsync(i => i.Customer);

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var invoices = await AsyncExecuter.ToListAsync(
                queryable
                    .OrderByDescending(i => i.InvoiceDate)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
            );

            return new PagedResultDto<InvoiceDto>(
                totalCount,
                ObjectMapper.Map<List<Invoice>, List<InvoiceDto>>(invoices)
            );
        }

        /// <summary>
        /// Updates an existing invoice (only if not paid)
        /// </summary>
        public override async Task<InvoiceDto> UpdateAsync(Guid id, UpdateInvoiceDto input)
        {
            var invoice = await _invoiceRepository.GetAsync(id);

            if (!invoice.CanEdit())
            {
                throw new BusinessException("Invoice:CannotEditInvoice")
                    .WithData("InvoiceId", id)
                    .WithData("Status", invoice.Status);
            }

            invoice.Update(input.InvoiceDate, input.DueDate, input.TaxAmount);
            await _invoiceRepository.UpdateAsync(invoice);

            return await GetAsync(id);
        }

        /// <summary>
        /// Updates invoice status
        /// </summary>
        public async Task<InvoiceDto> UpdateStatusAsync(Guid id, UpdateInvoiceStatusDto input)
        {
            var invoice = await _invoiceRepository.GetAsync(id);
            invoice.UpdateStatus(input.Status);
            await _invoiceRepository.UpdateAsync(invoice);

            return await GetAsync(id);
        }

        /// <summary>
        /// Gets invoices for a specific customer
        /// </summary>
        public async Task<List<InvoiceDto>> GetByCustomerIdAsync(Guid customerId)
        {
            var queryable = await _invoiceRepository.WithDetailsAsync(i => i.LineItems, i => i.Customer);
            var invoices = await AsyncExecuter.ToListAsync(
                queryable.Where(i => i.CustomerId == customerId)
            );

            return ObjectMapper.Map<List<Invoice>, List<InvoiceDto>>(invoices);
        }

        /// <summary>
        /// Adds a line item to an invoice
        /// </summary>
        public async Task<InvoiceDto> AddLineItemAsync(Guid invoiceId, CreateLineItemDto input)
        {
            var invoice = await _invoiceRepository.GetAsync(invoiceId);

            var lineItem = new LineItem(
                GuidGenerator.Create(),
                invoiceId,
                input.Description,
                input.Quantity,
                input.UnitPrice
            );

            invoice.AddLineItem(lineItem);
            await _lineItemRepository.InsertAsync(lineItem);

            return await GetAsync(invoiceId);
        }

        /// <summary>
        /// Updates a line item
        /// </summary>
        public async Task<InvoiceDto> UpdateLineItemAsync(Guid invoiceId, Guid lineItemId, UpdateLineItemDto input)
        {
            var invoice = await _invoiceRepository.GetAsync(invoiceId);

            if (!invoice.CanEdit())
            {
                throw new BusinessException("Invoice:CannotEditInvoice")
                    .WithData("InvoiceId", invoiceId)
                    .WithData("Status", invoice.Status);
            }

            var lineItem = await _lineItemRepository.GetAsync(lineItemId);

            if (lineItem.InvoiceId != invoiceId)
            {
                throw new BusinessException("Invoice:LineItemDoesNotBelongToInvoice")
                    .WithData("LineItemId", lineItemId)
                    .WithData("InvoiceId", invoiceId);
            }

            lineItem.Update(input.Description, input.Quantity, input.UnitPrice);
            await _lineItemRepository.UpdateAsync(lineItem);

            return await GetAsync(invoiceId);
        }

        /// <summary>
        /// Removes a line item from an invoice
        /// </summary>
        public async Task<InvoiceDto> RemoveLineItemAsync(Guid invoiceId, Guid lineItemId)
        {
            var invoice = await _invoiceRepository.GetAsync(invoiceId);
            invoice.RemoveLineItem(lineItemId);

            await _lineItemRepository.DeleteAsync(lineItemId);

            return await GetAsync(invoiceId);
        }
    }
}
