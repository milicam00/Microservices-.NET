using System.Reflection;
using UserAccess.Infrastructure.Configuration.Commands;

namespace UserAccess.Infrastructure.Configuration
{
    public static class Assemblies
    {
        public static readonly Assembly Application = typeof(InternalCommandBase).Assembly;
    }
}
