using Volo.Abp.Modularity;

namespace CustomerInvoice;

[DependsOn(
    typeof(CustomerInvoiceApplicationModule),
    typeof(CustomerInvoiceDomainTestModule)
)]
public class CustomerInvoiceApplicationTestModule : AbpModule
{

}
