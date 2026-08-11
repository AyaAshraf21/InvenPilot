using AutoMapper;
using InvenPilot.Application.Features.Categories.DTO;
using InvenPilot.Application.Features.Customers.DTO;
using InvenPilot.Application.Features.Orders.DTO;
using InvenPilot.Application.Features.Products.DTO;
using InvenPilot.Application.Features.Suppliers.DTO;
using InvenPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductResponseDTO>();
            CreateMap<ProductDTO, Product>();

            CreateMap<Supplier, SupplierResponseDTO>();
            CreateMap<SupplierDTO, Supplier>();

            CreateMap<Customer, CustomerResponseDTO>();
            CreateMap<CustomerDTO, Customer>();

            CreateMap<Category, CategoryResponseDTO>();
            CreateMap<CategoryDTO, Category>();

            CreateMap<Order, OrderResponseDTO>();
            CreateMap<OrderDTO, Order>();
            CreateMap<OrderItem, OrderItemResponseDTO>();
            CreateMap<OrderItemDTO, OrderItem>();
        }
    }
}
