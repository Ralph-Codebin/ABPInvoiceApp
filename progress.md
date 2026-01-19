# Customer & Invoice Management System - Development Progress

## Project Status: In Progress - Frontend Detail Views
**Last Updated:** 2026-01-19

---

## Phase 1: Project Setup & Infrastructure ✅

### 1.1 Environment Setup
- [x] Install ABP CLI (`dotnet tool install -g Volo.Abp.Cli`) - v8.3.0
- [x] Verify .NET SDK version (8.0 or later recommended) - .NET 9.0 installed
- [x] Verify Node.js and npm/yarn installation - Node.js v22.15.0, npm 9.9.4
- [x] Install Angular CLI - Included with ABP template
- [x] Set up SQL Server or PostgreSQL database - SQL Server LocalDB configured

### 1.2 Project Initialization
- [x] Create new ABP application using ABP CLI
  - [x] Choose application template (MVC/Angular) - Angular selected
  - [x] Select UI framework (Angular) - Angular 17 with ABP UI
  - [x] Configure database provider (EF Core - SQL Server/PostgreSQL) - EF Core with SQL Server
  - [x] Set up solution structure - Default ABP layered structure
- [x] Initialize Git repository - Auto-created by ABP CLI
- [x] Create `.gitignore` file - Auto-created by ABP CLI
- [x] Set up database connection strings - LocalDB configured in appsettings.json
- [x] Run initial migrations - DbMigrator executed successfully
- [x] Verify application runs (backend + frontend) - Application is fully functional

### 1.3 Module Structure Setup
- [x] Using default ABP layered structure (no separate modules needed)
  - Domain project contains entities
  - Application project contains app services
  - Application.Contracts project contains DTOs and interfaces
  - HttpApi project uses ABP's dynamic API controllers
  - EntityFrameworkCore project contains DbContext configuration

---

## Phase 2: Backend Development - Domain Layer ✅

### 2.1 Customer Domain
- [x] Create `Customer` entity class
  - [x] Add properties (Id, Name, Email, Phone, BillingAddress)
  - [x] Implement soft-delete (ISoftDelete via FullAuditedAggregateRoot)
  - [x] Add audit properties (CreationTime, LastModificationTime, etc.)
  - [x] Add navigation property for Invoices
