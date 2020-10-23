using System;
using MongoDB.Driver;
using MongoFramework;

namespace IdentityCore.Tests
{
	static class TestConfiguration
	{
		public static string ConnectionString => Environment.GetEnvironmentVariable("MONGODB_URI") ?? "mongodb://localhost";

		public static IMongoDbConnection GetConnection(string databaseName)
		{
			var urlBuilder = new MongoUrlBuilder(ConnectionString)
			{
				DatabaseName = databaseName
			};
			return MongoDbConnection.FromUrl(urlBuilder.ToMongoUrl());
		}
	}
}
