namespace MongoEntityFramework.AspNetCore.Identity.Tests.TestClasses
{
    public class TestUserInt : MongoIdentityUser<int>
    {
        public string CustomData { get; set; }

        public static TestUserInt First => new TestUserInt
        {
            Id = 1000,
            Email = "test1@testing.com",
            UserName = "User Name1",
            CustomData = "Some Info 1",
            NormalizedEmail = "TEST1@TESTING.COM",
            NormalizedUserName = "USER NAME1"
        };
        public static TestUserInt Second => new TestUserInt
        {
            Id = 2000,
            Email = "test2@testing.com",
            UserName = "User Name2",
            CustomData = "Some Info 2",
            NormalizedEmail = "TEST2@TESTING.COM",
            NormalizedUserName = "USER NAME2"
        };
        public static TestUserInt Third => new TestUserInt
        {
            Id = 3000,
            Email = "test3@testing.com",
            UserName = "User Name3",
            CustomData = "Some Info 3",
            NormalizedEmail = "TEST3@TESTING.COM",
            NormalizedUserName = "USER NAME3"
        };
    }
}