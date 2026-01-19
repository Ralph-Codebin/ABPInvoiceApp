using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerInvoice.Entities
{
    /// <summary>
    /// Represents a line item in an invoice
    /// </summary>
    public class LineItem : CreationAuditedEntity<Guid>
    {
        /// <summary>
        /// Foreign key to Invoice
        /// </summary>
        [Required]
        public Guid InvoiceId { get; set; }

        /// <summary>
        /// Description of the item/service
        /// </summary>
        [Required]
        [MaxLength(LineItemConsts.MaxDescriptionLength)]
        public string Description { get; set; }

        /// <summary>
        /// Quantity of the item
        /// </summary>
        [Required]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Unit price of the item
        /// </summary>
        [Required]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Calculated total (Quantity * UnitPrice)
        /// </summary>
        public decimal Total => Quantity * UnitPrice;

        /// <summary>
        /// Navigation property to Invoice
        /// </summary>
        public virtual Invoice Invoice { get; set; }

        /// <summary>
        /// Protected constructor for ORM
        /// </summary>
        protected LineItem()
        {
        }

        /// <summary>
        /// Creates a new line item
        /// </summary>
        public LineItem(
            Guid id,
            Guid invoiceId,
            string description,
            decimal quantity,
            decimal unitPrice)
            : base(id)
        {
            InvoiceId = invoiceId;
            SetDescription(description);
            SetQuantity(quantity);
            SetUnitPrice(unitPrice);
        }

        /// <summary>
        /// Sets the description with validation
        /// </summary>
        public void SetDescription(string description)
        {
            Description = Volo.Abp.Check.NotNullOrWhiteSpace(
                description,
                nameof(Description),
                LineItemConsts.MaxDescriptionLength
            );
        }

        /// <summary>
        /// Sets the quantity with validation
        /// </summary>
        public void SetQuantity(decimal quantity)
        {
            if (quantity <= 0)
            {
                throw new BusinessException("LineItem:QuantityMustBeGreaterThanZero")
                    .WithData("Quantity", quantity);
            }

            Quantity = quantity;
        }

        /// <summary>
        /// Sets the unit price with validation
        /// </summary>
        public void SetUnitPrice(decimal unitPrice)
        {
            if (unitPrice < 0)
            {
                throw new BusinessException("LineItem:UnitPriceCannotBeNegative")
                    .WithData("UnitPrice", unitPrice);
            }

            UnitPrice = unitPrice;
        }

        /// <summary>
        /// Updates the line item details
        /// </summary>
        public void Update(string description, decimal quantity, decimal unitPrice)
        {
            SetDescription(description);
            SetQuantity(quantity);
            SetUnitPrice(unitPrice);
        }
    }

    /// <summary>
    /// Constants for LineItem entity
    /// </summary>
    public static class LineItemConsts
    {
        public const int MaxDescriptionLength = 500;
    }
}
