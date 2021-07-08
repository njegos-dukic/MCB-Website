using Core.DTO;
using Core.DTO.Menu;
using Core.Entity;
using Core.Util;
using System.Collections.Generic;

namespace Core.AutoMapper.ExtensionMethods
{
    public static class MapperExtensionMethods
    {
        public static Company ToEntity(this DetailedCompanyDTO dto) => Mapping.Mapper.Map<Company>(dto);

        public static DetailedCompanyDTO ToDetailedDto(this Company entity)
            => Mapping.Mapper.Map<DetailedCompanyDTO>(entity);

        public static OutputProductDTO ToDTO(this Product entity) => Mapping.Mapper.Map<OutputProductDTO>(entity);

        public static Product ToEntity(this InputProductDTO dto) => Mapping.Mapper.Map<Product>(dto);

        public static Menu ToEntity(this MenuDTO dto) => Mapping.Mapper.Map<Menu>(dto);

        public static MenuDTO ToDTO(this Menu entity) => Mapping.Mapper.Map<MenuDTO>(entity);

        public static Quote ToEntity(this QuoteDTO dto) => Mapping.Mapper.Map<Quote>(dto);

        public static QuoteDTO ToDTO(this Quote enitity) => Mapping.Mapper.Map<QuoteDTO>(enitity);

        public static OutputMenuDTO ToOutputDTO(this Menu entity)
            => Mapping.Mapper.Map<OutputMenuDTO>(entity);

        public static OutputMenuItemDTO ForMenu(this Product entity)
            => Mapping.Mapper.Map<OutputMenuItemDTO>(entity);

        public static OutputMenuItemServingDTO ForMenu(this ProductServing entity)
            => Mapping.Mapper.Map<OutputMenuItemServingDTO>(entity);

        public static DetailedProductServingDTO ToDTO(this ProductServing entity)
            => Mapping.Mapper.Map<ProductServing, DetailedProductServingDTO>(entity);

        public static EventDTO ToDTO(this Event entity) => Mapping.Mapper.Map<EventDTO>(entity);

        public static Event ToEntity(this EventDTO dto) => Mapping.Mapper.Map<Event>(dto);

        public static Gallery ToEntity(this GalleryBaseDTO dto) => Mapping.Mapper.Map<Gallery>(dto);

        public static DetailedGalleryDTO ToDetailedDTO(this Gallery entity) => Mapping.Mapper.Map<DetailedGalleryDTO>(entity);

        public static GalleryDTO ToGalleryDTO(this Gallery entity) => Mapping.Mapper.Map<GalleryDTO>(entity);

        public static GalleryWithThumbnailDTO ToThumbnailGalleryDTO(this Gallery entity) => Mapping.Mapper.Map<GalleryWithThumbnailDTO>(entity);

        public static GalleryBaseDTO ToGalleryBaseDTO(this Gallery entity) => Mapping.Mapper.Map<GalleryBaseDTO>(entity);

        public static DetailedMediaFileDTO ToDetailedDTO(this MediaFileDTO dto) => Mapping.Mapper.Map<DetailedMediaFileDTO>(dto);

        public static GalleryBaseDTO GalleryWithThumbnailInGalleryBase(this GalleryWithThumbnailDTO dto)
            => Mapping.Mapper.Map<GalleryBaseDTO>(dto);

        public static ShopProductServing ToEntity(this ShopProductServingDTO dto) => Mapping.Mapper.Map<ShopProductServing>(dto);

        public static OutputShopProductServingDTO ToDTO(this ShopProductServing entity)
            => Mapping.Mapper.Map<OutputShopProductServingDTO>(entity);

        public static OrderDTO ToDTO(this Order entity) => Mapping.Mapper.Map<OrderDTO>(entity);

        public static Order ToEntity(this OrderDTO dto) => Mapping.Mapper.Map<Order>(dto);

        public static OutputOrderDTO ToOutputDTO(this Order entity) => Mapping.Mapper.Map<OutputOrderDTO>(entity);
    }
}
