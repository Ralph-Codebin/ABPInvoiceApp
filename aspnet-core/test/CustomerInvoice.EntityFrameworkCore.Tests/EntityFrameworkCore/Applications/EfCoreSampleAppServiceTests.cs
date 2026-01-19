using CustomerInvoice.Samples;
using Xunit;

namespace CustomerInvoice.EntityFrameworkCore.Applications;

[Collection(CustomerInvoiceTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<CustomerInvoiceEntityFrameworkCoreTestModule>
{

}
