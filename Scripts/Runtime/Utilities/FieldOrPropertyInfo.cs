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
        private readonly PropertyInfo _propertyInfo;
        private readonly FieldInfo _fieldInfo;
        private readonly string _name;

        public FieldOrPropertyInfo(PropertyInfo pPropertyInfo)
        {
            _propertyInfo = pPropertyInfo;
            _fieldInfo = null;
            _name = pPropertyInfo.Name.ToLowerInvariant();
        }

        public FieldOrPropertyInfo(FieldInfo pFieldInfo)
        {
            _propertyInfo = null;
            _fieldInfo = pFieldInfo;
            _name = pFieldInfo.Name.ToLowerInvariant();

        }

        public string DisplayName
        {
            get
            {
                if (_propertyInfo != null)
                    return StringUtility.GetHumanReadableText(_propertyInfo.Name);
                return StringUtility.GetHumanReadableText(_fieldInfo.Name);
            }
        }

        public object GetValue(object target)
        {
            return _propertyInfo != null ? 
                _propertyInfo.GetValue(target, null) : 
                _fieldInfo.GetValue(target);
        }

        public static implicit operator FieldOrPropertyInfo(PropertyInfo pPropertyInfo)
        {
            return new FieldOrPropertyInfo(pPropertyInfo);
        }

        public static implicit operator FieldOrPropertyInfo(FieldInfo pFieldInfo)
        {
            return new FieldOrPropertyInfo(pFieldInfo);
        }

        protected bool Equals(FieldOrPropertyInfo pOther)
        {
            return string.Equals(_name, pOther._name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && 
                   Equals((FieldOrPropertyInfo)obj);
        }

        public override int GetHashCode()
        {
            return _name != null ? 
                _name.GetHashCode() : 
                0;
        }
    }
}