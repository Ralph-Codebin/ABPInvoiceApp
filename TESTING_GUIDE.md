# Testing Guide - Customer & Invoice Management System

## How to Run the Application

### Backend (ASP.NET Core)
```bash
cd aspnet-core\src\CustomerInvoice.HttpApi.Host
dotnet run
```
The API will be available at: `https://localhost:44361` (or check console output)

### Frontend (Angular)
```bash
cd angular
npm start
```
The application will be available at: `http://localhost:4200`

---

## Features to Test

### 1. Customer Management

#### Customer List (http://localhost:4200/customers)
- ✅ View list of customers in a sortable table
- ✅ Create new customer (click "New Customer" button)
- ✅ Edit existing customer (Actions dropdown → Edit)
- ✅ Delete customer (Actions dropdown → Delete with confirmation)
- ✅ **NEW:** View customer details (Actions dropdown → View)

#### Customer Detail View (http://localhost:4200/customers/{id})
- ✅ **NEW:** Display complete customer information
  - Name, Email, Phone, Billing Address
  - Creation time and last modification time
- ✅ **NEW:** List all invoices for the customer
  - Invoice number (clickable link to invoice detail)
  - Invoice date, Due date, Status badge
  - Grand total with currency formatting
  - View button to navigate to invoice details
- ✅ **NEW:** Calculate and display total amount of all invoices
- ✅ **NEW:** Edit button (navigates back to list with edit modal)
- ✅ **NEW:** Back button and breadcrumb navigation

**Test Scenarios:**
1. Create a new customer with all fields filled
2. Navigate to customer detail view
3. Verify all customer information is displayed correctly
4. Create invoices for this customer (see Invoice Management below)
5. Return to customer detail and verify invoices appear in the list
6. Click on invoice number link to navigate to invoice detail
7. Click "Edit" button and verify modal opens with customer data
8. Click "Back" button to return to customer list

---

### 2. Invoice Management

#### Invoice List (http://localhost:4200/invoices)
- ✅ View list of invoices in a sortable table
- ✅ Display: Invoice Number, Customer, Date, Status (badge), Grand Total
- ✅ Create new invoice with line items (click "New Invoice" button)
- ✅ Edit existing invoice basic info (Actions dropdown → Edit)
- ✅ Delete invoice (Actions dropdown → Delete with confirmation)
- ✅ **NEW:** View invoice details (Actions dropdown → View)

#### Invoice Detail View (http://localhost:4200/invoices/{id})
- ✅ **NEW:** Display complete invoice header
  - Invoice number, Customer name (clickable link), Invoice date, Due date
  - Status badge with color coding
  - Creation time and last modification time
- ✅ **NEW:** Update invoice status
  - Dropdown to select new status (Draft → Pending → Sent → Paid → Cancelled)
  - Update button to save status change
  - Only available for non-Paid and non-Cancelled invoices
- ✅ **NEW:** Line Items Management
  - View all line items in a table (Description, Quantity, Unit Price, Total)
  - **Add new line items:**
    - Click "Add Line Item" button
    - Inline form appears with Description, Quantity, Unit Price fields
    - Real-time total calculation (Quantity × Unit Price)
    - Save or cancel
  - **Edit existing line items:**
    - Click Edit button on any line item
    - Inline editing mode with same fields
    - Real-time total calculation
    - Save or cancel
  - **Delete line items:**
    - Click Delete button
    - Confirmation dialog appears
    - Line item removed and invoice recalculated
  - Line item management disabled for Paid/Cancelled invoices
- ✅ **NEW:** Financial Summary Section
  - SubTotal (sum of all line item totals)
  - Tax Amount
  - Grand Total (SubTotal + Tax Amount)
- ✅ **NEW:** Edit invoice button (basic fields only)
- ✅ **NEW:** Back button and breadcrumb navigation

**Test Scenarios:**

**Creating an Invoice with Line Items:**
1. Click "New Invoice" button on invoice list
2. Select a customer from dropdown
3. Set invoice date (defaults to today)
4. Set due date (optional)
5. Enter tax amount (optional)
6. Click "Add Line Item" button
7. Add multiple line items:
   - Description: "Website Design"
   - Quantity: 1
   - Unit Price: 5000
8. Add another line item:
   - Description: "Logo Design"
   - Quantity: 2
   - Unit Price: 500
9. Verify totals calculate correctly (should be 6000)
10. Click "Save"
11. Verify invoice appears in list

**Managing Line Items on Invoice Detail:**
1. Navigate to invoice detail view (click View from list)
2. Verify all invoice information displays correctly
3. **Test Adding Line Item:**
   - Click "Add Line Item" button
   - Fill in: "SEO Optimization", Quantity: 3, Unit Price: 200
   - Verify total shows 600
   - Click save (checkmark button)
   - Verify invoice Grand Total updates
4. **Test Editing Line Item:**
   - Click Edit button (pencil icon) on any line item
   - Change quantity or price
   - Verify total updates in real-time
   - Click save (checkmark button)
   - Verify invoice Grand Total updates
5. **Test Deleting Line Item:**
   - Click Delete button (trash icon) on any line item
   - Confirm deletion in dialog
   - Verify line item removed and Grand Total updated
6. **Test Status Update:**
   - Change status dropdown to "Sent"
   - Click "Update" button
   - Verify status badge updates
