using System.ComponentModel.DataAnnotations;

namespace CustomerInvoice.Invoices
{
    /// <summary>
    /// DTO for creating a new line item
    /// </summary>
    public class CreateLineItemDto
    {
        /// <summary>
        /// Line item description
        /// </summary>
        [Required]
        [StringLength(LineItemConsts.MaxDescriptionLength)]
        public string Description { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Unit price
        /// </summary>
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Unit price must be greater than or equal to 0")]
        public decimal UnitPrice { get; set; }
    }
}
