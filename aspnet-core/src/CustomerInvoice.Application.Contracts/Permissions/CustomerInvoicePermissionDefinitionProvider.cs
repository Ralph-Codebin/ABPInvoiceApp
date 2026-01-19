using CustomerInvoice.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace CustomerInvoice.Permissions;

public class CustomerInvoicePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CustomerInvoicePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(CustomerInvoicePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CustomerInvoiceResource>(name);
    }
}
