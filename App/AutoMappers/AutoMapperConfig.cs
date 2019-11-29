using AutoMapper;

namespace App.AutoMappers
{
    public class AutoMapperConfig
    {
        public static IMapper Mapper { get; private set; }

        public static void RegisterMappings()
        {
            var mapper = new MapperConfiguration(o =>
            {
                o.AddProfile<DomainToViewModelMappingProfile>();
                o.AddProfile<ViewModelToDomainMappingProfile>();
            });

            Mapper = mapper.CreateMapper();
        }
    }
}