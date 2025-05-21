using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Application.Sales.SaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
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
            CreateMap<UpdateSaleItemRequest, UpdateSaleItemCommand>();

            // Entity to Result
            CreateMap<Sale, CreateSaleResult>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            CreateMap<Sale, UpdateSaleResult>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            CreateMap<Sale, ListSalesResult>();
            CreateMap<Sale, GetSaleResult>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<SaleItem, SaleItemResult>();
            CreateMap<SaleItem, UpdateSaleItemResult>();
            CreateMap<ListSalesResult, ListSalesResponse>();
            CreateMap<PaginatedResult<ListSalesResult>, PaginatedList<ListSalesResponse>>()
                .ConstructUsing((src, ctx) => new PaginatedList<ListSalesResponse>())
                .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
                .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
                .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
                .AfterMap((src, dest, ctx) =>
                {
                    // Clear any existing items
                    dest.Clear();

                    // Add all items after mapping them individually
                    var mappedItems = src.Select(item => ctx.Mapper.Map<ListSalesResponse>(item)).ToList();
                    dest.AddRange(mappedItems);
                });

            AllowNullCollections = true;
            AddGlobalIgnore("Item");

            // Result to Response
            CreateMap<CreateSaleResult, CreateSaleResponse>();
            CreateMap<SaleItemResult, CreateSaleItemResponse>();
            CreateMap<GetSaleResult, GetSaleResponse>();
            CreateMap<UpdateSaleResult, UpdateSaleResponse>();
            CreateMap<UpdateSaleItemResult, UpdateSaleItemResponse>();
        }
    }

}
