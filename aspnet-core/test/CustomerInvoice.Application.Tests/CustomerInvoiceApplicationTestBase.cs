using Volo.Abp.Modularity;

namespace CustomerInvoice;

public abstract class CustomerInvoiceApplicationTestBase<TStartupModule> : CustomerInvoiceTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
