using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MongoDB.Driver;

namespace MongoEntityFramework.AspNetCore.Identity.Tests
{
    static class TestConfiguration
    {
        public static string ConnectionString => Environment.GetEnvironmentVariable("MONGODB_URI") ?? "mongodb://localhost";

        public static DbContextOptions GetConnection(string databaseName)
        {
            return new DbContextOptionsBuilder().UseMongoDB(ConnectionString, databaseName).ConfigureWarnings(x => x.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)).Options;
        }
    }
}
