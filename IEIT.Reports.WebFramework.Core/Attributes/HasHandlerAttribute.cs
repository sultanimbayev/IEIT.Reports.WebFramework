using IEIT.Reports.WebFramework.Core.Interfaces;
using System;

namespace IEIT.Reports.WebFramework.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HasHandlerAttribute : Attribute
    {
        public string HandlerName { get; private set; }

        public HasHandlerAttribute(string handlerName)
        {
            HandlerName = handlerName;
        }

        public HasHandlerAttribute(Type handlerType)
        {
            var interfaceName = typeof(IHandler).Name;
            if (handlerType.GetInterface(interfaceName) == null) { throw new Exception($"Аттрибут HasHandler применим только для классов типа {interfaceName}"); };
            HandlerName = handlerType.Name;
        }
    }
}