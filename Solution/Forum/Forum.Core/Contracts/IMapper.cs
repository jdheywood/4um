namespace Forum.Core.Contracts
{
    public interface IMapper
    {
        TDestination Map<TDestination>(params object[] sources);

        void Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}
