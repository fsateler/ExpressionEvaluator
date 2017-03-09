using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExpressionEvaluator
{
    static partial class ExpressionEvaluator
    {
        // Try to perform only the minimal conversions allowed
        static object ConvertObject(object instance, Type destType, Type origType, bool asConversion)
        {
            Type underlying = Nullable.GetUnderlyingType(destType);
            if (instance == null) {
                bool isNullable = underlying != null || !destType.GetTypeInfo().IsValueType;
                if (isNullable) {
                    return null;
                }
                else {
#pragma warning disable S112 // General exceptions should never be thrown
                    // we are emulating the runtime here...
                    throw new NullReferenceException();
#pragma warning restore S112 // General exceptions should never be thrown
                }
            }
            var instanceType = instance.GetType();
            // Short circuit easy case
            if (instanceType == (underlying ?? destType)) {
                return instance;
            }
            if ((underlying ?? destType).GetTypeInfo().IsValueType) {
                if (asConversion && underlying != null) {
                    return null;
                }
                else if (origType != instanceType) {
                    // (short)(int)1 is allowed
                    // (short)(object)1 is not
                    throw new InvalidCastException($"Unable to cast object of type '{origType.FullName}' to '{(underlying ?? destType).FullName}");
                }
            }
            if (destType.GetTypeInfo().IsInstanceOfType(instance)) {
                return instance;
            }
            try {
                return DoCast(instance, underlying ?? destType);
            }
            catch (InvalidCastException) when (asConversion) {
                return null;
            }
        }

        private static object DoCast(object instance, Type destType)
        {
            // special case for strings as the converter could try to parse/convert to it
            if (destType == typeof(string) || instance is string) {
                throw new InvalidCastException();
            }
            return Convert.ChangeType(instance, destType);
        }
    }
}
