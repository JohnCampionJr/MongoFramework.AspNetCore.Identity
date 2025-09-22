using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoFramework.AspNetCore.Identity
{
    namespace MongoFramework
    {
        public interface IMongoDbConnection : IDisposable
        {
            IMongoClient Client { get; }
            IMongoDatabase GetDatabase();
        }
    }
}
