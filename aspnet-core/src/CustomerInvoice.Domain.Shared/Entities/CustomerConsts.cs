namespace CustomerInvoice
{
    /// <summary>
    /// Constants for Customer entity validation
    /// </summary>
    public static class CustomerConsts
    {
        /// <summary>
        /// Maximum length for customer name
        /// </summary>
        public const int MaxNameLength = 200;

        /// <summary>
        /// Maximum length for customer email
        /// </summary>
        public const int MaxEmailLength = 256;

        /// <summary>
        /// Maximum length for customer phone
        /// </summary>
        public const int MaxPhoneLength = 50;

        /// <summary>
        /// Maximum length for billing address
        /// </summary>
        public const int MaxBillingAddressLength = 500;
    }
}
