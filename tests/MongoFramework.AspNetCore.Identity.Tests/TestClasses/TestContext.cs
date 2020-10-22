namespace MongoFramework.AspNetCore.Identity.Tests.TestClasses
{
	public class TestContext : MongoDbContext
	{
		public TestContext(IMongoDbConnection connection) : base(connection) { }
		public MongoDbSet<TestUser> TestUsers { get; set; }
		public MongoDbSet<TestUserInt> TestUsersInt { get; set; }
	}
}