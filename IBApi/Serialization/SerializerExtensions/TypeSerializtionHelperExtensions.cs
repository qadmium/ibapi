using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace IBApi.Serialization.SerializerExtensions
{
    internal static class TypeSerializtionHelperExtensions
    {
        public static bool ShouldSerializeAsDateTime(this FieldInfo field)
        {
            Contract.Requires(field != null);
            return field.FieldType == typeof(DateTime) || field.FieldType == typeof(DateTime?);
        }
            
        public static bool ShouldSerializeAsBool(this FieldInfo field)
        {
            Contract.Requires(field != null);
            return field.FieldType == typeof(bool) || field.FieldType == typeof(bool?);
        }

        public static bool ShouldSerializeAsEnum(this FieldInfo field)
        {
            Contract.Requires(field != null);
            return field.FieldType.IsEnum || 
                (Nullable.GetUnderlyingType(field.FieldType) != null && Nullable.GetUnderlyingType(field.FieldType).IsEnum);
        }

        public static bool ShouldSerializeAsEnumerable(this FieldInfo field)
        {
            return typeof(IEnumerable).IsAssignableFrom(field.FieldType)
                && !typeof(string).IsAssignableFrom(field.FieldType);
        }

        public static bool ShouldSerializeForThisObject(this FieldInfo field, object obj)
        {
            Contract.Requires(field != null);
            Contract.Requires(obj != null);

            var method = obj.GetType().GetMethod("ShouldSerialize" + field.Name);

            if (method == null)
            {
                return true;
            }

            return (bool)method.Invoke(obj, null);
        }

        public static IEnumerable<FieldInfo> GetSerializableFields(this IReflect forType)
        {
            Contract.Requires(forType != null);
            return forType.GetFields(BindingFlags.Instance | BindingFlags.Public).Where(fieldInfo => !fieldInfo.IsNotSerialized);
        }
    }
}