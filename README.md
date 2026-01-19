# Customer & Invoice Management System

[![Watch the video](./thumb.png)](https://drive.google.com/file/d/1yU5xuukio-bCvhKgHgtdwnVnNEX5MAB-/view?usp=sharing)


A modern, full-stack application built with the ABP Framework for managing customers and invoices. Features a .NET backend API with Entity Framework Core and an Angular frontend with responsive UI.

## Features

### Customer Management
- Create, view, edit, and delete customers
- Searchable and filterable customer grid
- Customer profile with linked invoices
- Soft-delete implementation for data preservation

### Invoice Management
- Create and manage invoices linked to customers
- Auto-generated sequential invoice numbers
- Dynamic line item management
- Invoice status workflow (Draft → Pending → Sent → Paid → Cancelled)
- Automatic calculations (subtotal, tax, grand total)
- Comprehensive filtering and search capabilities

## Tech Stack

### Backend
- .NET Core with ABP Framework
- Entity Framework Core
- SQL Server / PostgreSQL
- ABP Identity Module (JWT authentication)

### Frontend
- Angular
- ABP Angular UI Components
- Responsive design
- Professional ABP Theme

## Project Structure

```
ABPInvoiceApp/
├── aspnet-core/                    # .NET backend
│   ├── src/
│   │   ├── CustomerInvoice.Application/
│   │   ├── CustomerInvoice.Application.Contracts/
│   │   ├── CustomerInvoice.Domain/
│   │   ├── CustomerInvoice.Domain.Shared/
│   │   ├── CustomerInvoice.EntityFrameworkCore/
│   │   ├── CustomerInvoice.HttpApi/
│   │   ├── CustomerInvoice.HttpApi.Host/
│   │   ├── CustomerInvoice.AuthServer/
│   │   └── CustomerInvoice.DbMigrator/
│   └── test/
├── angular/                        # Angular frontend
│   ├── src/
│   │   ├── app/
│   │   │   ├── customer/
│   │   │   ├── invoice/
│   │   │   └── proxy/
│   │   └── environments/
└── CLAUDE.md                       # Detailed project specification
```

## Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Node.js 18.x or later
- SQL Server or PostgreSQL
- Visual Studio 2022 / VS Code / Rider (optional)

### Backend Setup

1. Navigate to the backend directory:
```bash
cd aspnet-core
```

2. Update the connection string in `src/CustomerInvoice.DbMigrator/appsettings.json`

3. Run database migrations:
```bash
cd src/CustomerInvoice.DbMigrator
dotnet run
```

4. Start the API server:
```bash
cd ../CustomerInvoice.HttpApi.Host
dotnet run
```

The API will be available at `https://localhost:44300` (or configured port)

### Frontend Setup

1. Navigate to the Angular directory:
```bash
cd angular
```

2. Install dependencies:
```bash
npm install
```

3. Update the API endpoint in `src/environments/environment.ts`

4. Start the development server:
```bash
npm start
```

The application will be available at `http://localhost:4200`

## Architecture

### Domain-Driven Design (DDD)
The application follows ABP's DDD architecture with clear separation of concerns:

- **Presentation Layer**: Angular UI components
- **Application Layer**: Application services, DTOs, and mapping
- **Domain Layer**: Entities, business logic, and domain services
- **Infrastructure Layer**: EF Core, repositories, and database

### Key Entities

**Customer**
- Name, Email, Phone, Billing Address
- Soft-delete support
- One-to-many relationship with Invoices

**Invoice**
- Auto-generated invoice number
- Invoice date, due date, status
- Customer reference
- Calculated totals (subtotal, tax, grand total)
- One-to-many relationship with Line Items

**LineItem**
- Description, quantity, unit price
- Calculated total (quantity × unit price)
- Belongs to an Invoice

## Security & Permissions

Role-based access control (RBAC) with ABP Identity:

### Roles
- **Admin**: Full system access
- **Staff**: Create/edit customers and invoices
- **Viewer**: Read-only access

### Permissions
- CustomerManagement.Customers (Create, Edit, Delete, View)
- InvoiceManagement.Invoices (Create, Edit, Delete, View, UpdateStatus)

## API Endpoints

### Customers
- `GET /api/app/customer` - List customers
- `GET /api/app/customer/{id}` - Get customer details
- `POST /api/app/customer` - Create customer
- `PUT /api/app/customer/{id}` - Update customer
- `DELETE /api/app/customer/{id}` - Delete customer

### Invoices
- `GET /api/app/invoice` - List invoices
- `GET /api/app/invoice/{id}` - Get invoice details
- `POST /api/app/invoice` - Create invoice
- `PUT /api/app/invoice/{id}` - Update invoice
- `DELETE /api/app/invoice/{id}` - Delete invoice
- `PUT /api/app/invoice/{id}/status` - Update invoice status

### Line Items
- `POST /api/app/invoice/{id}/line-item` - Add line item
- `PUT /api/app/invoice/{id}/line-item/{lineItemId}` - Update line item
- `DELETE /api/app/invoice/{id}/line-item/{lineItemId}` - Delete line item

## Business Rules

### Customer Rules
- Email must be unique
- Name is required (max 200 characters)
- Soft-deleted customers remain in database
- Cannot delete customer with unpaid invoices

### Invoice Rules
- Invoice number is auto-generated and unique
- Invoice date cannot be in the future
- Due date must be after invoice date
- Paid invoices cannot be edited
- Grand Total = SubTotal + TaxAmount
- SubTotal = Sum of all LineItem Totals

### Line Item Rules
- Quantity must be greater than 0
- Unit Price must be greater than or equal to 0
- Description is required
- Cannot modify line items on Paid invoices

## Documentation

- [CLAUDE.md](CLAUDE.md) - Complete project specification
- [IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md) - Implementation details
- [NAVIGATION_FLOW.md](NAVIGATION_FLOW.md) - User navigation flows
- [TESTING_GUIDE.md](TESTING_GUIDE.md) - Testing instructions

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License.

## Support

For issues and questions:
- Create an issue in the GitHub repository
- Refer to [ABP Documentation](https://abp.io/docs)

## Acknowledgments

Built with [ABP Framework](https://abp.io) - Open Source Web Application Framework
