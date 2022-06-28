using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ZeroPass.Storage
{
    public static class EntityExtension
    {
        public static byte[] ToByteArray<T>(this T entity)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using MemoryStream memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, entity);
            return memoryStream.ToArray();
        }

        public static T ToEntity<T>(this byte[] bytes)
        {
            using MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Binder = new CustomBinder();
            memoryStream.Write(bytes, 0, bytes.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return (T)binaryFormatter.Deserialize(memoryStream);
        }

        sealed class CustomBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                if (string.IsNullOrEmpty(assemblyName) || string.IsNullOrEmpty(typeName))
                {
                    throw new SerializationException("Invalid deserialized object");
                }
                return Assembly.Load(assemblyName).GetType(typeName);
            }
        }
    }
}
