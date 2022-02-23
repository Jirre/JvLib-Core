using System.Collections.Generic;

namespace System.Reflection
{
    public static class AppDomainExtensions
    {
        private static IList<Type> cachedTypes;

        public static IList<Type> GetAllTypes(this AppDomain appDomain, bool mayUseCache = true)
        {
            if (mayUseCache && cachedTypes != null)
                return cachedTypes;

            List<Type> result = new List<Type>();
            Assembly[] assemblies = appDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];
                result.AddRange(assembly.GetLoadableTypes());
            }
            cachedTypes = result.AsReadOnly();
            return cachedTypes;
        }
    }
}
