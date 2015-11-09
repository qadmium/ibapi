using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace IBApi.Serialization.SerializerExtensions
{
    internal static class DeserializationExtensions
    {
        public static async Task<int> ReadTypeIdFromStream(this FieldsStream stream, CancellationToken cancellationToken)
        {
            var typeIdString = await stream.ReadNextField(cancellationToken);

            var typeId = int.Parse(typeIdString, CultureInfo.InvariantCulture);
            return typeId;
        }

        public static async Task<object> ReadObject(this FieldsStream stream, Type type,
            CancellationToken cancellationToken)
        {
            if (type.IsPrimitive)
            {
                return await stream.ReadPrimitive(type, cancellationToken);
            }

            var result = Activator.CreateInstance(type);

            foreach (
                var serializableField in
                    type.GetSerializableFields()
                        .Where(serializableField => serializableField.ShouldSerializeForThisObject(result)))
            {
                var propertyValue = await stream.ReadFieldValue(serializableField, cancellationToken);
                serializableField.SetValue(result, propertyValue);
            }

            return result;
        }

        private static async Task<object> ReadPrimitive(this FieldsStream stream, Type what,
            CancellationToken cancellationToken)
        {
            var str = await stream.ReadNextField(cancellationToken);
            return TypeDescriptor.GetConverter(what).ConvertFromString(null, CultureInfo.InvariantCulture, str);
        }

        private static async Task<object> ReadFieldValue(this FieldsStream stream, FieldInfo field,
            CancellationToken cancellationToken)
        {
            if (field.ShouldSerializeAsEnumerable())
            {
                return await stream.ReadEnumerable(field.FieldType.GetGenericArguments().Single(), cancellationToken);
            }

            if (field.ShouldSerializeAsBool())
            {
                return await stream.ReadBool(cancellationToken);
            }

            if (field.ShouldSerializeAsDateTime())
            {
                return await stream.ReadDateTime(cancellationToken);
            }

            if (field.ShouldSerializeAsEnum())
            {
                return await stream.ReadEnum(field.FieldType, cancellationToken);
            }

            return await stream.ReadPrimitive(field.FieldType, cancellationToken);
        }

        private static async Task<object> ReadDateTime(this FieldsStream stream, CancellationToken cancellationToken)
        {
            var fieldValue = await stream.ReadNextField(cancellationToken);

            if (string.IsNullOrEmpty(fieldValue))
            {
                return null;
            }

            return DateTime.ParseExact(fieldValue, "yyyyMMdd", CultureInfo.InvariantCulture);
        }

        private static async Task<bool?> ReadBool(this FieldsStream stream, CancellationToken cancellationToken)
        {
            var fieldValue = await stream.ReadNextField(cancellationToken);

            return fieldValue == "0" ? false : fieldValue == "1" ? true : (bool?) null;
        }

        private static async Task<object> ReadEnum(this FieldsStream stream, Type type,
            CancellationToken cancellationToken)
        {
            return Enum.ToObject(type, await stream.ReadPrimitive(typeof (int), cancellationToken));
        }

        private static async Task<object> ReadEnumerable(this FieldsStream stream, Type enumerableType,
            CancellationToken cancellationToken)
        {
            var size = int.Parse(await stream.ReadNextField(cancellationToken), CultureInfo.InvariantCulture);
            var result = (IList) Activator.CreateInstance(typeof (List<>).MakeGenericType(enumerableType));

            for (var i = 0; i < size; i++)
            {
                result.Add(await stream.ReadObject(enumerableType, cancellationToken));
            }

            return result;
        }
    }
}