using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Domain.Contracts
{
    public interface IMongoConfigurationFactory
    {
        IMongoConfiguration Create();
    }
}
