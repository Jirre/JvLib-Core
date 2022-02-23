using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    public static partial class TypeExtensions
    {
        private static readonly Dictionary<Type, IList<Type>> typeToSubClasses
            = new Dictionary<Type, IList<Type>>();

        public static AttributeClass GetAttribute<AttributeClass>(this Type type, bool inherit = false)
            where AttributeClass : Attribute
        {
            object[] attributes = type.GetCustomAttributes(typeof(AttributeClass), inherit);

            if (attributes.Length > 0)
                return attributes[0] as AttributeClass;

            return null;
        }

        public static bool IsArrayOrList(this Type type)
        {
            if (type.IsArray)
                return true;

            if (type.IsList())
                return true;

            return false;
        }

        public static bool IsList(this Type type)
        {
            if (type.IsArray)
                return false;

            if (type.IsGenericType &&
                typeof(List<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
                return true;

            if (type.IsGenericType &&
                typeof(IList<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
                return true;

            return false;
        }

        public static Type GetArrayOrListType(this Type type)
        {
            if (type.IsArray)
                return type.GetElementType();

            if (type.IsList())
                return type.GetGenericArguments()[0];

            return null;
        }

        public static IList<Type> FindAllSubclasses(this Type baseType, bool useCache = true)
        {
            IList<Type> result;
            if (!useCache || !typeToSubClasses.TryGetValue(baseType, out result))
            {
                result = FindAllSubclassesInternal(baseType, useCache).AsReadOnly();
                typeToSubClasses[baseType] = result;
            }

            return result;
        }

        private static List<Type> FindAllSubclassesInternal(Type baseType, bool useCache)
        {
            List<Type> result = new List<Type>();

            if (baseType == null)
                return result;

            IList<Type> types = AppDomain.CurrentDomain.GetAllTypes(useCache);
            for (int i = 0; i < types.Count; i++)
            {
                Type type = types[i];
                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(baseType))
                {
                    result.Add(type);
                }
            }
            return result;
        }

        public static List<Type> FindAllAssignableTypes(this Type type, bool includeAbstract = true)
        {
            List<Type> result = new List<Type>();
            IList<Type> types = AppDomain.CurrentDomain.GetAllTypes();
            for (int j = 0; j < types.Count; j++)
            {
                Type getType = types[j];
                if (type.IsAssignableFrom(getType) && getType != type
                    && (includeAbstract || !getType.IsAbstract))
                {
                    result.Add(getType);
                }
            }
            return result;
        }


        public static FieldInfo GetFieldInHierarchy(this Type type, string fieldName)
        {
            while (type != null)
            {
                FieldInfo field = type.GetField(fieldName,
                    BindingFlags.Public |
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

                if (field != null)
                    return field;

                type = type.BaseType;
            }
            return null;
        }

        public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                Type cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        public static List<FieldInfo> GetFieldsUpUntilBaseClass<TBaseClass>(
            this Type type, bool includeBaseClass = true, bool includeObsolete = true, bool cullDuplicates = false)
        {
            List<FieldInfo> fields = new List<FieldInfo>();
            while (typeof(TBaseClass).IsAssignableFrom(type))
            {
                if (type == typeof(TBaseClass) && !includeBaseClass)
                    break;

                FieldInfo[] newFields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                
                fields.Capacity += newFields.Length;
                for (int i = 0; i < newFields.Length; i++)
                {
                    FieldInfo field = newFields[i];
                    
                    // If requested, filter out duplicates.
                    if (cullDuplicates)
                    {
                        bool isCulledDuplicate = false;
                        for (int j = 0; j < fields.Count; j++)
                        {
                            if (fields[j].Name == field.Name)
                            {
                                isCulledDuplicate = true;
                                break;
                            }
                        }
                        if (isCulledDuplicate)
                            continue;
                    }
                    
                    if (includeObsolete || field.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length == 0)
                        fields.Add(field);
                }

                type = type.BaseType;
            }
            return fields;
        }

        public static List<PropertyInfo> GetPropertiesUpUntilBaseClass<TBaseClass>(
            this Type type, bool includeBaseClass = true, bool includeObsolete = true)
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();
            while (typeof(TBaseClass).IsAssignableFrom(type))
            {
                if (type == typeof(TBaseClass) && !includeBaseClass)
                    break;

                PropertyInfo[] newProperties
                    = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (includeObsolete)
                {
                    properties.AddRange(newProperties);
                }
                else
                {
                    properties.Capacity += newProperties.Length;
                    for (int i = 0; i < newProperties.Length; i++)
                    {
                        PropertyInfo property = newProperties[i];
                        if (property.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length == 0)
                        {
                            properties.Add(property);
                        }
                    }
                }

                type = type.BaseType;

                if (type == typeof(TBaseClass) && includeBaseClass)
                    break;
            }
            return properties;
        }

        public static List<FieldInfo> GetFieldsUpUntilBaseClass<TBaseClass, TFieldType>(
            this Type type, bool includeBaseClass = true)
        {
            List<FieldInfo> fields = GetFieldsUpUntilBaseClass<TBaseClass>(type, includeBaseClass);

            for (int i = fields.Count - 1; i >= 0; i--)
            {
                if (!typeof(TFieldType).IsAssignableFrom(fields[i].FieldType))
                    fields.RemoveAt(i);
            }
            return fields;
        }

        public static List<FieldType> GetFieldValuesUpUntilBaseClass<BaseClass, FieldType>(
            this Type type, object instance, bool includeBaseClass = true)
        {
            List<FieldInfo> fields = GetFieldsUpUntilBaseClass<BaseClass, FieldType>(
                type, includeBaseClass);

            List<FieldType> values = new List<FieldType>();
            for (int i = 0; i < fields.Count; i++)
            {
                values.Add((FieldType)fields[i].GetValue(instance));
            }
            return values;
        }

        public static FieldInfo GetDeclaringFieldUpUntilBaseClass<TBaseClass, TFieldType>(
            this Type type, object instance, TFieldType value, bool includeBaseClass = true)
        {
            List<FieldInfo> fields = GetFieldsUpUntilBaseClass<TBaseClass, TFieldType>(
                type, includeBaseClass);

            TFieldType fieldValue;
            for (int i = 0; i < fields.Count; i++)
            {
                fieldValue = (TFieldType)fields[i].GetValue(instance);
                if (Equals(fieldValue, value))
                    return fields[i];
            }

            return null;
        }

        public static string GetNameOfDeclaringField<TBaseClass, TFieldType>(
            this Type type, object instance, TFieldType value, bool capitalize = false)
        {
            FieldInfo declaringField = type
                .GetDeclaringFieldUpUntilBaseClass<TBaseClass, TFieldType>(instance, value);

            if (declaringField == null)
                return null;

            return GetFieldName(type, declaringField, capitalize);
        }

        public static string GetFieldName(this Type type, FieldInfo fieldInfo, bool capitalize = false)
        {
            string name = fieldInfo.Name;

            if (!capitalize)
                return name;

            if (name.Length <= 1)
                return name.ToUpper();

            return char.ToUpper(name[0]) + name.Substring(1);
        }

        public static Type[] GetAllAssignableClasses(this Type type, bool includeAbstract = true)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => type.IsAssignableFrom(t) && t != type
                && (includeAbstract || !t.IsAbstract)).ToArray();
        }

        public static MethodInfo GetMethodIncludingFromBaseClasses(this Type type, string name)
        {
            MethodInfo methodInfo = null;
            Type baseType = type;
            while (methodInfo == null)
            {
                methodInfo = baseType.GetMethod(name,
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                if (methodInfo != null)
                    return methodInfo;

                baseType = baseType.BaseType;
                if (baseType == null)
                    break;
            }

            return null;
        }

    }
}
