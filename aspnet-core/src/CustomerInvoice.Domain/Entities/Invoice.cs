using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerInvoice.Entities
{
    /// <summary>
    /// Represents an invoice in the system
    /// </summary>
    public class Invoice : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// Unique invoice number (auto-generated, sequential)
        /// </summary>
        [Required]
        [MaxLength(InvoiceConsts.MaxInvoiceNumberLength)]
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Foreign key to Customer
        /// </summary>
        [Required]
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Date the invoice was created
        /// </summary>
        [Required]
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// Due date for payment (optional)
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Current status of the invoice
        /// </summary>
        [Required]
        public InvoiceStatus Status { get; set; }

        /// <summary>
        /// Tax amount (optional)
        /// </summary>
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// Navigation property to Customer
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Collection of line items in this invoice
        /// </summary>
        public virtual ICollection<LineItem> LineItems { get; set; }

        /// <summary>
        /// Calculated subtotal (sum of all line item totals)
        /// </summary>
        public decimal SubTotal => LineItems?.Sum(li => li.Total) ?? 0;

        /// <summary>
        /// Calculated grand total (subtotal + tax)
        /// </summary>
        public decimal GrandTotal => SubTotal + TaxAmount;

        /// <summary>
        /// Protected constructor for ORM
        /// </summary>
        protected Invoice()
        {
            LineItems = new List<LineItem>();
        }

        /// <summary>
        /// Creates a new invoice
        /// </summary>
        public Invoice(
            Guid id,
            string invoiceNumber,
            Guid customerId,
            DateTime invoiceDate,
            DateTime? dueDate = null,
            InvoiceStatus status = InvoiceStatus.Draft)
            : base(id)
        {
            SetInvoiceNumber(invoiceNumber);
            CustomerId = customerId;
            SetInvoiceDate(invoiceDate);
            SetDueDate(dueDate);
            Status = status;
            TaxAmount = 0;
            LineItems = new List<LineItem>();
        }

        /// <summary>
        /// Sets the invoice number with validation
        /// </summary>
        public void SetInvoiceNumber(string invoiceNumber)
        {
            InvoiceNumber = Volo.Abp.Check.NotNullOrWhiteSpace(
                invoiceNumber,
                nameof(InvoiceNumber),
                InvoiceConsts.MaxInvoiceNumberLength
            );
        }

        /// <summary>
        /// Sets the invoice date with validation
        /// </summary>
        public void SetInvoiceDate(DateTime invoiceDate)
        {
            // Invoice date cannot be in the future
            if (invoiceDate.Date > DateTime.Now.Date)
            {
                throw new BusinessException("Invoice:DateCannotBeInFuture")
                    .WithData("InvoiceDate", invoiceDate);
            }

            InvoiceDate = invoiceDate;
        }

        /// <summary>
        /// Sets the due date with validation
        /// </summary>
        public void SetDueDate(DateTime? dueDate)
        {
            // Due date must be after invoice date
            if (dueDate.HasValue && dueDate.Value.Date < InvoiceDate.Date)
            {
                throw new BusinessException("Invoice:DueDateMustBeAfterInvoiceDate")
                    .WithData("InvoiceDate", InvoiceDate)
                    .WithData("DueDate", dueDate.Value);
            }

            DueDate = dueDate;
        }

        /// <summary>
        /// Updates the invoice status with validation
        /// </summary>
        public void UpdateStatus(InvoiceStatus newStatus)
        {
            // Cannot change status if already cancelled
            if (Status == InvoiceStatus.Cancelled && newStatus != InvoiceStatus.Cancelled)
            {
                throw new BusinessException("Invoice:CannotChangeStatusOfCancelledInvoice");
            }

            Status = newStatus;
        }

        /// <summary>
        /// Adds a line item to the invoice
        /// </summary>
        public void AddLineItem(LineItem lineItem)
        {
            // Cannot modify paid invoices
            if (Status == InvoiceStatus.Paid)
            {
                throw new BusinessException("Invoice:CannotModifyPaidInvoice");
            }

            LineItems.Add(lineItem);
        }

        /// <summary>
        /// Removes a line item from the invoice
        /// </summary>
        public void RemoveLineItem(Guid lineItemId)
        {
            // Cannot modify paid invoices
            if (Status == InvoiceStatus.Paid)
            {
                throw new BusinessException("Invoice:CannotModifyPaidInvoice");
            }

            var lineItem = LineItems.FirstOrDefault(li => li.Id == lineItemId);
            if (lineItem != null)
            {
                LineItems.Remove(lineItem);
            }
        }

        /// <summary>
        /// Updates invoice details
        /// </summary>
        public void Update(DateTime invoiceDate, DateTime? dueDate, decimal taxAmount)
        {
            // Cannot edit paid invoices
            if (Status == InvoiceStatus.Paid)
            {
                throw new BusinessException("Invoice:CannotModifyPaidInvoice");
            }

            SetInvoiceDate(invoiceDate);
            SetDueDate(dueDate);
            TaxAmount = taxAmount;
        }

        /// <summary>
        /// Checks if the invoice can be edited
        /// </summary>
        public bool CanEdit()
        {
            return Status != InvoiceStatus.Paid && Status != InvoiceStatus.Cancelled;
        }
    }
}
