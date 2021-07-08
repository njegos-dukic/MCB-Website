using AutoMapper;
using System;

namespace Core.AutoMapper
{
    /// <summary>
    /// Automapper provides many ways of creating configuration, and this approach 
    /// is based on static api.
    /// <see cref="https://docs.automapper.org/en/v8.1.0/Static-and-Instance-API.html"/>
    /// </summary>
    public static class Mapping
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }
}
