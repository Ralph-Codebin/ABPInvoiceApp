using CustomerInvoice.Samples;
using Xunit;

namespace CustomerInvoice.EntityFrameworkCore.Domains;

[Collection(CustomerInvoiceTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<CustomerInvoiceEntityFrameworkCoreTestModule>
{

}
