using Nucleus.Extensions;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Nucleus.Serialization
{
    /// <summary>A utility class to serialize and deserialize objects and their hierarchy to binary.</summary>
    public static class BinarySerializer
    {
        /// <summary>Converts a variable to binary and writes it to a file.</summary>
        /// <param name="toSerialize">The variable we are serializing to the file.</param>
        /// <param name="path">The path to the file we are writing to.</param>
        /// <param name="overwrite">Whether we should replace the file.</param>
        /// <param name="maxBinaryDataLength">The maximum amount of binary data that can be in a single packet.</param>
        public static void Serialize<T>(T? toSerialize, string? path, bool overwrite = true,
            int maxBinaryDataLength = 1024)
        {
            // Validate the path is valid, if it isn't, throw an exception
            if (path == null)
            {
                throw new IOException("Path cannot be null");
            }

            // Validate the object being serialized is valid, if it isn't, throw an exception
            if (toSerialize == null)
            {
                throw new NullReferenceException("ToSerialize cannot be null");
            }

            // Open the file stream and create a binary writer for it.
            using FileStream fileStream = new(path, overwrite ? FileMode.Create : FileMode.Append, FileAccess.Write);
            BinaryWriter writer = new(fileStream);

            // Serialize the value and dispose of the writer
            Serialize(toSerialize, ref writer, maxBinaryDataLength);
            writer.Dispose();
        }

        /// <summary>Attempts to read and deserialize all binary data from the specified file.</summary>
        /// <param name="toDeserialize">The value that we are deserializing the data to.</param>
        /// <param name="path">The path that the binary data exists in.</param>
        public static void Deserialize<T>(ref T? toDeserialize, string? path)
        {
            // Validate the path is valid, if it isn't, throw an exception
            if (path == null)
            {
                throw new IOException("Path cannot be null");
            }

            // Validate the file exists, if it doesn't throw an exception
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found", path);
            }

            // Open the file stream and get the binary data from it
            using FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new(fileStream);

            // Create the deserialized type and attempt to deserialize it
            Deserialize(ref toDeserialize, ref reader);

            // Cleanup and return the type
            reader.Dispose();
        }

        /// <summary>Deserialize the variable and it's hierarchy.</summary>
        /// <param name="toDeserialize">The value we want to deserialize to.</param>
        /// <param name="reader">The reader we are pulling the binary from.</param>
        private static void Deserialize<T>(ref T? toDeserialize, ref BinaryReader reader)
        {
            toDeserialize ??= Activator.CreateInstance<T>();

            // Final failsafe.
            if (toDeserialize == null)
            {
                return;
            }

            // Iterate over the fields of the passed value
            foreach (FieldInfo field in GetFields(toDeserialize))
            {
                // Validate that the value we are trying to serialize is allowed to be serialized
                if (!Serializable(field))
                {
                    continue;
                }

                object? value = field.GetValue(toDeserialize);
                // Check if the value we are looking at is primitive
                if (TryRead(ref value, ref reader))
                {
                    // We need to handle boxing for value types
                    if (field.FieldType.IsValueType)
                    {
                        object boxed = toDeserialize;

                        field.SetValue(boxed, value);

                        toDeserialize = (T)boxed;
                    }
                    else
                    {
                        field.SetValue(toDeserialize, value);
                    }

                    continue;
                }

                if (value is IEnumerable)
                {
                    Type type = field.FieldType.GetGenericArguments()[0];
                    MethodInfo? add = field.FieldType.GetMethod("Add");

                    int count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        object? item = Activator.CreateInstance(type);

                        // Check if the value we are looking at is primitive
                        if (!TryRead(ref item, ref reader))
                        {
                            Deserialize(ref item, ref reader);
                        }

                        add?.Invoke(value, [item]);
                    }
                }
                else
                {
                    Deserialize(ref value, ref reader);
                    field.SetValue(toDeserialize, value);
                }
            }
        }

        /// <summary>Serialize the variable and it's hierarchy.</summary>
        /// <param name="toSerialize">The value we want to serialize into binary.</param>
        /// <param name="writer">The writer we are putting the binary into.</param>
        /// <param name="maxBinaryDataLength">The maximum amount of binary data that can be in a single packet.</param>
        private static void Serialize<T>(T? toSerialize, ref BinaryWriter writer, int maxBinaryDataLength)
        {
            // Validate the variable we are trying to serialize
            if (toSerialize == null)
            {
                return;
            }

            // Iterate over the fields of the passed value
            foreach (FieldInfo field in GetFields(toSerialize))
            {
                // Validate that the value we are trying to serialize is allowed to be serialized
                if (!Serializable(field))
                {
                    continue;
                }

                // Get the raw value from the field
                object? value = field.GetValue(toSerialize);
                if (value == null || TryWrite(value, ref writer, maxBinaryDataLength))
                {
                    continue;
                }

                // Check if the variable is a list
                if (value is IEnumerable list)
                {
                    // Write the length of the list to binary
                    IEnumerable enumerable = list as object[] ?? list.Cast<object>().ToArray();
                    writer.Write(enumerable.Cast<object>().Count());

                    foreach (object item in enumerable)
                    {
                        // Check if the value we are looking at is primitive
                        if (!TryWrite(item, ref writer, maxBinaryDataLength))
                        {
                            Serialize(item, ref writer, maxBinaryDataLength);
                        }
                    }
                }
                else
                {
                    // Recursively get the values as this is not a raw value if it is marked as serializable
                    Serialize(value, ref writer, maxBinaryDataLength);
                }
            }
        }

        /// <summary>Get all fields from the passed object.</summary>
        private static FieldInfo[] GetFields<T>(T? obj)
        {
            return obj == null
                ? []
                : obj.GetType().GetFields(
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic
                );
        }

        /// <summary>Returns whether the passed field is able to serialize.</summary>
        private static bool Serializable(FieldInfo field)
        {
            return (field.HasAttribute<SerializeFieldAttribute>() || field.IsPublic) &&
                   !field.HasAttribute<NonSerializedAttribute>();
        }

        private static bool TryRead(ref object? toRead, ref BinaryReader reader)
        {
            if (toRead == null)
            {
                return false;
            }

            if (Convert.GetTypeCode(toRead) == TypeCode.Object)
            {
                return false;
            }

            toRead = Convert.GetTypeCode(toRead) switch
            {
                TypeCode.Boolean => reader.ReadBoolean(),
                TypeCode.Char => reader.ReadChar(),
                TypeCode.Byte => reader.ReadByte(),
                TypeCode.Int16 => reader.ReadInt16(),
                TypeCode.UInt16 => reader.ReadUInt16(),
                TypeCode.Int32 => reader.ReadInt32(),
                TypeCode.UInt32 => reader.ReadUInt32(),
                TypeCode.Int64 => reader.ReadInt64(),
                TypeCode.UInt64 => reader.ReadUInt64(),
                TypeCode.Single => reader.ReadSingle(),
                TypeCode.Double => reader.ReadDouble(),
                TypeCode.String => reader.ReadString(),
                TypeCode.DateTime => DateTime.FromBinary(reader.ReadInt64()),
                _ => throw new ArgumentOutOfRangeException()
            };

            return true;
        }

        /// <summary>Attempts to write the passed value into binary.</summary>
        /// <param name="toWrite">The value we want to write into binary.</param>
        /// <param name="writer">The writer we are putting the binary into.</param>
        /// <param name="maxBinaryDataLength">The maximum amount of binary data that can be in a single packet.</param>
        /// <returns>True if the variable was written to binary, false if the variable was null or an object.</returns>
        private static bool TryWrite(object? toWrite, ref BinaryWriter writer, int maxBinaryDataLength)
        {
            // Validate the variable is not null
            if (toWrite == null)
            {
                return false;
            }

            // Check if the value we are looking at is primitive
            if (Convert.GetTypeCode(toWrite) == TypeCode.Object)
            {
                return false;
            }

            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (Convert.GetTypeCode(toWrite))
            {
                case TypeCode.String:
                    // We will write strings differently so that it is correct
                    writer.Write((string)toWrite);
                    break;
                case TypeCode.DateTime:
                    writer.Write(((DateTime)toWrite).ToBinary());
                    break;
                default:
                    // Write the raw binary data and continue
                    writer.Write(GetBytes(toWrite, maxBinaryDataLength));
                    break;
            }

            return true;
        }

        /// <summary>Get the raw binary data of the passed value.</summary>
        /// <returns>The raw binary data of the passed value.</returns>
        private static byte[] GetBytes(object value, int maxBinaryDataLength)
        {
            // Get the raw byte data length and generate an array with the size
            int rawSize = Marshal.SizeOf(value);
            byte[] rawData = new byte[rawSize];

            // Get the raw data from the passed value
            GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
            Marshal.StructureToPtr(value, handle.AddrOfPinnedObject(), false);

            // Cleanup the handle
            handle.Free();

            // If the raw data length is within the bounds of the maxLength, return the raw data
            if (maxBinaryDataLength >= rawData.Length)
            {
                return rawData;
            }

            // We exceeded the rawData length, so create a temporary one with
            // the max length
            byte[] temp = new byte[maxBinaryDataLength];
            Array.Copy(rawData, temp, rawData.Length);

            return temp;
        }
    }
}