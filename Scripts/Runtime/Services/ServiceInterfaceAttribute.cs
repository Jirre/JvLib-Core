using System;

namespace JvLib.Services
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class ServiceInterfaceAttribute : Attribute
    {
        public string Name { get; set; }
        public Type Interface { get; set; }
    }
}
