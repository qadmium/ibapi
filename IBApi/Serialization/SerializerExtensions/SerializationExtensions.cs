﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Messages;

namespace IBApi.Serialization.SerializerExtensions
{
    internal static class SerializationExtensions
    {
        public static async Task Serialize(this IMessage message, FieldsStream stream, CancellationToken cancellationToken)
        {
            await stream.Write(message.SerializeObject(), cancellationToken);
        }

        private static byte[] SerializeObject(this object obj)
        {
            if (obj.GetType().IsPrimitive)
            {
                return obj.SerializePrimitive();
            }

            return obj.SerializeTypeId().Concat(
                obj.GetType().GetSerializableFields()
                    .Where(field => field.ShouldSerializeForThisObject(obj))
                    .SelectMany(field => field.SerializeField(obj))).ToArray();
        }
        private static byte[] SerializePrimitive(this object obj)
        {
            if (obj == null)
            {
                return EmptyString();
            }

            return Encoding.ASCII.GetBytes(string.Format(CultureInfo.InvariantCulture, "{0}", obj) + char.MinValue);
        }

        private static byte[] SerializeTypeId(this object obj)
        {
            if (!Attribute.IsDefined(obj.GetType(), typeof(IbSerializable)))
            {
                return NoValue();
            }

            var typeIdAttribute = obj.GetType().GetCustomAttributes(false).OfType<IbSerializable>().Single();
            var buffer = typeIdAttribute.IbTypeId.SerializePrimitive();
            return buffer;
        }

        private static byte[] SerializeField(this FieldInfo field, object obj)
        {
            if (field.ShouldSerializeAsEnumerable())
            {
                return SerializeEnumerable(field.GetValue(obj));
            }
            
            if (field.ShouldSerializeAsBool())
            {
                return SerializeAsBool(field.GetValue(obj));
            }

            if (field.ShouldSerializeAsDateTime())
            {
                return SerializeAsDateTime(field.GetValue(obj));
            }

            if (field.ShouldSerializeAsEnum())
            {
                return SerializeAsEnum(field.GetValue(obj));
            }

            return SerializePrimitive(field.GetValue(obj));
        }

        private static byte[] SerializeAsEnum(object value)
        {
            if (value == null)
            {
                return EmptyString();
            }

            return SerializePrimitive((int) value);
        }

        private static byte[] SerializeEnumerable(object value)
        {
            if (value == null)
            {
                return 0.SerializePrimitive();
            }

            IEnumerable<byte> result = new List<byte>();

            var serializable = (IEnumerable)value;

            var count = 0;

            foreach (var item in serializable)
            {
                result = result.Concat(item.SerializeObject());
                count++;
            }

            return count.SerializePrimitive().Concat(result).ToArray();
        }

        private static byte[] SerializeAsBool(object value)
        {
            if (value == null)
            {
                return EmptyString();
            }

            return (bool)value ? SerializePrimitive("1") : SerializePrimitive("0");
        }

        private static byte[] SerializeAsDateTime(object value)
        {
            if (value == null)
            {
                return EmptyString();
            }

            var dateTimValue = (DateTime)value;

            return SerializePrimitive(dateTimValue.ToString("yyyyMMdd"));
        }

        private static byte[] EmptyString()
        {
            return Encoding.ASCII.GetBytes(string.Format("{0}", char.MinValue));
        }

        private static byte[] NoValue()
        {
            return new byte[]{};
        }
    }
}