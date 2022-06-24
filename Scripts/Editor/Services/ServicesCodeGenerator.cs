using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.Compilation;
using JvLib.Editor.Utilities;
using JvLib.Services;

namespace JvLib.Editor.Services
{
    public static class ServicesCodeGenerator
    {
        private const string SERVICES_CLASS_NAME = "Svc";
        private const string REFERENCES_CLASS_NAME = "Ref";

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            GenerateServicesClass();
        }

        private static int CompareTypes(Type a, Type b)
        {
            return string.Compare(a.Name, b.Name, StringComparison.Ordinal);
        }

        [MenuItem("Tools/JvLib/Code Generation/Generate Services")]
        private static void GenerateServicesClass()
        {
            IList<Type> allTypes = AppDomain.CurrentDomain.GetAllTypes(AssembliesType.Player);
            if (allTypes == null)
                return;

            List<Type> serviceTypes = new List<Type>(allTypes);

            for (int i = serviceTypes.Count - 1; i >= 0; i--)
            {
                Type type = serviceTypes[i];

                if (type.IsValueType || type.IsEnum
                                     || type.GetCustomAttributes(typeof(ServiceInterfaceAttribute), false).Length == 0)
                {
                    serviceTypes.RemoveAt(i);
                }
            }

            if (serviceTypes.Count == 0) // Fix for some cases where an inappropriate empty Svc class is generated
                return;

            serviceTypes.Sort(CompareTypes);

            if (!Directory.Exists(AssetGenerationUtility.GeneratedCodeFolder))
                Directory.CreateDirectory(AssetGenerationUtility.GeneratedCodeFolder);

            string assetPath = Path.Combine(AssetGenerationUtility.GeneratedCodeFolder, SERVICES_CLASS_NAME + ".cs");
            string currentFileContents = "";

            try
            {
                currentFileContents = File.ReadAllText(assetPath, Encoding.UTF8);
            }
            catch
            {
                // ignored
            }

            StringBuilder output = new StringBuilder();
            const string referenceType = "ServiceReference";

            output.AppendCode("using UnityEngine;");
            output.AppendCode();
            output.AppendCode("namespace JvLib.Services");
            output.AppendCode("{");
            output.AppendCode("    public static class " + SERVICES_CLASS_NAME);
            output.AppendCode("    {");
            output.AppendCode("        public static class " + REFERENCES_CLASS_NAME);
            output.AppendCode("        {");
            foreach (Type serviceType in serviceTypes)
            {
                string serviceTypeFullName = GetServiceTypeFullName(serviceType);

                output.AppendCode("            public static " + referenceType + "<"
                                  + serviceTypeFullName + "> " + GetIdentifier(serviceType));
                output.AppendCode("                 = new " + referenceType + "<"
                                  + serviceTypeFullName + ">();");
            }

            output.AppendCode("        }");

            output.AppendCode();
            foreach (Type serviceType in serviceTypes)
            {
                object[] customAttributes
                    = serviceType.GetCustomAttributes(typeof(ServiceInterfaceAttribute), false);
                ServiceInterfaceAttribute serviceInterfaceAttribute =
                    (ServiceInterfaceAttribute) customAttributes[0];
                string serviceTypeFullName = serviceType.FullName;
                if (serviceInterfaceAttribute.Interface != null)
                {
                    serviceTypeFullName = serviceInterfaceAttribute.Interface.FullName;
                }

                output.AppendCode("        public static "
                                  + serviceTypeFullName + " " + GetIdentifier(serviceType));
                output.AppendCode("        {");
                output.AppendCode("            get");
                output.AppendCode("            {");
                output.AppendCode("                return " + REFERENCES_CLASS_NAME + "."
                                  + GetIdentifier(serviceType)
                                  + ".Reference;");
                output.AppendCode("            }");
                output.AppendCode("        }");
            }

            output.AppendCode();

            output.AppendCode(
                "        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]");
            output.AppendCode("        private static void ClearCache()");
            output.AppendCode("        {");
            foreach (Type serviceType in serviceTypes)
            {
                string serviceTypeFullName = GetServiceTypeFullName(serviceType);
                output.AppendCode("            " + REFERENCES_CLASS_NAME + "." + GetIdentifier(serviceType)
                                  + " = new " + referenceType + "<" + serviceTypeFullName + ">();");
            }

            output.AppendCode("        }");

            output.AppendCode("    }");

            output.AppendCode("}");
            output.AppendCode();

            string newFileContents = output.ToString();

            if (currentFileContents != newFileContents)
            {
                AssetDatabase.ReleaseCachedFileHandles();
                File.WriteAllText(assetPath, newFileContents, Encoding.UTF8);
            }
        }

        private static string GetServiceTypeFullName(Type serviceType)
        {
            object[] customAttributes
                = serviceType.GetCustomAttributes(typeof(ServiceInterfaceAttribute), false);
            ServiceInterfaceAttribute serviceInterfaceAttribute =
                (ServiceInterfaceAttribute) customAttributes[0];

            string serviceTypeFullName = serviceType.FullName;
            if (serviceInterfaceAttribute.Interface != null)
            {
                serviceTypeFullName = serviceInterfaceAttribute.Interface.FullName;
            }

            return serviceTypeFullName;
        }

        private static string GetIdentifier(Type type)
        {
            object[] customAttributes
                = type.GetCustomAttributes(typeof(ServiceInterfaceAttribute), false);
            ServiceInterfaceAttribute serviceInterfaceAttribute =
                (ServiceInterfaceAttribute) customAttributes[0];

            if (!string.IsNullOrEmpty(serviceInterfaceAttribute.Name))
                return serviceInterfaceAttribute.Name;

            string name = type.Name;

            const string serviceStr = "Service";
            if (name.EndsWith(serviceStr))
            {
                name = name.Substring(0, name.Length - serviceStr.Length);
            }

            if (type.IsInterface)
            {
                if (name.StartsWith("I"))
                {
                    if (char.IsUpper(name[1]))
                    {
                        name = name.Substring(1);
                    }
                }
            }

            return name.FirstToUpper();
        }
    }
}