7. **Test Business Rules:**
   - Change status to "Paid"
   - Verify "Add Line Item" button disappears
   - Verify Edit/Delete buttons are disabled
   - Verify status dropdown is disabled

**Navigation Testing:**
1. From invoice detail, click customer name link
2. Verify navigates to customer detail page
3. From customer detail, click invoice number link
4. Verify navigates back to invoice detail
5. Test breadcrumb navigation
6. Test Back buttons

---

### 3. Status Badge Colors

Verify status badges display with correct colors:
- **Draft** - Grey/Secondary
- **Pending** - Yellow/Warning
- **Sent** - Blue/Info
- **Paid** - Green/Success
- **Cancelled** - Red/Danger

---

### 4. Business Rules to Verify

#### Invoice Business Rules:
1. ✅ Cannot edit Paid invoices
2. ✅ Cannot edit Cancelled invoices
3. ✅ Cannot change status of Cancelled invoices
4. ✅ Invoice date cannot be in the future (backend validation)
5. ✅ Due date must be after invoice date (backend validation)
6. ✅ Line item quantity must be > 0
7. ✅ Line item unit price must be >= 0

#### Customer Business Rules:
1. ✅ Email must be unique (backend validation)
2. ✅ Name is required
3. ✅ Email format validation
4. ✅ Soft delete (customers remain in database)

---

### 5. UI/UX to Verify

#### Responsiveness:
- Test on desktop (1920×1080, 1366×768)
- Test on tablet (if available)
- Verify tables are scrollable on small screens

#### Loading States:
- Verify loading spinner appears when loading customer detail
- Verify loading spinner appears when loading invoice detail

#### Error Handling:
- Navigate to invalid customer ID (e.g., /customers/00000000-0000-0000-0000-000000000000)
- Verify redirect to customer list
- Try to save invoice with missing required fields
- Verify validation messages

#### Toast Notifications:
- Verify success message when adding line item
- Verify success message when updating line item
- Verify success message when deleting line item
- Verify success message when updating invoice status

---

## Known Issues to Track During Testing

1. **Localization Keys:**
   - Some localization keys may show as "::KeyName" instead of translated text
   - This is expected if localization resources haven't been added
   - Keys used:
     - `::CustomerDetails`, `::InvoiceDetails`
     - `::LineItems`, `::AddLineItem`, `::LineItemAdded`, etc.
     - `::FinancialSummary`, `::SubTotal`, `::TaxAmount`, `::GrandTotal`
     - `::NoInvoicesFound`, `::NoLineItemsFound`
     - `::UpdateStatus`, `::TotalAmount`

2. **Edit Customer from Detail:**
   - Currently navigates back to list page and opens modal
   - Could be improved to have inline editing on detail page

3. **Edit Invoice from Detail:**
   - Only edits basic fields (dates, tax amount)
   - Line items must be edited using Add/Edit/Delete buttons
   - Could be improved with a full edit mode

---

## Test Data Suggestions

### Sample Customers:
1. **Acme Corporation**
   - Email: contact@acme.com
   - Phone: (555) 123-4567
   - Billing Address: 123 Main St, Suite 100, San Francisco, CA 94105

2. **Tech Solutions Inc**
   - Email: info@techsolutions.com
   - Phone: (555) 987-6543
   - Billing Address: 456 Innovation Drive, Austin, TX 78701

### Sample Invoices:

**For Acme Corporation:**
- **Invoice 1 (Draft):**
  - Line Items:
    - Website Redesign: Qty 1, Price $10,000
    - Hosting (Annual): Qty 1, Price $1,200
  - Tax Amount: $1,120
  - Grand Total: $12,320

- **Invoice 2 (Paid):**
  - Line Items:
    - Consulting Hours: Qty 20, Price $150
  - Tax Amount: $300
  - Grand Total: $3,300

**For Tech Solutions Inc:**
- **Invoice 1 (Sent):**
  - Line Items:
    - Logo Design: Qty 1, Price $2,500
    - Business Cards: Qty 500, Price $0.50
  - Tax Amount: $275
  - Grand Total: $3,025

---

## API Testing (Optional)

If you want to test the API directly:

### Swagger UI
Navigate to: `https://localhost:44361/swagger`

### Test Endpoints:
- GET `/api/app/customer` - List customers
- GET `/api/app/customer/{id}` - Get customer
- GET `/api/app/invoice/{id}` - Get invoice with line items
- POST `/api/app/invoice/{invoiceId}/line-item` - Add line item
- PUT `/api/app/invoice/{invoiceId}/line-item/{lineItemId}` - Update line item
- DELETE `/api/app/invoice/{invoiceId}/line-item/{lineItemId}` - Delete line item
- PUT `/api/app/invoice/{id}/status` - Update invoice status

---

## Next Steps After Testing

Based on test results, the following enhancements could be prioritized:

1. **Phase 5: Authorization**
   - Implement permission enforcement in UI
   - Add role-based access control
   - Seed test users with different roles

2. **Phase 10: UI/UX Polish**
   - Add advanced filtering on invoice list
   - Improve responsive design
   - Add empty state messages
   - Enhance error handling

3. **Phase 11: Testing**
   - Write unit tests
   - Write integration tests
   - Add E2E tests

4. **Localization:**
   - Add localization resources for new keys
   - Support multiple languages
