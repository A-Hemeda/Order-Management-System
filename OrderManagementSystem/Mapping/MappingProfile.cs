using AutoMapper;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product mappings
            CreateMap<Product, ProductResponse>();
            CreateMap<CreateProductRequest, Product>();
            CreateMap<UpdateProductRequest, Product>();

            // Customer mappings
            CreateMap<Customer, CustomerResponse>();
            CreateMap<CreateCustomerRequest, Customer>();
            CreateMap<UpdateCustomerRequest, Customer>();

            // Order mappings
            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            CreateMap<CreateOrderRequest, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore());
            CreateMap<UpdateOrderStatusRequest, Order>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            // OrderItem mappings
            CreateMap<OrderItem, OrderItemResponse>();
            CreateMap<CreateOrderItemRequest, OrderItem>();

            // Invoice mappings
            CreateMap<Invoice, InvoiceResponse>();
            CreateMap<CreateInvoiceRequest, Invoice>();

            // User mappings
            CreateMap<User, UserResponse>();
            CreateMap<RegisterRequest, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
} 