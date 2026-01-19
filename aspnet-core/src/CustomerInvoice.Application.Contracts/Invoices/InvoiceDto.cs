using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace CustomerInvoice.Invoices
{
    /// <summary>
    /// DTO for Invoice entity - used for reading invoice data
    /// </summary>
    public class InvoiceDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// Unique invoice number
        /// </summary>
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Customer ID this invoice belongs to
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Customer name (denormalized for display)
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Invoice date
        /// </summary>
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// Due date (optional)
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Invoice status
        /// </summary>
        public InvoiceStatus Status { get; set; }

        /// <summary>
        /// Tax amount
        /// </summary>
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// Calculated subtotal (sum of all line items)
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Calculated grand total (SubTotal + TaxAmount)
        /// </summary>
        public decimal GrandTotal { get; set; }

        /// <summary>
        /// Line items associated with this invoice
        /// </summary>
        public List<LineItemDto> LineItems { get; set; }

        public InvoiceDto()
        {
            LineItems = new List<LineItemDto>();
        }
    }
}
