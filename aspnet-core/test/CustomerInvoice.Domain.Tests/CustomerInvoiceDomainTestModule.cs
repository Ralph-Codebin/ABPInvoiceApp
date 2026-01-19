using Volo.Abp.Modularity;

namespace CustomerInvoice;

[DependsOn(
    typeof(CustomerInvoiceDomainModule),
    typeof(CustomerInvoiceTestBaseModule)
)]
public class CustomerInvoiceDomainTestModule : AbpModule
{

}
