using System.Linq;

namespace System.Reflection
{
    public static partial class MemberInfoExtensions
    {
        public static T GetAttribute<T>(this MemberInfo memberInfo, bool inherit = true)
            where T : Attribute
        {
            object[] attributes = memberInfo.GetCustomAttributes(inherit);
            return attributes.OfType<T>().FirstOrDefault();
        }
    }
}