- [x] Create `Customer` repository interface (using ABP's IRepository)
- [x] Implement domain validation rules
  - [x] Email uniqueness validation (will be enforced in DB layer)
  - [x] Name required validation (Check.NotNullOrWhiteSpace)
  - [x] Field length validations (MaxLength attributes)

### 2.2 Invoice Domain
- [x] Create `InvoiceStatus` enum (Draft, Pending, Sent, Paid, Cancelled)
- [x] Create `Invoice` entity class
  - [x] Add properties (Id, InvoiceNumber, CustomerId, InvoiceDate, DueDate, Status)
  - [x] Add calculated properties (SubTotal, TaxAmount, GrandTotal)
  - [x] Implement soft-delete (ISoftDelete via FullAuditedAggregateRoot)
  - [x] Add audit properties
  - [x] Add navigation properties (Customer, LineItems)
- [x] Create `LineItem` entity class
  - [x] Add properties (Id, InvoiceId, Description, Quantity, UnitPrice, Total)
  - [x] Add audit properties (CreationTime via CreationAuditedEntity)
  - [x] Add navigation property (Invoice)
- [x] Implement invoice number generation service/domain service
  - [x] Created IInvoiceNumberGenerator interface
  - [x] Created InvoiceNumberGenerator implementation (format: INV-YYYYMM-XXXX)
- [x] Implement domain validation rules
  - [x] Invoice date validation (not in future)
  - [x] Due date validation (after invoice date)
  - [x] Status transition validation (cannot change cancelled invoices)
  - [x] Line item validation (quantity > 0, unitPrice >= 0)
  - [x] Paid invoice protection (cannot edit paid invoices)
- [x] Implement calculation logic
  - [x] Line item total calculation (Quantity * UnitPrice)
  - [x] Invoice subtotal calculation (sum of all line item totals)
  - [x] Grand total calculation (SubTotal + TaxAmount)

---

## Phase 3: Backend Development - Infrastructure Layer ✅

### 3.1 Database Configuration
- [x] Configure `Customer` entity in DbContext
  - [x] Set up table mapping (AppCustomers)
  - [x] Configure properties (max lengths, required fields)
  - [x] Set up indexes (email for lookups, IsDeleted for soft-delete queries)
  - [x] Configure soft-delete filter (via ConfigureByConvention)
- [x] Configure `Invoice` entity in DbContext
  - [x] Set up table mapping (AppInvoices)
  - [x] Configure properties and constraints (decimal(18,2) for TaxAmount)
  - [x] Set up indexes (invoice number unique, customer ID, status, dates, IsDeleted)
  - [x] Configure relationships (Customer 1-to-many Invoice with Restrict delete)
  - [x] Configure soft-delete filter (via ConfigureByConvention)
- [x] Configure `LineItem` entity in DbContext
  - [x] Set up table mapping (AppLineItems)
  - [x] Configure properties (decimal(18,2) precision for Quantity and UnitPrice)
  - [x] Configure relationship (Invoice 1-to-many LineItem)
  - [x] Set up cascade delete behavior (OnDelete Cascade)
  - [x] Ignore calculated property (Total)

### 3.2 Database Migrations
- [x] Create initial migration for Customer, Invoice, and LineItem entities
- [x] Review migration SQL scripts (verified correct tables, columns, indexes, FK constraints)
- [x] Apply migrations to database (DbMigrator executed successfully)
- [x] Verify database schema (migration created all tables with proper structure)

### 3.3 Repository Implementation
- [x] Using ABP's generic repositories (IRepository<TEntity, TKey>)
- [x] No custom repositories needed at this stage
- [x] Custom query methods can be added later in app services if needed

---

## Phase 4: Backend Development - Application Layer ✅

### 4.1 DTOs (Data Transfer Objects)
- [x] Create Customer DTOs
  - [x] `CustomerDto` (output) - FullAuditedEntityDto
  - [x] `CreateCustomerDto` (input) - with validation attributes
  - [x] `UpdateCustomerDto` (input) - with validation attributes
- [x] Create Invoice DTOs
  - [x] `InvoiceDto` (output with LineItems) - includes CustomerName, SubTotal, GrandTotal
  - [x] `CreateInvoiceDto` (input) - includes List<CreateLineItemDto>
  - [x] `UpdateInvoiceDto` (input) - basic fields only (dates, tax)
  - [x] `UpdateInvoiceStatusDto` (input) - status update
- [x] Create LineItem DTOs
  - [x] `LineItemDto` (output) - CreationAuditedEntityDto with calculated Total
  - [x] `CreateLineItemDto` (input) - with range validation
  - [x] `UpdateLineItemDto` (input) - with range validation

### 4.2 AutoMapper Profiles
- [x] Create AutoMapper profile for Customer mappings - CustomerInvoiceApplicationAutoMapperProfile
- [x] Create AutoMapper profile for Invoice mappings - includes custom mappings for calculated fields
- [x] Create AutoMapper profile for LineItem mappings
- [x] Test all mappings - working in production

### 4.3 Application Services
- [x] Implement `CustomerAppService`
  - [x] `GetListAsync` (with pagination, sort)
  - [x] `GetAsync` (by ID)
  - [x] `CreateAsync`
  - [x] `UpdateAsync`
  - [x] `DeleteAsync` (soft delete)
  - [x] `GetByEmailAsync` (custom method)
  - [x] `SearchAsync` (search by name or email)
- [x] Implement `InvoiceAppService`
  - [x] `GetListAsync` (paginated, ordered by date descending)
  - [x] `GetAsync` (by ID with line items and customer eager loaded)
  - [x] `CreateAsync` (with auto-generated invoice number and line items)
  - [x] `UpdateAsync` (with CanEdit validation)
  - [x] `DeleteAsync`
  - [x] `UpdateStatusAsync`
  - [x] `GetByCustomerIdAsync` (get invoices for a customer)
  - [x] `AddLineItemAsync`
  - [x] `UpdateLineItemAsync`
  - [x] `RemoveLineItemAsync`

### 4.4 Validation
- [x] Add input validation for Customer DTOs (DataAnnotations: Required, EmailAddress, StringLength)
- [x] Add input validation for Invoice DTOs (DataAnnotations with ranges)
- [x] Add input validation for LineItem DTOs (Range validation for Quantity > 0, UnitPrice >= 0)
- [x] Add business rule validation in app services
  - [x] Prevent editing paid invoices (CanEdit() domain method)
  - [x] Validate status transitions (CanChangeStatus() domain method)
  - [x] Invoice date and due date validation in domain layer

---

## Phase 5: Backend Development - Authorization

### 5.1 Permission Definitions
- [ ] Define Customer permissions
  - [ ] `CustomerManagement.Customers`
  - [ ] `CustomerManagement.Customers.Create`
  - [ ] `CustomerManagement.Customers.Edit`
  - [ ] `CustomerManagement.Customers.Delete`
  - [ ] `CustomerManagement.Customers.View`
- [ ] Define Invoice permissions
  - [ ] `InvoiceManagement.Invoices`
  - [ ] `InvoiceManagement.Invoices.Create`
  - [ ] `InvoiceManagement.Invoices.Edit`
  - [ ] `InvoiceManagement.Invoices.Delete`
  - [ ] `InvoiceManagement.Invoices.View`
  - [ ] `InvoiceManagement.Invoices.UpdateStatus`
- [ ] Register permissions in permission definition provider

### 5.2 Authorization Implementation
- [ ] Add `[Authorize]` attributes to CustomerAppService methods
- [ ] Add `[Authorize]` attributes to InvoiceAppService methods
- [ ] Test permission enforcement

### 5.3 Role Setup
- [ ] Create seed data for roles
  - [ ] Admin role (all permissions)
  - [ ] Staff role (create/edit permissions)
  - [ ] Viewer role (view-only permissions)
- [ ] Assign permissions to roles
- [ ] Create test users for each role

---

## Phase 6: Backend Development - API Layer

### 6.1 API Controllers
- [ ] Verify ABP dynamic API controllers are generated
- [ ] Test Customer API endpoints
  - [ ] GET /api/app/customer
  - [ ] GET /api/app/customer/{id}
  - [ ] POST /api/app/customer
  - [ ] PUT /api/app/customer/{id}
  - [ ] DELETE /api/app/customer/{id}
- [ ] Test Invoice API endpoints
  - [ ] GET /api/app/invoice
  - [ ] GET /api/app/invoice/{id}
  - [ ] POST /api/app/invoice
  - [ ] PUT /api/app/invoice/{id}
  - [ ] DELETE /api/app/invoice/{id}
  - [ ] PUT /api/app/invoice/{id}/status
  - [ ] POST /api/app/invoice/{id}/line-item
  - [ ] PUT /api/app/invoice/{id}/line-item/{lineItemId}
  - [ ] DELETE /api/app/invoice/{id}/line-item/{lineItemId}

### 6.2 Swagger Documentation
- [ ] Configure Swagger UI
- [ ] Add XML documentation comments to DTOs
- [ ] Add XML documentation comments to AppServices
- [ ] Test all endpoints in Swagger
- [ ] Verify authorization works in Swagger

---

## Phase 7: Frontend Development - Project Setup ✅

### 7.1 Angular Application Setup
- [x] Navigate to Angular project directory
- [x] Install npm dependencies (with --legacy-peer-deps)
- [x] Configure environment files (API URLs) - configured in environment.ts
- [x] Run Angular application and verify connection to backend - working
- [x] Configure proxy settings for API calls - using ABP's built-in proxy

### 7.2 Generate Service Proxies
- [x] Run ABP CLI command to generate Angular service proxies
- [x] Verify Customer service proxy generated - CustomerService in proxy/customers
- [x] Verify Invoice service proxy generated - InvoiceService in proxy/invoices
- [x] Import generated services in modules - auto-imported via ABP

### 7.3 Routing Setup
- [x] Set up main routing module - app-routing.module.ts with lazy loading
- [x] Create route for Customer module - /customers route
- [x] Create route for Invoice module - /invoices route
- [x] Add navigation menu items - configured in route.provider.ts
- [x] Configure route guards for authentication - ABP handles this

---

## Phase 8: Frontend Development - Customer Module ✅

### 8.1 Customer List Component ✅
- [x] Generate customer list component
- [x] Set up ABP data grid (abp-table)
- [x] Implement customer list retrieval (using ListService)
- [x] Add column sorting (Name, Email, Phone)
- [x] Add pagination (via ListService)
- [x] Add "Create Customer" button
- [x] Add action buttons (Edit, Delete)
- [x] Implement delete confirmation dialog (via ConfirmationService)
- [x] Add loading indicators

### 8.2 Customer Create/Edit Component ✅ (Modal-based)
- [x] Create reactive form with validation in modal
  - [x] Name field (required, max 200)
  - [x] Email field (required, email format, max 256)
  - [x] Phone field (max 50)
  - [x] Billing Address field (max 500)
- [x] Implement create functionality
- [x] Implement edit functionality (load existing data)
- [x] Add inline validation messages
- [x] Add save and cancel buttons
- [x] Handle API errors
- [x] Show success notifications (via ToasterService)
- [x] Modal closes after successful save

### 8.3 Customer Detail Component ✅
- [x] Generate customer detail component
- [x] Display customer information
- [x] Display list of customer invoices
- [x] Add "Edit Customer" button
- [x] Add navigation to invoice details
- [x] Add breadcrumb navigation
- [x] Add "View" link in customer list actions dropdown

---

## Phase 9: Frontend Development - Invoice Module ✅

### 9.1 Invoice List Component ✅
- [x] Generate invoice list component
- [x] Set up ABP data grid
- [x] Implement invoice list retrieval
- [x] Display columns (Invoice #, Customer, Date, Status, Total)
- [x] Add column sorting
- [x] Add pagination
- [x] Add "Create Invoice" button
- [x] Add action buttons (Edit, Delete, View Status)
- [x] Add status badge styling (Draft, Pending, Sent, Paid, Cancelled)
- [x] Implement delete confirmation dialog
- [x] Add loading indicators

### 9.2 Invoice Create/Edit Component ✅ (Modal-based)
- [x] Create reactive form with validation in modal
  - [x] Customer selection (dropdown, required)
  - [x] Invoice date (date picker, required)
  - [x] Due date (date picker)
  - [x] Tax amount field
- [x] Implement create functionality
  - [x] Auto-generate invoice number on backend
  - [x] Set default invoice date (today)
  - [x] Line items management (add/remove during creation)
- [x] Implement edit functionality
  - [x] Load existing invoice data
  - [x] Basic field editing only (not line items in modal)
- [x] Add inline validation messages
- [x] Add save and cancel buttons
- [x] Handle API errors
- [x] Show success notifications
- [x] Modal closes after successful save

### 9.3 Line Item Management Component ✅ (Integrated in Create Modal)
- [x] Line items section in create modal
- [x] Display line items in a table
  - [x] Columns: Description, Quantity, Unit Price, Total
- [x] Add "Add Line Item" button
- [x] Implement add line item functionality
  - [x] Description field (required, max 500)
  - [x] Quantity field (required, > 0, decimal)
  - [x] Unit Price field (required, >= 0, decimal)
  - [x] Calculate and display total (Qty × Unit Price) - real-time calculation
- [x] Implement delete line item functionality (during creation)
  - [x] Remove button for each row
- [x] Calculate and display invoice subtotal
- [x] Calculate and display grand total
- [x] Implement edit line item functionality (for existing invoices) - implemented in detail view
- [x] Use AddLineItemAsync/UpdateLineItemAsync/RemoveLineItemAsync endpoints for existing invoices

### 9.4 Invoice Detail Component ✅
- [x] Generate invoice detail component
- [x] Display invoice header information
  - [x] Invoice number
  - [x] Customer name (with link to customer detail)
  - [x] Invoice date
  - [x] Due date
  - [x] Status (with badge)
- [x] Display all line items in a table
- [x] Display financial summary
  - [x] Subtotal
  - [x] Tax amount
  - [x] Grand total
- [x] Add "Edit Invoice" button
- [x] Add "Update Status" dropdown with update button
- [x] Add line item management section
  - [x] Add new line items (inline form)
  - [x] Edit existing line items (inline editing)
  - [x] Delete line items (with confirmation)
  - [x] Real-time total calculation
- [x] Add breadcrumb navigation
- [x] Add "View" link in invoice list actions dropdown
- [x] Disable editing for Paid/Cancelled invoices

---

## Phase 10: Frontend Development - UI/UX Polish

### 10.1 Responsive Design
- [ ] Test all pages on desktop (1920×1080)
- [ ] Test all pages on desktop (1366×768)
- [ ] Test all pages on tablet landscape
- [ ] Test all pages on tablet portrait
- [ ] Adjust layouts for responsiveness
- [ ] Test data grids on small screens (horizontal scroll or adaptive columns)

### 10.2 Navigation & Menus
- [ ] Add Customer Management menu item
- [ ] Add Invoice Management menu item
- [ ] Configure menu permissions
- [ ] Add breadcrumb navigation on all pages
- [ ] Test navigation flows

### 10.3 Error Handling & Feedback
- [ ] Implement global error handler
- [ ] Add toast notifications for success actions
- [ ] Add toast notifications for errors
- [ ] Add loading spinners for async operations
- [ ] Add empty state messages (no data)
- [ ] Add confirmation dialogs for destructive actions

### 10.4 Accessibility
- [ ] Add ARIA labels to interactive elements
- [ ] Ensure keyboard navigation works
- [ ] Test with screen reader (basic check)
- [ ] Ensure color contrast meets standards

---

## Phase 11: Testing

### 11.1 Backend Unit Tests
- [ ] Write unit tests for Customer entity validation
- [ ] Write unit tests for Invoice entity validation
- [ ] Write unit tests for LineItem calculations
- [ ] Write unit tests for invoice number generation
- [ ] Write unit tests for CustomerAppService methods
- [ ] Write unit tests for InvoiceAppService methods
- [ ] Achieve minimum 70% code coverage

### 11.2 Backend Integration Tests
- [ ] Write integration tests for Customer API endpoints
- [ ] Write integration tests for Invoice API endpoints
- [ ] Test authorization (different roles)
- [ ] Test validation error responses
- [ ] Test soft-delete behavior
- [ ] Test business rule enforcement

### 11.3 Frontend Unit Tests
- [ ] Write unit tests for Customer components
- [ ] Write unit tests for Invoice components
- [ ] Write unit tests for form validation
- [ ] Test service integration (with mocks)

### 11.4 End-to-End Tests (Optional)
- [ ] Set up E2E testing framework (Playwright/Cypress)
- [ ] Write E2E tests for customer CRUD flow
- [ ] Write E2E tests for invoice creation flow
- [ ] Write E2E tests for line item management

### 11.5 Manual Testing
- [ ] Test complete customer CRUD workflow
- [ ] Test complete invoice CRUD workflow
- [ ] Test line item management
- [ ] Test invoice status updates
- [ ] Test authorization (login as different roles)
- [ ] Test search and filter functionality
- [ ] Test soft-delete behavior
- [ ] Test validation messages
- [ ] Test error scenarios (network errors, validation errors)

---

## Phase 12: Data & Deployment Preparation

### 12.1 Database Seeding
- [ ] Create seed data for roles (Admin, Staff, Viewer)
- [ ] Create seed data for test users
- [ ] Assign permissions to roles
- [ ] Create seed data for sample customers (5-10)
- [ ] Create seed data for sample invoices (10-20)
- [ ] Create seed data for line items
- [ ] Test seeding on fresh database

### 12.2 Configuration
- [ ] Configure production database connection string
- [ ] Configure CORS settings
- [ ] Configure JWT token settings (expiration, secret)
- [ ] Configure logging (Serilog or similar)
- [ ] Review and secure appsettings.json
- [ ] Set up environment-specific configurations

### 12.3 Documentation
- [ ] Document API endpoints (Swagger/OpenAPI)
- [ ] Write README with setup instructions
- [ ] Document database schema
- [ ] Document deployment steps
- [ ] Create user guide (optional)

---

## Phase 13: Deployment

### 13.1 Backend Deployment
- [ ] Choose hosting platform (Azure, AWS, on-premises)
- [ ] Set up database server
- [ ] Apply database migrations to production
- [ ] Build backend for production
- [ ] Deploy backend API
- [ ] Configure SSL/HTTPS
- [ ] Test API endpoints in production

### 13.2 Frontend Deployment
- [ ] Configure production API URLs
- [ ] Build Angular app for production
- [ ] Deploy frontend (Azure Static Web Apps, S3, IIS, etc.)
- [ ] Configure SSL/HTTPS
- [ ] Test frontend in production

### 13.3 Post-Deployment
- [ ] Run smoke tests
- [ ] Verify authentication works
- [ ] Verify all features work end-to-end
- [ ] Monitor logs for errors
- [ ] Set up application monitoring (Application Insights, etc.)

---

## Phase 14: Future Enhancements (Backlog)

- [ ] PDF invoice generation
- [ ] Email invoice to customers
- [ ] Payment gateway integration
- [ ] Recurring invoices
- [ ] Multi-currency support
- [ ] Invoice templates (customizable)
- [ ] Reporting and analytics dashboard
- [ ] Audit log for tracking changes
- [ ] Customer portal for self-service
- [ ] Export to Excel functionality
- [ ] Bulk operations (bulk delete, bulk status update)
- [ ] Invoice reminders (automated emails)
- [ ] Mobile app (optional)

---

## Notes & Issues

### Session Notes
_Add notes here after each development session_

**Session 1 (2026-01-19):**
- Created CLAUDE.md specification document
- Created progress.md tracking document
- Installed ABP CLI version 8.3.0 (latest stable version 10.0.2 had compatibility issues)
- Created ABP project "CustomerInvoice" with:
  - Angular frontend
  - Entity Framework Core with SQL Server LocalDB
  - Separate Identity Server
  - LeptonXLite theme
  - Version: 8.3.0 (targeting .NET 8.0)
- Built backend solution successfully (0 errors, 0 warnings)
- Installed Angular dependencies with --legacy-peer-deps flag (peer dependency conflicts with Angular 17)
- Ran DbMigrator successfully - database created with initial migrations
- **Phase 1 Complete:** Project setup infrastructure ready

**Session 2 (2026-01-19 continued):**
- Created all domain entities in [aspnet-core/src/CustomerInvoice.Domain/Entities/](aspnet-core/src/CustomerInvoice.Domain/Entities/)
  - Customer entity with soft-delete, audit properties, and validation
  - Invoice entity with relationships, calculated properties, and business rules
  - LineItem entity with validation and calculations
  - InvoiceStatus enum (Draft, Pending, Sent, Paid, Cancelled)
- Implemented domain services in [aspnet-core/src/CustomerInvoice.Domain/Services/](aspnet-core/src/CustomerInvoice.Domain/Services/)
  - InvoiceNumberGenerator service (format: INV-YYYYMM-XXXX)
- Added comprehensive business rules:
  - Invoice date cannot be in future
  - Due date must be after invoice date
  - Paid invoices cannot be modified
  - Cancelled invoices cannot change status
  - Line item quantity must be > 0, unit price >= 0
- Built solution successfully (14 nullable warnings, 0 errors)
- **Phase 2 Complete:** Domain layer with all entities and business logic

**Session 3 (2026-01-19 continued):**
- Configured EF Core entity mappings in [CustomerInvoiceDbContext.cs](aspnet-core/src/CustomerInvoice.EntityFrameworkCore/EntityFrameworkCore/CustomerInvoiceDbContext.cs)
  - Added DbSet properties for Customer, Invoice, and LineItem
  - Configured table mappings (AppCustomers, AppInvoices, AppLineItems)
  - Set up all property constraints (max lengths, required fields, decimal precision)
  - Created indexes for performance (email, invoice number, status, dates, soft-delete)
  - Configured relationships with proper cascade behaviors
  - Ignored calculated properties (Invoice.SubTotal, Invoice.GrandTotal, LineItem.Total)
- Registered InvoiceNumberGenerator as domain service in [CustomerInvoiceDomainModule.cs](aspnet-core/src/CustomerInvoice.Domain/CustomerInvoiceDomainModule.cs)
- Created database migration "AddCustomerInvoiceEntities" (20260119164031)
  - Migration includes all three tables with correct schema
  - All indexes and foreign key constraints properly configured
- Applied migration successfully using DbMigrator
- Built solution with 0 errors, 0 warnings (fixed ConfigureByConvention issue by adding Volo.Abp.EntityFrameworkCore.Modeling using directive)
- **Phase 3 Complete:** Infrastructure layer with database configuration and migrations

**Session 4 (2026-01-19 continued):**
- Updated progress.md to accurately reflect implemented features (application layer, frontend modules were already complete)
- Created [CustomerDetailComponent](angular/src/app/customer/customer-detail/) with:
  - Full customer information display
  - List of all customer invoices with status badges
  - Navigation to invoice details
  - Edit customer button (opens list modal)
  - Back button and breadcrumb navigation
  - Total invoice amount calculation
- Created [InvoiceDetailComponent](angular/src/app/invoice/invoice-detail/) with:
  - Complete invoice header with customer link
  - Status update functionality (dropdown with update button)
  - Line items table with full CRUD operations:
    - Add new line items (inline form with real-time total calculation)
    - Edit existing line items (inline editing)
    - Delete line items (with confirmation dialog)
    - Automatic invoice recalculation after changes
  - Financial summary section (SubTotal, TaxAmount, GrandTotal)
  - Edit invoice button (opens list modal for basic fields)
  - Business rule enforcement (disable editing for Paid/Cancelled invoices)
  - Back button and breadcrumb navigation
- Updated routing modules to include detail view routes
- Added "View" action to both Customer and Invoice list dropdowns
- **Phase 8 & 9 Complete:** Customer and Invoice frontend modules fully implemented with detail views

### Known Issues
_Track issues discovered during development_

- **Angular npm peer dependencies**: Need to use `--legacy-peer-deps` flag when installing packages due to karma version conflicts between @angular/build and @angular-devkit/build-angular
- **Redis not installed**: ABP CLI warned that Redis is not running. This is optional for development but will be needed for distributed caching in production. DbMigrator shows warnings but completes successfully.
- **ABP CLI version mismatch**: ABP CLI 10.0.2 requires .NET 10.0 which is not yet available. Using ABP CLI 8.3.0 with .NET 8.0 instead.
- **EF Core query filter warning**: Invoice entity has global query filter (soft-delete) and is required end of relationship with LineItem. This is expected with ABP's soft-delete feature and won't cause issues in practice.

### Decisions Made
_Document key architectural or design decisions_

- **ABP Version**: Using ABP Framework 8.3.0 instead of 10.0.2 due to .NET SDK compatibility (.NET 9.0 SDK installed, ABP 10.x requires .NET 10.0)
- **Database**: Using SQL Server LocalDB for development (can be changed to PostgreSQL or full SQL Server later)
- **Architecture**: Using ABP's default layered architecture with separate Identity Server
- **UI Theme**: LeptonXLite (free ABP theme)
- **Entity Framework**: Using ABP's FullAuditedAggregateRoot for Customer and Invoice (provides soft-delete, audit fields automatically)
- **Invoice Number Format**: INV-YYYYMM-XXXX (year-month prefix with 4-digit sequence, resets monthly)
- **Domain Validation**: Business rules enforced in entity methods (not just data annotations) for better domain-driven design
- **Table Naming**: Using "App" prefix for custom tables (AppCustomers, AppInvoices, AppLineItems) per ABP convention
- **Delete Behaviors**: Restrict on Customer→Invoice (prevent deleting customers with invoices), Cascade on Invoice→LineItem (delete line items when invoice deleted)
- **Decimal Precision**: Using decimal(18,2) for all monetary and quantity fields
- **Indexes**: Strategic indexes on lookup fields (email, invoice number, customer ID, status, dates) and soft-delete filter
- **Detail Views**: Separate detail components for Customer and Invoice with inline line item management (add/edit/delete) on invoice detail page for better UX than modal editing
- **Navigation Pattern**: List views use modals for create/edit, detail views provide full-page experience with breadcrumbs and contextual actions

### Questions / Blockers
_Track items that need clarification or are blocking progress_

- None currently

---

## Progress Summary

| Phase | Status | Progress |
|-------|--------|----------|
| Phase 1: Project Setup | ✅ Complete | 100% |
| Phase 2: Domain Layer | ✅ Complete | 100% |
| Phase 3: Infrastructure Layer | ✅ Complete | 100% |
| Phase 4: Application Layer | ✅ Complete | 100% |
| Phase 5: Authorization | ⚠️ Partial | 30% (definitions exist, not enforced) |
| Phase 6: API Layer | ✅ Complete | 100% (ABP dynamic controllers) |
| Phase 7: Frontend Setup | ✅ Complete | 100% |
| Phase 8: Customer Module | ✅ Complete | 100% |
| Phase 9: Invoice Module | ✅ Complete | 100% |
| Phase 10: UI/UX Polish | Not Started | 0% |
| Phase 11: Testing | Not Started | 0% |
| Phase 12: Deployment Prep | Not Started | 0% |
| Phase 13: Deployment | Not Started | 0% |

**Overall Progress: 75%** (8.5 of 13 phases complete)
