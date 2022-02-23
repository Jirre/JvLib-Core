using System.Reflection;

namespace JvLib.Utilities
{
    public enum EFieldPropertyType
    {
        Field,
        Property
    }

    public class FieldOrPropertyInfo
    {
        private readonly PropertyInfo propertyInfo;
        private readonly FieldInfo fieldInfo;
        private readonly string name;

        public FieldOrPropertyInfo(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
            this.fieldInfo = null;
            this.name = propertyInfo.Name.ToLowerInvariant();
        }

        public FieldOrPropertyInfo(FieldInfo fieldInfo)
        {
            this.propertyInfo = null;
            this.fieldInfo = fieldInfo;
            this.name = fieldInfo.Name.ToLowerInvariant();

        }

        public string DisplayName
        {
            get
            {
                if (propertyInfo != null)
                    return StringUtility.GetHumanReadableText(propertyInfo.Name);
                return StringUtility.GetHumanReadableText(fieldInfo.Name);
            }
        }

        public object GetValue(object target)
        {
            if (propertyInfo != null)
                return propertyInfo.GetValue(target, null);

            return fieldInfo.GetValue(target);
        }

        public static implicit operator FieldOrPropertyInfo(PropertyInfo propertyInfo)
        {
            return new FieldOrPropertyInfo(propertyInfo);
        }

        public static implicit operator FieldOrPropertyInfo(FieldInfo fieldInfo)
        {
            return new FieldOrPropertyInfo(fieldInfo);
        }

        protected bool Equals(FieldOrPropertyInfo other)
        {
            return string.Equals(name, other.name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FieldOrPropertyInfo)obj);
        }

        public override int GetHashCode()
        {
            return (name != null ? name.GetHashCode() : 0);
        }
    }
}