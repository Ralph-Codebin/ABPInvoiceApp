# Customer & Invoice Management System

## Project Overview

A modular application built with the ABP framework for managing customers and invoices, featuring a .NET backend API and Angular frontend with responsive UI.

---

## Functional Requirements

### Customer Management

#### FR-C1: Create Customer
- Capture customer information:
  - Name (required)
  - Email (required)
  - Phone
  - Billing Address

#### FR-C2: View Customer List
- Display customers in a searchable grid
- Support sorting by columns
- Filter capabilities (by name, email, etc.)

#### FR-C3: View Customer Details
- Display complete customer profile
- Show all linked invoices
- Navigation to invoice details

#### FR-C4: Edit Customer
- Modify existing customer details
- Validation on required fields
- Update billing information

#### FR-C5: Delete Customer
- Soft-delete implementation
- Preserve historical data
- Maintain invoice relationships

---

### Invoice Management

#### FR-I1: Create Invoice
- Link to existing customer (required)
- Auto-generate sequential invoice number
- Set invoice date (default: current date)
- Set due date
- Set initial status (Draft/Pending)

#### FR-I2: Manage Line Items
- Add multiple line items to invoice
- Edit existing line items
- Remove line items
- **Line Item Fields:**
  - Description (text)
  - Quantity (decimal)
  - Unit Price (decimal)
  - Total (calculated: Qty × Unit Price)

#### FR-I3: View Invoice List
- Display invoices in filterable grid
- Filter by:
  - Customer
  - Status
  - Date range
  - Invoice number
- Sort by multiple columns

#### FR-I4: View Invoice Details
- Complete invoice breakdown
- List all line items
- Calculate subtotal
- Display taxes (if applicable)
- Show grand total
- Customer information summary

#### FR-I5: Edit Invoice
- Modify invoice details based on status
- Restrict editing for finalized invoices
- Allow editing of Draft/Pending invoices
- Update line items dynamically

#### FR-I6: Update Invoice Status
- Manual status updates
- **Status Flow:**
  - Draft → Pending → Sent → Paid
  - Allow marking as Cancelled
- Status change validation

---

## Technical Architecture

### Tech Stack

| Layer | Technology |
|-------|-----------|
| **Frontend** | Angular + ABP Angular UI |
| **Backend** | .NET Core + ABP Framework |
| **ORM** | Entity Framework Core |
| **Database** | SQL Server / PostgreSQL |
| **Authentication** | ABP Identity Module (JWT) |

### Architecture Layers

```
┌─────────────────────────────────┐
│     Presentation Layer          │
│  (Angular + ABP UI Components)  │
└─────────────────────────────────┘
              ↓
┌─────────────────────────────────┐
│     Application Layer           │
│  (App Services, DTOs, Mapping)  │
└─────────────────────────────────┘
              ↓
┌─────────────────────────────────┐
│      Domain Layer               │
│  (Entities, Business Logic)     │
└─────────────────────────────────┘
              ↓
┌─────────────────────────────────┐
│   Infrastructure Layer          │
│  (EF Core, Repositories, DB)    │
└─────────────────────────────────┘
```

### Modules

#### CustomerManagementModule
- Customer CRUD operations
- Customer search and filtering
- Soft-delete implementation
- Customer-Invoice relationship management

#### InvoiceManagementModule
- Invoice CRUD operations
- Line item management
- Invoice number generation
- Status workflow management
- Invoice calculations (subtotal, tax, total)

---

## Data Model

### Customer Entity
```
Customer
├── Id (Guid) - Primary Key
├── Name (string, required, max: 200)
├── Email (string, required, max: 256)
├── Phone (string, max: 50)
├── BillingAddress (string, max: 500)
├── IsDeleted (bool) - Soft delete flag
├── CreationTime (DateTime)
├── LastModificationTime (DateTime?)
└── Invoices (Collection<Invoice>) - Navigation property
```

