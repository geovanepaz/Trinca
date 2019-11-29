using AutoMapper;

namespace Cross.Util.Extensions
{
    public static class MapperExtension
    {
        public static T MapTo<T>(this object src, IMapper mapper) => (T)mapper.Map(src, src.GetType(), typeof(T));
    }
}