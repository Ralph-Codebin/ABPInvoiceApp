using System;
using Volo.Abp.Application.Dtos;

namespace CustomerInvoice.Invoices
{
    /// <summary>
    /// DTO for LineItem entity - used for reading line item data
    /// </summary>
    public class LineItemDto : CreationAuditedEntityDto<Guid>
    {
        /// <summary>
        /// Invoice ID this line item belongs to
        /// </summary>
        public Guid InvoiceId { get; set; }

        /// <summary>
        /// Line item description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Unit price
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Calculated total (Quantity × UnitPrice)
        /// </summary>
        public decimal Total { get; set; }
    }
}
