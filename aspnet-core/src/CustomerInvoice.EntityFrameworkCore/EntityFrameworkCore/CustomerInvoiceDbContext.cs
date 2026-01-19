using CustomerInvoice.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace CustomerInvoice.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class CustomerInvoiceDbContext :
    AbpDbContext<CustomerInvoiceDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Domain Entities

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<LineItem> LineItems { get; set; }

    #endregion

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public CustomerInvoiceDbContext(DbContextOptions<CustomerInvoiceDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        // Customer entity configuration
        builder.Entity<Customer>(b =>
        {
            b.ToTable(CustomerInvoiceConsts.DbTablePrefix + "Customers", CustomerInvoiceConsts.DbSchema);
            b.ConfigureByConvention(); // Auto configure for base class props (audit fields, soft-delete, etc.)

            // Properties
            b.Property(x => x.Name).IsRequired().HasMaxLength(CustomerConsts.MaxNameLength);
            b.Property(x => x.Email).IsRequired().HasMaxLength(CustomerConsts.MaxEmailLength);
            b.Property(x => x.Phone).HasMaxLength(CustomerConsts.MaxPhoneLength);
            b.Property(x => x.BillingAddress).HasMaxLength(CustomerConsts.MaxBillingAddressLength);

            // Indexes
            b.HasIndex(x => x.Email); // Index for email lookups and uniqueness enforcement
            b.HasIndex(x => x.IsDeleted); // Index for soft-delete queries

            // Relationships
            b.HasMany(x => x.Invoices)
                .WithOne(x => x.Customer)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting customer with invoices
        });

        // Invoice entity configuration
        builder.Entity<Invoice>(b =>
        {
            b.ToTable(CustomerInvoiceConsts.DbTablePrefix + "Invoices", CustomerInvoiceConsts.DbSchema);
            b.ConfigureByConvention(); // Auto configure for base class props

            // Properties
            b.Property(x => x.InvoiceNumber).IsRequired().HasMaxLength(InvoiceConsts.MaxInvoiceNumberLength);
            b.Property(x => x.InvoiceDate).IsRequired();
            b.Property(x => x.Status).IsRequired();
            b.Property(x => x.TaxAmount).HasColumnType("decimal(18,2)");

            // Indexes
            b.HasIndex(x => x.InvoiceNumber).IsUnique(); // Unique index for invoice number
            b.HasIndex(x => x.CustomerId); // Index for customer lookups
            b.HasIndex(x => x.Status); // Index for filtering by status
            b.HasIndex(x => x.InvoiceDate); // Index for date range queries
            b.HasIndex(x => x.IsDeleted); // Index for soft-delete queries

            // Relationships
            b.HasMany(x => x.LineItems)
                .WithOne(x => x.Invoice)
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade); // Delete line items when invoice is deleted
        });

        // LineItem entity configuration
        builder.Entity<LineItem>(b =>
        {
            b.ToTable(CustomerInvoiceConsts.DbTablePrefix + "LineItems", CustomerInvoiceConsts.DbSchema);
            b.ConfigureByConvention(); // Auto configure for base class props

            // Properties
            b.Property(x => x.Description).IsRequired().HasMaxLength(LineItemConsts.MaxDescriptionLength);
            b.Property(x => x.Quantity).IsRequired().HasColumnType("decimal(18,2)");
            b.Property(x => x.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");

            // Indexes
            b.HasIndex(x => x.InvoiceId); // Index for invoice lookups

            // Ignore calculated property (not stored in database)
            b.Ignore(x => x.Total);
        });
    }
}
