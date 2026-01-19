using Microsoft.Extensions.Localization;
using CustomerInvoice.Localization;
using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace CustomerInvoice;

[Dependency(ReplaceServices = true)]
public class CustomerInvoiceBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<CustomerInvoiceResource> _localizer;

    public CustomerInvoiceBrandingProvider(IStringLocalizer<CustomerInvoiceResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
