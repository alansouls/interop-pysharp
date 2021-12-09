import struct
from enum import IntEnum
from timeit import main
from typing import Type

class VarType(IntEnum):
    Int = 0
    Float = 1
    Char = 2
    Bool = 3
    
class TypeSize(IntEnum):
    Int = 4
    Float = 8
    Char = 1
    Bool = 1

class ByteEncodeDecode:
    simple_value_header_size = 2
    array_value_header_size = 6
    simple_value_header = 0
    array_value_header = 1
    
    @staticmethod
    def to_byte(value):
        if type(value) is int:
            return ByteEncodeDecode.int_to_byte(value)
        elif type(value) is float:
            return ByteEncodeDecode.float_to_byte(value)
        elif type(value) is list and type(value[0]) is int:
            return ByteEncodeDecode.int_list_to_byte(value)
        elif type(value) is list and type(value[0]) is float:
            return ByteEncodeDecode.float_list_to_byte(value)
        elif type(value) is str:
            return ByteEncodeDecode.string_to_byte(value)
        elif type(value) is bool:
            return ByteEncodeDecode.bool_to_byte(value)

    
            
    @staticmethod
    def bool_to_byte(value):
        content =  bytes([ByteEncodeDecode.simple_value_header])
        content += bytes([VarType.Bool])
        content += struct.pack('<c', (1).to_bytes(1, byteorder='little') if value else (0).to_bytes(1, byteorder='little'))
        return content

    @staticmethod
    def string_to_byte(value):
        content =  bytes([ByteEncodeDecode.array_value_header])
        content += bytes([VarType.Char])
        content += bytes(struct.pack('<i', len(value)))
        content += struct.pack('<{0}s'.format(len(value)), bytes(value, 'utf-8'))
        return content
            
    @staticmethod
    def int_to_byte(value):
        content =  bytes([ByteEncodeDecode.simple_value_header])
        content += bytes([VarType.Int])
        content += struct.pack('<i', value)
        return content

    @staticmethod
    def float_to_byte(value):
        content =  bytes([ByteEncodeDecode.simple_value_header])
        content += bytes([VarType.Float])
        content += struct.pack('<d', value)
        return content

    @staticmethod
    def int_list_to_byte(value):
        content =  bytes([ByteEncodeDecode.array_value_header])
        content += bytes([VarType.Int])
        content += bytes(struct.pack('<i', len(value)))
        content += struct.pack('<{0}i'.format(len(value)), *value)
        return content

    @staticmethod
    def float_list_to_byte(value):
        content =  bytes([ByteEncodeDecode.array_value_header])
        content += bytes([VarType.Float])
        content += bytes(struct.pack('<i', len(value)))
        content += struct.pack('<{0}d'.format(len(value)), *value)
        return content

    @staticmethod
    def read_from_bytes(buffer):
        result = []
        i = 0
        while i < len(buffer):
            is_array = buffer[i] != 0
            i += 1
            type = VarType(buffer[i])
            i += 1
            if not is_array:
                if type == VarType.Int:
                    result.append(struct.unpack('<i', buffer[i : i + TypeSize.Int])[0])
                    i += TypeSize.Int
                elif type == VarType.Float:
                    result.append(struct.unpack('<d', buffer[i : i + TypeSize.Float])[0])
                    i += TypeSize.Float
                elif type == VarType.Bool:
                    result.append(struct.unpack('<c', buffer[i : i + TypeSize.Bool])[0] != (0).to_bytes(1, byteorder='little'))
                    i += TypeSize.Bool
                elif type == VarType.Char:
                    result.append(struct.unpack('<c', buffer[i : i + TypeSize.Char])[0])
                    i += TypeSize.Char
            else:
                arrayLen = struct.unpack('<i', buffer[i : i + TypeSize.Int])[0]
                i += TypeSize.Int
                if type == VarType.Int:
                    array = list(struct.unpack('<{0}i'.format(arrayLen), buffer[i : i + (TypeSize.Int * arrayLen)]))
                    result.append(array)
                    i += (arrayLen * TypeSize.Int)
                elif type == VarType.Float:
                    array = list(struct.unpack('<{0}d'.format(arrayLen), buffer[i : i + (TypeSize.Float * arrayLen)]))
                    result.append(array)
                    i += (arrayLen * TypeSize.Float)
                elif type == VarType.Bool:
                    array = list(struct.unpack('<{0}c'.format(arrayLen), buffer[i : i + (TypeSize.Bool * arrayLen)]))
                    array = [x != 0 for x in array]
                    result.append(array)
                    i += (arrayLen * TypeSize.Bool)
                elif type == VarType.Char:
                    result.append(buffer[i : i + (TypeSize.Char * (arrayLen))].decode('utf-8'))
                    i += ((arrayLen) * TypeSize.Char)
        
        return result




def test():
    int_encoded = ByteEncodeDecode.to_byte(1)
    print(int_encoded)
    int_decoded = ByteEncodeDecode.read_from_bytes(int_encoded)[0]
    print(int_decoded)

    float_encoded = ByteEncodeDecode.to_byte(1.0)
    print(float_encoded)
    float_decoded = ByteEncodeDecode.read_from_bytes(float_encoded)[0]
    print(float_decoded)

    int_list_encoded = ByteEncodeDecode.to_byte([1, 2, 3])
    print(int_list_encoded)
    int_list_decoded = ByteEncodeDecode.read_from_bytes(int_list_encoded)[0]
    print(int_list_decoded)

    float_list_encoded = ByteEncodeDecode.to_byte([1.0, 2.0, 3.0])
    print(float_list_encoded)
    float_list_decoded = ByteEncodeDecode.read_from_bytes(float_list_encoded)[0]
    print(float_list_decoded)

    bool_encoded = ByteEncodeDecode.to_byte(True)
    print(bool_encoded)
    bool_decoded = ByteEncodeDecode.read_from_bytes(bool_encoded)[0]
    print(bool_decoded)

    bool_encoded = ByteEncodeDecode.to_byte(False)
    print(bool_encoded)
    bool_decoded = ByteEncodeDecode.read_from_bytes(bool_encoded)[0]
    print(bool_decoded)


    string_encoded = ByteEncodeDecode.to_byte('alan')
    string_encoded += ByteEncodeDecode.to_byte('maia')
    print(string_encoded)
    strings_decoded = ByteEncodeDecode.read_from_bytes(string_encoded)
    print(strings_decoded)


if __name__ == '__main__':
    test()
