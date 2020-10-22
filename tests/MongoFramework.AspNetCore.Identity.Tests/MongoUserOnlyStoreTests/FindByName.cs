using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoFramework.AspNetCore.Identity.Tests.TestClasses;
using Shouldly;
using Xunit;

namespace MongoFramework.AspNetCore.Identity.Tests.MongoUserOnlyStoreTests
{
	public class FindByName : TestBase, IAsyncLifetime
	{

		public FindByName() : base("MongoUserOnlyStore-FindByName", false) { }

		public async Task InitializeAsync()
		{
			var context = new TestContext(GetConnection());
			var store = new MongoUserOnlyStore<TestUser>(context);

			await store.CreateAsync(TestUser.First);
			await store.CreateAsync(TestUser.Second);
			await store.CreateAsync(TestUser.Third);
		}

		public Task DisposeAsync() => Task.CompletedTask;

		[Fact]
		public async Task FindsCorrectUserWithValidUserName()
		{
			var context = new TestContext(GetConnection());
			var store = new MongoUserOnlyStore<TestUser>(context);

			var result = await store.FindByNameAsync("USER NAME2");

			result.ShouldNotBeNull();
			result.UserName.ShouldBe("User Name2");
		}

		[Fact]
		public async Task ReturnsNullWithInvalidUserName()
		{
			var context = new TestContext(GetConnection());
			var store = new MongoUserOnlyStore<TestUser>(context);

			var result = await store.FindByNameAsync("none");

			result.ShouldBeNull();
		}

		[Fact]
		public async Task ThrowsExceptionWithNull()
		{
			var context = new TestContext(GetConnection());
			var store = new MongoUserOnlyStore<TestUser>(context);

			await Should.ThrowAsync<ArgumentNullException>( async () =>
			{
				await store.FindByNameAsync(null);
			});
		}

	}}