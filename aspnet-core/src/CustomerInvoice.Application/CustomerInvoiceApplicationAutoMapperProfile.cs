using System;
using AutoMapper;
using CustomerInvoice.Customers;
using CustomerInvoice.Entities;
using CustomerInvoice.Invoices;

namespace CustomerInvoice;

public class CustomerInvoiceApplicationAutoMapperProfile : Profile
{
    public CustomerInvoiceApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        #region Customer Mappings

        // Entity to DTO mappings
        CreateMap<Customer, CustomerDto>();

        // DTO to Entity mappings for creating
        CreateMap<CreateCustomerDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Invoices, opt => opt.Ignore());

        // DTO to Entity mappings for updating
        CreateMap<UpdateCustomerDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Invoices, opt => opt.Ignore());

        #endregion

        #region Invoice Mappings

        // Entity to DTO mappings
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal))
            .ForMember(dest => dest.GrandTotal, opt => opt.MapFrom(src => src.GrandTotal));

        // DTO to Entity mappings for creating
        CreateMap<CreateInvoiceDto, Invoice>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.InvoiceNumber, opt => opt.Ignore()) // Auto-generated
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => InvoiceStatus.Draft))
            .ForMember(dest => dest.InvoiceDate, opt => opt.MapFrom(src => src.InvoiceDate ?? DateTime.Now))
            .ForMember(dest => dest.LineItems, opt => opt.Ignore()); // Handled separately

        // DTO to Entity mappings for updating
        CreateMap<UpdateInvoiceDto, Invoice>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.InvoiceNumber, opt => opt.Ignore())
            .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.LineItems, opt => opt.Ignore());

        #endregion

        #region LineItem Mappings

        // Entity to DTO mappings
        CreateMap<LineItem, LineItemDto>()
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total));

        // DTO to Entity mappings for creating
        CreateMap<CreateLineItemDto, LineItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.InvoiceId, opt => opt.Ignore())
            .ForMember(dest => dest.Invoice, opt => opt.Ignore());

        // DTO to Entity mappings for updating
        CreateMap<UpdateLineItemDto, LineItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.InvoiceId, opt => opt.Ignore())
            .ForMember(dest => dest.Invoice, opt => opt.Ignore());

        #endregion
    }
}
