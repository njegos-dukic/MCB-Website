using AutoMapper;
using Core.DTO;
using Core.DTO.Menu;
using Core.Entity;
using System;
using System.Linq;

namespace Core.AutoMapper
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapCompany();
            MapProduct();
            MapMenu();
            MapAdministrator();
            MapQuote();
            MapEvents();
            MapGallery();
            MapShop();
        }

        private void MapGallery()
        {
            CreateMap<MediaFile, MediaFileDTO>().ReverseMap();
            CreateMap<MediaFileDTO, DetailedMediaFileDTO>();
            CreateMap<MediaFile, DetailedMediaFileDTO>().IncludeBase<MediaFile, MediaFileDTO>().ReverseMap();
            CreateMap<GalleryBaseDTO, Gallery>();
            CreateMap<Gallery, GalleryBaseDTO>()
                                        .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedOn, DateTimeKind.Utc)));
            CreateMap<GalleryWithThumbnailDTO, GalleryBaseDTO>();
            CreateMap<DetailedGalleryDTO, Gallery>().IncludeBase<GalleryBaseDTO, Gallery>()
                                        .ForMember(dest => dest.MediaFiles, opt => opt.MapFrom(src => src.MediaFiles));
            CreateMap<GalleryDTO, Gallery>().IncludeBase<GalleryBaseDTO, Gallery>()
                                        .ForMember(dest => dest.MediaFiles, opt => opt.MapFrom(src => src.MediaFiles));
            CreateMap<Gallery, DetailedGalleryDTO>().IncludeBase<Gallery, GalleryBaseDTO>()
                                        .ForMember(dest => dest.MediaFiles, opt => opt.MapFrom(src => src.MediaFiles));
            CreateMap<Gallery, GalleryDTO>().IncludeBase<Gallery, GalleryBaseDTO>()
                                        .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedOn, DateTimeKind.Utc)))
                                        .ForMember(dest => dest.MediaFiles, opt => opt.MapFrom(src => src.MediaFiles));
            CreateMap<Gallery, GalleryWithThumbnailDTO>().IncludeBase<Gallery, GalleryBaseDTO>()
                                        .ForMember(dest => dest.MediaFiles, opt => opt.MapFrom(src => src.MediaFiles))
                                        .ForMember(dest => dest.Thumbnail, opt => opt.Ignore());
        }

        private void MapShop()
        {
            CreateMap<ShopProductServingDTO, ShopProductServing>();
            CreateMap<ShopProductServing, OutputShopProductServingDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Product.Description))
                .ForMember(dest => dest.ServingName, opt => opt.MapFrom(src => src.Serving.Name))
                .ForMember(dest => dest.PackageAmount, opt => opt.MapFrom(src => src.ShopAmount.PackageAmount))
                .ForMember(dest => dest.IncrementAmount, opt => opt.MapFrom(src => src.ShopAmount.IncrementAmount));

            CreateMap<ProductOrderDTO, ProductOrder>().ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.OrderedAmount));
            CreateMap<ProductOrder, OutputProductOrderDTO>().ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.ShopProductServing));

            CreateMap<BaseOrderDTO, Order>().ForMember(dest => dest.OrderedOn, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Order, BaseOrderDTO>().ForMember(dest => dest.OrderedOn, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.OrderedOn, DateTimeKind.Utc))); ;
            CreateMap<Order, OrderDTO>().IncludeBase<Order, BaseOrderDTO>()
                                         .ForMember(dest => dest.ProductOrders, opt => opt.Ignore());

            CreateMap<OrderDTO, Order>().IncludeBase<BaseOrderDTO, Order>().ForMember(dest => dest.ProductOrders, opt => opt.MapFrom(src => src.ProductOrders));
            CreateMap<Order, OutputOrderDTO>().IncludeBase<Order, BaseOrderDTO>()
                                              .ForMember(dest => dest.ProductOrders, opt => opt.MapFrom(src => src.ProductOrders));
        }

        private void MapEvents()
        {
            CreateMap<Event, EventDTO>().ForMember(dest => dest.BeginOn, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.BeginOn, DateTimeKind.Utc)))
                                        .ForMember(dest => dest.EndOn, opt =>
                                          {
                                              opt.PreCondition(src => src.EndOn.HasValue);
                                              opt.MapFrom(src => DateTime.SpecifyKind(src.EndOn.Value, DateTimeKind.Utc));
                                          });
            CreateMap<EventDTO, Event>().ForMember(dest => dest.BeginOn, opt => opt.MapFrom(src => src.BeginOn.ToUniversalTime()))
                                        .ForMember(dest => dest.EndOn, opt =>
                                        {
                                            opt.PreCondition(src => src.EndOn.HasValue);
                                            opt.MapFrom(src => src.EndOn.Value.ToUniversalTime());
                                        });
        }

        private void MapQuote()
        {
            CreateMap<QuoteDTO, Quote>();
            CreateMap<Quote, QuoteDTO>()
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedOn, DateTimeKind.Utc)));

        }

        private void MapAdministrator()
        {
            CreateMap<Administrator, AdministratorDTO>().ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<AdministratorDTO, Administrator>().ForMember(dest => dest.Salt, opt => opt.Ignore())
                                                        .ForMember(dest => dest.Password, opt => opt.Ignore());
        }

        private void MapCompany()
        {
            CreateMap<SocialMediaDTO, SocialMedia>().ReverseMap();
            CreateMap<WholesaleDTO, Wholesale>().ReverseMap();
            CreateMap<PhoneDTO, Phone>().ReverseMap();
            CreateMap<CompanyDTO, Company>().ReverseMap();
            CreateMap<DetailedCompanyDTO, Company>().IncludeBase<CompanyDTO, Company>()
                                                    .ForMember(dest => dest.SocialMedias, opt => opt.MapFrom(src => src.SocialMedias))
                                                    .ForMember(dest => dest.Wholesales, opt => opt.MapFrom(src => src.Wholesales))
                                                    .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones));

            CreateMap<Company, DetailedCompanyDTO>().IncludeBase<Company, CompanyDTO>()
                                                    .ForMember(dest => dest.SocialMedias, opt => opt.MapFrom(src => src.SocialMedias))
                                                    .ForMember(dest => dest.Wholesales, opt => opt.MapFrom(src => src.Wholesales))
                                                    .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones));
        }

        private void MapProduct()
        {
            CreateMap<string, Ingredient>().ForMember(src => src.Name, opt => opt.MapFrom(src => src));
            CreateMap<ProductServingDTO, ProductServing>().ReverseMap();
            CreateMap<ProductServing, OutputProductServingDTO>().IncludeBase<ProductServing, ProductServingDTO>()
                                                                 .ForMember(dest => dest.ServingName, opt => opt.MapFrom(src => src.Serving.Name));
            CreateMap<ProductTypeDTO, ProductType>().ReverseMap();
            CreateMap<Serving, ServingDTO>().ReverseMap();
            CreateMap<Serving, ProductServingDTO>();
            CreateMap<Product, BaseProductDTO>().ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients.Select(x => x.Name)));
            CreateMap<BaseProductDTO, Product>().ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients));

            CreateMap<InputProductDTO, Product>().IncludeBase<BaseProductDTO, Product>()
                                                 .ForMember(dest => dest.ProductServings, opt => opt.MapFrom(src => src.ProductServings));

            CreateMap<Product, OutputProductDTO>().IncludeBase<Product, BaseProductDTO>()
                                                  .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType))
                                                  .ForMember(dest => dest.ProductServings, opt => opt.MapFrom(src => src.ProductServings));
            CreateMap<Product, MenuProductDTO>().IncludeBase<Product, BaseProductDTO>()
                                                   .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType));

            CreateMap<ProductServing, DetailedProductServingDTO>().IncludeBase<ProductServing, OutputProductServingDTO>()
                                                                  .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
        }

        private void MapMenu()
        {
            CreateMap<MenuItemDTO, MenuItem>().ForMember(dest => dest.ProductServing, opt => opt.Ignore());
            CreateMap<MenuDTO, Menu>().ForMember(dest => dest.MenuItems, opt => opt.MapFrom(src => src.MenuItems));

            CreateMap<MenuItem, MenuItemDTO>().ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.ProductServing.Product))
                                              .ForMember(dest => dest.Serving, opt => opt.MapFrom(src => src.ProductServing.Serving))
                                              .ForMember(dest => dest.ProductServing, opt => opt.MapFrom(src => src.ProductServing));

            CreateMap<Menu, MenuDTO>().ForMember(dest => dest.MenuItems, opt => opt.MapFrom(src => src.MenuItems));

            CreateMap<ProductServing, OutputMenuItemServingDTO>().ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                                                                 .ForMember(dest => dest.ServingId, opt => opt.MapFrom(src => src.ServingId))
                                                                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Serving.Name))
                                                                 .ForMember(dest => dest.ProductServingId, opt => opt.MapFrom(src => src.ProductServingId));

            CreateMap<Product, OutputMenuItemDTO>().ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients.Select(x => x.Name)));
            CreateMap<Menu, OutputMenuDTO>().ForMember(dest => dest.MenuItems, opt => opt.Ignore());
        }
    }
}
