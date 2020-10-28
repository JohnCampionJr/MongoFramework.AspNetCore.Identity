using MongoFramework.Infrastructure.Diagnostics;

// ReSharper disable once CheckNamespace
namespace MongoFramework
{
    public class MongoDbContextOptions
    {
        public string ConnectionString { get; set; }
        public IDiagnosticListener DiagnosticListener { get; set; }
    }
}
