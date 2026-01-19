using Volo.Abp.Modularity;

namespace CustomerInvoice;

/* Inherit from this class for your domain layer tests. */
public abstract class CustomerInvoiceDomainTestBase<TStartupModule> : CustomerInvoiceTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