### Invoice Entity
```
Invoice
├── Id (Guid) - Primary Key
├── InvoiceNumber (string, required, unique)
├── CustomerId (Guid, required) - Foreign Key
├── InvoiceDate (DateTime, required)
├── DueDate (DateTime?)
├── Status (enum: Draft, Pending, Sent, Paid, Cancelled)
├── SubTotal (decimal, calculated)
├── TaxAmount (decimal)
├── GrandTotal (decimal, calculated)
├── IsDeleted (bool)
├── CreationTime (DateTime)
├── LastModificationTime (DateTime?)
├── Customer (Customer) - Navigation property
└── LineItems (Collection<LineItem>) - Navigation property
```

### LineItem Entity
```
LineItem
├── Id (Guid) - Primary Key
├── InvoiceId (Guid, required) - Foreign Key
├── Description (string, required, max: 500)
├── Quantity (decimal, required, precision: 18,2)
├── UnitPrice (decimal, required, precision: 18,2)
├── Total (decimal, calculated: Quantity × UnitPrice)
├── CreationTime (DateTime)
└── Invoice (Invoice) - Navigation property
```

### Invoice Status Enum
```csharp
public enum InvoiceStatus
{
    Draft = 0,
    Pending = 1,
    Sent = 2,
    Paid = 3,
    Cancelled = 4
}
```

---

## Data Flow

### Standard Request Flow
```
1. Frontend (Angular)
   └── User inputs data via ABP UI components

2. API Request
   └── HTTP request to ASP.NET Core API endpoint

3. Backend Processing
   ├── Controller receives request
   ├── App Service handles business logic
   ├── Domain layer validates rules
   └── Maps DTOs to entities

4. Persistence
   ├── Repository pattern (ABP Repository)
   ├── EF Core translates to SQL
   └── Database saves/retrieves data

5. Response
   ├── Entity mapped to DTO
   ├── JSON response returned
   └── Frontend displays updated data
```

---

## Security & Access Control

### Role-Based Access Control (RBAC)

Using ABP Identity Module for authentication and authorization.

#### Roles & Permissions

| Role | Permissions |
|------|-------------|
| **Admin** | Full access to all features<br>- Delete customers<br>- Delete invoices<br>- Manage users & roles |
| **Staff** | - Create/Edit customers<br>- Create/Edit/View invoices<br>- Manage line items<br>- Update invoice status |
| **Viewer** | - View customer list<br>- View invoice list<br>- View details (read-only) |

#### Permission Structure
```
CustomerManagement
├── CustomerManagement.Customers
├── CustomerManagement.Customers.Create
├── CustomerManagement.Customers.Edit
├── CustomerManagement.Customers.Delete
└── CustomerManagement.Customers.View

InvoiceManagement
├── InvoiceManagement.Invoices
├── InvoiceManagement.Invoices.Create
├── InvoiceManagement.Invoices.Edit
├── InvoiceManagement.Invoices.Delete
├── InvoiceManagement.Invoices.View
└── InvoiceManagement.Invoices.UpdateStatus
```

### Authentication
- JWT Bearer token authentication
- Token expiration and refresh mechanism
- Secure password policies via ABP Identity

---

## User Experience (UX) Requirements

### UI Framework
- ABP Angular UI components
- Professional, consistent design language
- ABP Theme integration

### Responsiveness
- Full desktop support (1920×1080, 1366×768)
- Tablet support (landscape & portrait)
- Adaptive layouts for different screen sizes

### Key UX Features
- **Navigation:** Clear sidebar/menu structure
- **Forms:** Inline validation with helpful error messages
- **Grids:** Sortable columns, pagination, search
- **Feedback:** Toast notifications for actions
- **Loading:** Progress indicators for async operations
- **Confirmation:** Modals for delete/critical actions

---

## Development Checklist

