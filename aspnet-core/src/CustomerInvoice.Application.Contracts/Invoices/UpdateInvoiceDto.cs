using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerInvoice.Invoices
{
    /// <summary>
    /// DTO for updating an existing invoice
    /// Note: Line items are managed separately through dedicated endpoints
    /// </summary>
    public class UpdateInvoiceDto
    {
        /// <summary>
        /// Invoice date
        /// </summary>
        [Required]
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// Due date (optional)
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Tax amount
        /// </summary>
        [Range(0, double.MaxValue)]
        public decimal TaxAmount { get; set; }
    }
}
