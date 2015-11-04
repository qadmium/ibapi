using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IBApi.Serialization.SerializerExtensions
{
    internal static class DeserializationExtensions
    {
        public static int ReadTypeIdFromStream(this Stream stream)
        {
            var typeIdString = stream.ReadUntilNull();

            var typeId = int.Parse(typeIdString, CultureInfo.InvariantCulture);
            return typeId;
        }

        public static object ReadObject(this Stream stream, Type type)
        {
            if (type.IsPrimitive)
            {
                return stream.ReadPrimitive(type);
            }

            var result = Activator.CreateInstance(type);

            foreach (var serializableField in type.GetSerializableFields())
            {
                if (serializableField.ShouldSerializeForThisObject(result))
                {
                    var propertyValue = stream.ReadFieldValue(serializableField);
                    serializableField.SetValue(result, propertyValue);   
                }
            }

            return result;
        }

        private static string ReadUntilNull(this Stream stream)
        {
            var bytes = new List<byte>();
            var ch = stream.ReadByte();

            while (ch != 0)
            {
                if (ch == -1)
                {
                    throw new IOException("End of stream");
                }

                bytes.Add((byte)ch);
                ch = stream.ReadByte();
            }

            return Encoding.ASCII.GetString(bytes.ToArray());
        }

        private static object ReadPrimitive(this Stream stream, Type what)
        {
            var str = stream.ReadUntilNull();
            return TypeDescriptor.GetConverter(what).ConvertFromString(null, CultureInfo.InvariantCulture, str);
        }

        private static object ReadFieldValue(this Stream stream, FieldInfo field)
        {
            if (field.ShouldSerializeAsEnumerable())
            {
                return stream.ReadEnumerable(field.FieldType.GetGenericArguments().Single());
            }

            if (field.ShouldSerializeAsBool())
            {
                return stream.ReadBool();
            }
            
            if (field.ShouldSerializeAsDateTime())
            {
                return stream.ReadDateTime();
            }

            if (field.ShouldSerializeAsEnum())
            {
                return stream.ReadEnum(field.FieldType);
            }

            return stream.ReadPrimitive(field.FieldType);
        }

        private static object ReadDateTime(this Stream stream)
        {
            var fieldValue = stream.ReadUntilNull();

            if (string.IsNullOrEmpty(fieldValue))
            {
                return null;
            }

            return DateTime.ParseExact(fieldValue, "yyyyMMdd", CultureInfo.InvariantCulture);
        }

        private static object ReadBool(this Stream stream)
        {
            var fieldValue = stream.ReadUntilNull();

            return fieldValue == "0" ? false : fieldValue == "1" ? true : (bool?)null;
        }

        private static object ReadEnum(this Stream stream, Type type)
        {
            return Enum.ToObject(type, stream.ReadPrimitive(typeof (int)));
        }

        private static object ReadEnumerable(this Stream stream, Type enumerableType)
        {
            var size = int.Parse(stream.ReadUntilNull(), CultureInfo.InvariantCulture);
            var result = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(enumerableType));

            for (var i = 0; i < size; i++)
            {
                result.Add(stream.ReadObject(enumerableType));
            }

            return result;
        }
    }
}
