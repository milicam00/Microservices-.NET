using Catalog.Infrastructure.Configuration.Commands;
using System.Reflection;

namespace Catalog.Infrastructure.Configuration
{
    public static class Assemblies
    {
        public static readonly Assembly Application = typeof(InternalCommandBase).Assembly;
    }
}
