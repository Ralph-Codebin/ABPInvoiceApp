import type { FullAuditedEntityDto, CreationAuditedEntityDto } from '@abp/ng.core';

/**
 * Invoice status enumeration
 */
export enum InvoiceStatus {
  Draft = 0,
  Pending = 1,
  Sent = 2,
  Paid = 3,
  Cancelled = 4,
}

/**
 * Invoice DTO for reading invoice data
 */
export interface InvoiceDto extends FullAuditedEntityDto<string> {
  invoiceNumber: string;
  customerId: string;
  customerName: string;
  invoiceDate: string;
  dueDate?: string;
  status: InvoiceStatus;
  taxAmount: number;
  subTotal: number;
  grandTotal: number;
  lineItems: LineItemDto[];
}

/**
 * DTO for creating a new invoice
 */
export interface CreateInvoiceDto {
  customerId: string;
  invoiceDate?: string;
  dueDate?: string;
  taxAmount: number;
  lineItems: CreateLineItemDto[];
}

/**
 * DTO for updating an existing invoice
 */
export interface UpdateInvoiceDto {
  invoiceDate: string;
  dueDate?: string;
  taxAmount: number;
}

/**
 * DTO for updating invoice status
 */
export interface UpdateInvoiceStatusDto {
  status: InvoiceStatus;
}

/**
 * Line item DTO for reading line item data
 */
export interface LineItemDto extends CreationAuditedEntityDto<string> {
  invoiceId: string;
  description: string;
  quantity: number;
  unitPrice: number;
  total: number;
}

/**
 * DTO for creating a new line item
 */
export interface CreateLineItemDto {
  description: string;
  quantity: number;
  unitPrice: number;
}

/**
 * DTO for updating an existing line item
 */
export interface UpdateLineItemDto {
  description: string;
  quantity: number;
  unitPrice: number;
}
