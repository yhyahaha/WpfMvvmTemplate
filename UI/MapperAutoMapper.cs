using InterFaces;
using Model;
using ViewModel;
using AutoMapper;

namespace UI
{
    public class MapperAutoMapper : IObjectMapping
    {
        private Mapper mapper;
        
        // PersonViewModel →　Person
        public MapperAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Person, PersonViewModel>();
            });

            mapper = new Mapper(config);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return mapper.Map<TDestination>(source);
        }
    }

    // Person → PersonViewModel
    public class ReverseMapperAutoMapper : IObjectMapping
    {
        private Mapper mapper;

        public ReverseMapperAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PersonViewModel,Person>();
            });

            mapper = new Mapper(config);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return mapper.Map<TDestination>(source);
        }
    }



}
