import random
from byte_encode_decode import VarType, ByteEncodeDecode, TypeSize

size = 1000000

#Generate 5 random numbers between 10 and 30
randomListA = random.sample(range(0, 1000000), 1000000)
randomListB = random.sample(range(0, 1000000), 1000000)

with open("param_array_int_shared_memory.data", "bw") as f:
    buffer = ByteEncodeDecode.to_byte(randomListA)
    buffer += ByteEncodeDecode.to_byte(randomListB)
    f.write(buffer)

randomListFloatA = []
randomListFloatB = []

for i in range(1000000):
    randomListFloatA.append(random.uniform(0, 1000))
    randomListFloatB.append(random.uniform(0, 1000))

with open("param_array_float_shared_memory.data", "bw") as f:
    buffer = ByteEncodeDecode.to_byte(randomListFloatA)
    buffer += ByteEncodeDecode.to_byte(randomListFloatB)
    f.write(buffer)