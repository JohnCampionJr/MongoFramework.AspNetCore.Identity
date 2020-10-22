using System;
using MongoDB.Driver;

namespace MongoFramework.AspNetCore.Identity.Tests
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