### Backend Tasks
- [ ] Set up ABP solution with modules
- [ ] Create Customer entity and repository
- [ ] Create Invoice and LineItem entities
- [ ] Implement Customer AppService (CRUD)
- [ ] Implement Invoice AppService (CRUD + calculations)
- [ ] Create DTOs and AutoMapper profiles
- [ ] Implement soft-delete for Customer
- [ ] Implement invoice number generation
- [ ] Set up permissions and authorization
- [ ] Create database migrations
- [ ] Seed initial data (roles, test customers)
- [ ] Write unit tests for domain logic
- [ ] Write integration tests for API endpoints

### Frontend Tasks
- [ ] Set up Angular application with ABP
- [ ] Create Customer list component
- [ ] Create Customer detail/edit component
- [ ] Create Invoice list component
- [ ] Create Invoice detail/edit component
- [ ] Implement line item management UI
- [ ] Add search and filter functionality
- [ ] Implement responsive layouts
- [ ] Add route guards for permissions
- [ ] Integrate authentication flow
- [ ] Add toast notifications
- [ ] Implement error handling
- [ ] Add loading indicators
- [ ] Create confirmation dialogs

### Database Tasks
- [ ] Design database schema
- [ ] Create initial migration
- [ ] Set up indexes for performance
- [ ] Configure relationships and cascades
- [ ] Test soft-delete behavior
- [ ] Validate data constraints

---

## API Endpoints Reference

### Customer Endpoints
```
GET    /api/app/customer                    - List customers
GET    /api/app/customer/{id}               - Get customer details
POST   /api/app/customer                    - Create customer
PUT    /api/app/customer/{id}               - Update customer
DELETE /api/app/customer/{id}               - Delete customer (soft)
GET    /api/app/customer/{id}/invoices      - Get customer invoices
```

### Invoice Endpoints
```
GET    /api/app/invoice                     - List invoices
GET    /api/app/invoice/{id}                - Get invoice details
POST   /api/app/invoice                     - Create invoice
PUT    /api/app/invoice/{id}                - Update invoice
DELETE /api/app/invoice/{id}                - Delete invoice
PUT    /api/app/invoice/{id}/status         - Update invoice status
POST   /api/app/invoice/{id}/line-item      - Add line item
PUT    /api/app/invoice/{id}/line-item/{lineItemId} - Update line item
DELETE /api/app/invoice/{id}/line-item/{lineItemId} - Delete line item
```

---

## Business Rules

### Customer Rules
1. Email must be unique across customers
2. Name is required and cannot exceed 200 characters
3. Soft-deleted customers remain in database but hidden from UI
4. Cannot delete customer with unpaid invoices (business constraint)

### Invoice Rules
1. Invoice number must be unique and auto-generated
2. Invoice date cannot be in the future
3. Due date must be after invoice date
4. Paid invoices cannot be edited
5. Cancelled invoices cannot change status
6. Grand Total = SubTotal + TaxAmount
7. SubTotal = Sum of all LineItem Totals
8. Each line item Total = Quantity × UnitPrice

### Line Item Rules
1. Quantity must be greater than 0
2. Unit Price must be greater than or equal to 0
3. Description is required
4. Cannot modify line items on Paid invoices

---

## Future Enhancements (Out of Scope)

- PDF invoice generation
- Email invoice to customers
- Payment gateway integration
- Recurring invoices
- Multi-currency support
- Invoice templates
- Reporting and analytics dashboard
- Audit log for changes
- Customer portal for self-service

---

## Notes for Development

- Follow ABP best practices and conventions
- Use ABP's built-in features (auditing, multi-tenancy if needed)
- Implement proper error handling and logging
- Write clean, maintainable code with SOLID principles
- Use ABP's localization for multi-language support
- Follow ABP's repository pattern and unit of work
- Leverage ABP's AutoMapper integration for DTO mapping
- Use ABP's dynamic API controller feature
- Implement proper validation with data annotations and FluentValidation
