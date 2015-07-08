﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Forum.Domain.Context;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using MongoDB.Driver;
using SimpleInjector;

namespace Forum.IntegrationTests
{
    public class Context
    {
        public Container Container { get; set; }
        public IMongoContext MongoContext { get; set; }

        public Context()
        {
            Container = new Container();

            TestContainer.RegisterDependencies(Container);

            Container.Verify();

            MongoContext = Container.GetInstance<IMongoContext>();
        }
    }
}
