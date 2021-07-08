using AutoMapper;
using Core.Util;
using MasterCraftBreweryAPI.Wrapper;

namespace MasterCraftBreweryAPI.Mapper
{
    public static class ThumbnailExtensionMethods
    {
        public static ThumbnailDimensions ToCoreObject(this ThumbnailDimensionsWrapper wrapper, IMapper mapper)
            => (wrapper.Width == 0 || wrapper.Height == 0) ? null : mapper.Map<ThumbnailDimensions>(wrapper);
    }
}
