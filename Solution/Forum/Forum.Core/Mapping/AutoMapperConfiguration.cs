using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Forum.Core.Mapping
{
    public class AutoMapperConfiguration
    {
        public static void Configure(IEnumerable<Profile> profiles)
        {
            AutoMapper.Mapper.Initialize(x => BuildAutoMapperConfiguration(AutoMapper.Mapper.Configuration, profiles));
        }

        private static void BuildAutoMapperConfiguration(IConfiguration configuration, IEnumerable<Profile> profiles)
        {
            profiles.ToList().ForEach(configuration.AddProfile);
        }
    }
}
