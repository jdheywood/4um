using System;
using System.Linq;
using Castle.Core.Internal;
using Forum.Core.Contracts;

namespace Forum.Core.Mapping
{
    public class Mapper : IMapper
    {
        public TDestination Map<TDestination>(params object[] sources)
        {
            if (sources == null || !sources.Any())
            {
                throw new ArgumentException("At least one source object must be passed to Map");
            }

            if (sources.Count() == 1)
            {
                return AutoMapper.Mapper.Map<TDestination>(sources.First());
            }

            var firstSource = sources.First();

            var destination = AutoMapper.Mapper.Map<TDestination>(firstSource);

            sources.ToList().Skip(1).ForEach(source => MapWithoutType(source, destination));

            return destination;
        }

        public void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            AutoMapper.Mapper.Map(source, destination);
        }

        private void MapWithoutType(object source, object destination)
        {
            AutoMapper.Mapper.Map(source, destination, source.GetType(), destination.GetType());
        }
    }
}
