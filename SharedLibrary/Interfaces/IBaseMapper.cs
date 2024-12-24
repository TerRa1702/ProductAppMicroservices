namespace SharedLibrary.Interfaces
{
    public interface IBaseMapper<TSource, TDestination>
    {
        TDestination MapDTO(TSource source);
        TDestination MapDTO(TSource source, TDestination destination);
        IEnumerable<TDestination> MapList(IEnumerable<TSource> source);
    }
}
