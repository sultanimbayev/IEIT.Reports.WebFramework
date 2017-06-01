using System;

namespace IEIT.Reports.WebFramework.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HasHandlerAttribute : Attribute
    {
        public Type HandlerType { get; private set; }
        public HasHandlerAttribute(Type handlerType)
        {
            if (handlerType.GetInterface("IHandler") == null) { throw new Exception("Аттрибут HasHandler применим только для классов типа IHandler"); };
            HandlerType = handlerType;
        }
    }
}