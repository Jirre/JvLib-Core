using System.Collections.Generic;

namespace System.Reflection
{
    public static class AppDomainExtensions
    {
        private static IList<Type> _cachedTypes;

        public static IList<Type> GetAllTypes(this AppDomain appDomain, bool mayUseCache = true)
        {
            if (mayUseCache && _cachedTypes != null)
                return _cachedTypes;

            List<Type> result = new List<Type>();
            Assembly[] assemblies = appDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];
                result.AddRange(assembly.GetLoadableTypes());
            }
            _cachedTypes = result.AsReadOnly();
            return _cachedTypes;
        }
    }
}
