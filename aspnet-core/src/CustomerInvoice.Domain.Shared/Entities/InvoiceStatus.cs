namespace CustomerInvoice
{
    /// <summary>
    /// Represents the status of an invoice
    /// </summary>
    public enum InvoiceStatus
    {
        /// <summary>
        /// Invoice is in draft state, not yet finalized
        /// </summary>
        Draft = 0,

        /// <summary>
        /// Invoice is pending approval or processing
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Invoice has been sent to the customer
        /// </summary>
        Sent = 2,

        /// <summary>
        /// Invoice has been paid
        /// </summary>
        Paid = 3,

        /// <summary>
        /// Invoice has been cancelled
        /// </summary>
        Cancelled = 4
    }
}
