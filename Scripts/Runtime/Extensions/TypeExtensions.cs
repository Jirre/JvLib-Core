using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    public static partial class TypeExtensions
    {
        private static readonly Dictionary<Type, IList<Type>> TYPE_TO_SUB_CLASSES
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
            return type.IsArray || 
                   type.IsList();
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
            if (useCache && TYPE_TO_SUB_CLASSES.TryGetValue(baseType, out result)) return result;
            
            result = FindAllSubclassesInternal(baseType, useCache).AsReadOnly();
            TYPE_TO_SUB_CLASSES[baseType] = result;

            return result;
        }

        private static List<Type> FindAllSubclassesInternal(Type baseType, bool useCache)
        {
            List<Type> result = new List<Type>();

            if (baseType == null)
                return result;

            IList<Type> types = AppDomain.CurrentDomain.GetAllTypes(useCache);
            foreach (Type type in types)
            {
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
            foreach (Type getType in types)
            {
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

                if (type != null)
                {
                    FieldInfo[] newFields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                
                    fields.Capacity += newFields.Length;
                    foreach (FieldInfo field in newFields)
                    {
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

                if (type != null)
                {
                    PropertyInfo[] newProperties
                        = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (includeObsolete)
                    {
                        properties.AddRange(newProperties);
                    }
                    else
                    {
                        properties.Capacity += newProperties.Length;
                        foreach (PropertyInfo property in newProperties)
                        {
                            if (property.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length == 0)
                            {
                                properties.Add(property);
                            }
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
            foreach (FieldInfo t in fields)
            {
                values.Add((FieldType)t.GetValue(instance));
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

            return declaringField == null ? null : GetFieldName(type, declaringField, capitalize);
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
