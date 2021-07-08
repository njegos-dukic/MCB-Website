using AutoMapper;
using Core.DTO;
using Core.DTO.Menu;
using Core.ErrorHandling;
using Core.Util;
using MasterCraftBreweryAPI.Util;
using MasterCraftBreweryAPI.Wrapper;
using MasterCraftBreweryAPI.Wrapper.Company;
using MasterCraftBreweryAPI.Wrapper.Contact;
using MasterCraftBreweryAPI.Wrapper.Event;
using MasterCraftBreweryAPI.Wrapper.Menu;
using MasterCraftBreweryAPI.Wrapper.Product;
using MasterCraftBreweryAPI.Wrapper.Shop;
using System;

namespace MasterCraftBreweryAPI.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapCompany();
            MapProduct();
            MapThumbnail();
            MapMenu();
            MapLogin();
            MapRegistration();
            MapQuote();
            MapEvent();
            MapShop();
            MapContact();
        }

        private void MapContact()
        {
            CreateMap<ContactMessagePostWrapper, ContactDTO>();
            CreateMap<bool, ContactMessagePostResponseWrapper>().ForCtorParam("isSent", conf => conf.MapFrom(x => x));
            CreateMap<ResultMessage<bool>, ResultMessage<ContactMessagePostResponseWrapper>>();
        }

        private void MapShop()
        {
            CreateMap<ShopProductServingPostWrapper, ShopProductServingDTO>();
            CreateMap<bool, IsShopProductServingDeletedResponseWrapper>().ForCtorParam("isDeleted", conf => conf.MapFrom(x => x));
            CreateMap<ResultMessage<bool>, ResultMessage<IsShopProductServingDeletedResponseWrapper>>();
            CreateMap<bool, AddedReponseWrapper>().ForCtorParam("isAdded", conf => conf.MapFrom(x => x));
            CreateMap<ResultMessage<bool>, ResultMessage<AddedReponseWrapper>>();
            CreateMap<ResultMessage<bool>, ResultMessage<Wrapper.Shop.ChangeImageResponseWrapper>>();
            CreateMap<bool, Wrapper.Shop.ChangeImageResponseWrapper>().ForCtorParam("isChanged", conf => conf.MapFrom(x => x));
            CreateMap<OrderPostWrapper, OrderDTO>();
            CreateMap<ProductOrderWrapper, ProductOrderDTO>();
            MapGallery();
        }

        private void MapGallery()
        {
            CreateMap<GalleryPostWrapper, GalleryBaseDTO>().ReverseMap();
            CreateMap<MediaFilePutWrapper, MediaFileDTO>();
            CreateMap<GalleryPutWrapper, GalleryDTO>()
                                .ForMember(dest => dest.MediaFiles, opt => opt.MapFrom(src => src.MediaFiles));
            CreateMap<ResultMessage<bool>, ResultMessage<GalleryDeleteWrapper>>();
            CreateMap<bool, GalleryDeleteWrapper>().ForCtorParam("isDeleted", conf => conf.MapFrom(x => x));
        }

        private void MapQuote()
        {
            CreateMap<QuotePostWrapper, QuoteDTO>();
            CreateMap<QuotePutWrapper, QuoteDTO>();
            CreateMap<ResultMessage<bool>, ResultMessage<QuoteDeleteWrapper>>();
            CreateMap<bool, QuoteDeleteWrapper>().ForCtorParam("isDeleted", conf => conf.MapFrom(x => x));
        }

        private void MapEvent()
        {
            CreateMap<EventPostWrapper, EventDTO>();
            CreateMap<EventPutWrapper, EventDTO>();
            CreateMap<ResultMessage<bool>, ResultMessage<EventDeleteResponseWrapper>>();
            CreateMap<bool, EventDeleteResponseWrapper>().ForCtorParam("isDeleted", conf => conf.MapFrom(x => x));
            CreateMap<ResultMessage<bool>, ResultMessage<Wrapper.Event.ChangeImageResponseWrapper>>();
            CreateMap<bool, Wrapper.Event.ChangeImageResponseWrapper>().ForCtorParam("isChanged", conf => conf.MapFrom(x => x));
        }

        private void MapRegistration()
        {
            CreateMap<ResultMessage<bool>, ResultMessage<AdministratorPostResponseWrapper>>();
            CreateMap<bool, AdministratorPostResponseWrapper>().ForCtorParam("isRegistered", conf => conf.MapFrom(x => x));
            CreateMap<AdministratorPostWrapper, AdministratorDTO>();
        }

        private void MapLogin()
        {
            CreateMap<ResultMessage<string>, ResultMessage<LoginResponseWrapper>>();
            CreateMap<string, LoginResponseWrapper>().ForCtorParam("token", conf => conf.MapFrom(x => x));
            CreateMap<ResultMessage<bool>, ResultMessage<IsLoggedInResponseWrapper>>();
            CreateMap<bool, IsLoggedInResponseWrapper>().ForCtorParam("isLoggedIn", conf => conf.MapFrom(x => x));
        }

        private void MapMenu()
        {
            CreateMap<MenuItemPostWrapper, MenuItemDTO>().ForMember(dest => dest.ProductServing, opt => opt.Ignore());
            CreateMap<MenuPostWrapper, MenuDTO>().ForMember(dest => dest.MenuItems, opt => opt.MapFrom(src => src.MenuItems));

            CreateMap<MenuItemPutWrapper, MenuItemDTO>().ForMember(dest => dest.ProductServing, opt => opt.Ignore());
            CreateMap<MenuPutWrapper, MenuDTO>().ForMember(dest => dest.MenuItems, opt => opt.MapFrom(src => src.MenuItems));

            CreateMap<ResultMessage<bool>, ResultMessage<MenuDeleteWrapper>>();
            CreateMap<bool, MenuDeleteWrapper>().ForCtorParam("isDeleted", conf => conf.MapFrom(x => x));
        }

        private void MapThumbnail()
        {
            CreateMap<ThumbnailDimensions, ThumbnailDimensionsWrapper>().ReverseMap();
        }

        private void MapProduct()
        {
            CreateMap<ProductServingDTO, ProductServingWrapper>().ReverseMap();
            CreateMap<ProductPostWrapper, InputProductDTO>();
            CreateMap<ProductServingPostWrapper, ProductServingDTO>();
            CreateMap<ProductPutWrapper, InputProductDTO>();
            CreateMap<ResultMessage<bool>, ResultMessage<ProductDeleteWrapper>>();
            CreateMap<bool, ProductDeleteWrapper>().ForCtorParam("isDeleted", conf => conf.MapFrom(x => x));
            CreateMap<ResultMessage<bool>, ResultMessage<Wrapper.Product.ChangeImageResponseWrapper>>();
            CreateMap<bool, Wrapper.Product.ChangeImageResponseWrapper>().ForCtorParam("isChanged", conf => conf.MapFrom(x => x));
        }

        private void MapCompany()
        {
            CreateMap<PhoneWrapper, PhoneDTO>().ReverseMap();
            CreateMap<SocialMediaWrapper, SocialMediaDTO>().ReverseMap();
            CreateMap<WholesaleWrapper, WholesaleDTO>().ReverseMap();
            CreateMap<DetailedCompanyPostWrapper, DetailedCompanyDTO>().ForMember(dest => dest.SocialMedias, opt => opt.MapFrom(src => src.SocialMedias))
                                                                       .ForMember(dest => dest.Wholesales, opt => opt.MapFrom(src => src.Wholesales))
                                                                       .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones));
            CreateMap<DetailedCompanyPutWrapper, DetailedCompanyDTO>().ForMember(dest => dest.SocialMedias, opt => opt.MapFrom(src => src.SocialMedias))
                                                                      .ForMember(dest => dest.Wholesales, opt => opt.MapFrom(src => src.Wholesales))
                                                                      .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones));
            CreateMap<ResultMessage<bool>, ResultMessage<CompanyDeleteWrapper>>();
            CreateMap<bool, CompanyDeleteWrapper>().ForCtorParam("isDeleted", conf => conf.MapFrom(x => x));
        }
    }
}
