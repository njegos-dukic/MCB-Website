using AutoMapper;

namespace MasterCraftBreweryAPI.AutoMapper
{
    public static class MappingConfiguration
    {
        public static IMapper CreateMapping()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<MappingProfile>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper;
        }
    }
}
