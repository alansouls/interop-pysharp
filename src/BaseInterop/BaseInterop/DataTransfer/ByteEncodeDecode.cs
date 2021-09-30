using System;
using System.Collections.Generic;
using System.Text;

namespace BaseInterop.DataTransfer
{
    public enum VarType
    {
        Int,
        Float
    };
    public static class ByteEncodeDecode
    {
        private const int SimpleValueHeaderSize = 2;
        private const int ArrayValueHeaderSize = 6;
        private const int SimpleValueHeader = 0;
        private const int ArrayValueHeader = 1;
        public static byte[] GetBytes(int value)
        {
            byte[] content = new byte[SimpleValueHeaderSize + sizeof(int)];
            content[0] = SimpleValueHeader;
            content[1] = (byte)VarType.Int;
            BitConverter.GetBytes(value).CopyTo(content, 2);
            return content;
        }
        public static byte[] GetBytes(double value)
        {
            byte[] content = new byte[SimpleValueHeaderSize + sizeof(double)];
            content[0] = SimpleValueHeader;
            content[1] = (byte)VarType.Float;
            BitConverter.GetBytes(value).CopyTo(content, 2);
            return content;
        }

        public static byte[] GetBytes(int[] array)
        {
            var arrayByteSize = sizeof(int) * array.Length;
            byte[] content = new byte[ArrayValueHeaderSize + arrayByteSize];
            content[0] = ArrayValueHeader;
            content[1] = (byte)VarType.Int;
            BitConverter.GetBytes(array.Length).CopyTo(content, 2);
            int j = 2 + sizeof(int);
            for (int i = 0; i < array.Length; ++i)
            {
                BitConverter.GetBytes(array[i]).CopyTo(content, j);
                j += sizeof(int);
            }
            return content;
        }

        public static byte[] GetBytes(double[] array)
        {
            var arrayByteSize = sizeof(double) * array.Length;
            byte[] content = new byte[ArrayValueHeaderSize + arrayByteSize];
            content[0] = ArrayValueHeader;
            content[1] = (byte)VarType.Float;
            BitConverter.GetBytes(array.Length).CopyTo(content, 2);
            int j = 2 + sizeof(int);
            for (int i = 0; i < array.Length; ++i)
            {
                BitConverter.GetBytes(array[i]).CopyTo(content, j);
                j += sizeof(double);
            }
            return content;
        }

        public static List<object> ReadFromBytes(byte[] buffer, out List<Type> types)
        {
            List<object> resultObject = new List<object>();
            types = new List<Type>();
            int i = 0;
            while (i < buffer.Length)
            {
                var isArray = buffer[i++] != 0;
                var type = (VarType)buffer[i++];
                if (!isArray)
                {
                    switch (type)
                    {
                        case VarType.Int:
                            resultObject.Add(BitConverter.ToInt32(buffer, i));
                            types.Add(typeof(int));
                            i += sizeof(int);
                            break;
                        case VarType.Float:
                            resultObject.Add(BitConverter.ToDouble(buffer, i));
                            types.Add(typeof(double));
                            i += sizeof(double);
                            break;
                        default:
                            throw new Exception();
                    }
                }
                else
                {
                    var arrayLen = BitConverter.ToInt32(buffer, i);
                    i += sizeof(int);
                    switch (type)
                    {
                        case VarType.Int:
                            var arrayInt = new int[arrayLen];
                            for (int j = 0; j < arrayLen; ++j)
                            {
                                arrayInt[j] = BitConverter.ToInt32(buffer, i);
                                i += sizeof(int);
                            }
                            resultObject.Add(arrayInt);
                            types.Add(typeof(int[]));
                            break;
                        case VarType.Float:
                            var arrayFloat = new double[arrayLen];
                            for (int j = 0; j < arrayLen; ++j)
                            {
                                arrayFloat[j] = BitConverter.ToDouble(buffer, i);
                                i += sizeof(double);
                            }
                            resultObject.Add(arrayFloat);
                            types.Add(typeof(double[]));
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }

            return resultObject;
        }
    }
}
