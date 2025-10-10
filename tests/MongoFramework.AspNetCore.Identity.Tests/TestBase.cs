using System;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace MongoEntityFramework.AspNetCore.Identity.Tests
{
    public abstract class TestBase : IDisposable
    {
        private readonly string _databaseName;
        private readonly bool _clearOnDispose;

        protected TestBase(string databaseName, bool clearDatabaseOnDispose = true)
        {
            _databaseName = databaseName;
            _clearOnDispose = clearDatabaseOnDispose;
            ClearDatabase();
        }

        private void ClearDatabase()
        {
            //Removing the database created for the tests
            var client = new MongoClient(TestConfiguration.ConnectionString);
            client.DropDatabase(_databaseName);
        }

        protected DbContextOptions GetConnection() => TestConfiguration.GetConnection(_databaseName);

        public void Dispose()
        {
            if (_clearOnDispose)
            {
                ClearDatabase();
            }
        }
    }
}
