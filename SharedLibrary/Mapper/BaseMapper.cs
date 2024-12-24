using AutoMapper;
using SharedLibrary.Interfaces;

namespace SharedLibrary.Mapper
{
    public class BaseMapper<TSourse, TDestination> : IBaseMapper<TSourse, TDestination>
    {
        private readonly IMapper _mapper;
        public BaseMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination MapDTO(TSourse source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public TDestination MapDTO(TSourse source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }

        public IEnumerable<TDestination> MapList(IEnumerable<TSourse> source)
        {
            return _mapper.Map<IEnumerable<TDestination>>(source);
        }
    }
}
