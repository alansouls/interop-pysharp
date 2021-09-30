import proof_of_concept
import timeit
import random
from byte_encode_decode import VarType, ByteEncodeDecode, TypeSize



a = 0
b = 0
result = 0

encoding = 'UTF-8'

def read_int():
    global a
    global b
    # Protocol: Each line of the file represents a parameter
    with open("input.data", "br") as f:
        # memory-map the file, size 0 means whole file
        data = ByteEncodeDecode.read_from_bytes(f.read())
        a = data[0]
        b = data[1]
        f.close()

def write_int():
    global result
    with open("output.data", "bw") as f:
        # memory-map the file, size 0 means whole file
        buffer = ByteEncodeDecode.to_byte(result)
        f.write(buffer)
        f.close()



read_int()

result = proof_of_concept.sum(a, b)

write_int()