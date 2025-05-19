using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings
{
    public class SalesProfile : Profile
    {
        public SalesProfile()
        {
            // Request to Command
            CreateMap<CreateSaleRequest, CreateSaleCommand>();
            CreateMap<CreateSaleItemRequest, SaleItemDto>();
            CreateMap<ListSalesRequest, ListSalesCommand>();
            CreateMap<UpdateSaleRequest, UpdateSaleCommand>();

            // Entity to Result
            CreateMap<Sale, CreateSaleResult>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<SaleItem, SaleItemResult>();

            // Result to Response
            CreateMap<CreateSaleResult, CreateSaleResponse>();
            CreateMap<SaleItemResult, CreateSaleItemResponse>();
        }
    }

}
