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
    with open("param_int_shared_memory.data", "br") as f:
        # memory-map the file, size 0 means whole file
        data = ByteEncodeDecode.read_from_bytes(f.read())
        a = data[0]
        b = data[1]
        f.close()

def write_int():
    global result
    with open("result_shared_memory.data", "bw") as f:
        # memory-map the file, size 0 means whole file
        buffer = ByteEncodeDecode.to_byte(result)
        f.write(buffer)
        f.close()

def read_int_list():
    global a
    global b
    # Protocol: Each line of the file represents a parameter
    with open("param_array_int_shared_memory.data", "br") as f:
        # memory-map the file, size 0 means whole file
        data = ByteEncodeDecode.read_from_bytes(f.read())
        a = data[0]
        b = data[1]
        f.close()

def write_int_list():
    global result
    with open("result_shared_memory.data", "bw") as f:
        # memory-map the file, size 0 means whole file
        buffer = ByteEncodeDecode.to_byte(result)
        f.write(buffer)
        f.close()

def read_float():
    global a
    global b
    with open("param_float_shared_memory.data", "br") as f:
        # memory-map the file, size 0 means whole file
        data = ByteEncodeDecode.read_from_bytes(f.read())
        a = data[0]
        b = data[1]
        f.close()

def write_float():
    global result
    with open("result_shared_memory.data", "bw") as f:
        buffer = ByteEncodeDecode.to_byte(result)
        f.write(buffer)
        f.close()

def read_float_list():
    global a
    global b
    # Protocol: Each line of the file represents a parameter
    with open("param_array_float_shared_memory.data", "br") as f:
        # memory-map the file, size 0 means whole file
        data = ByteEncodeDecode.read_from_bytes(f.read())
        a = data[0]
        b = data[1]
        f.close()


def write_float_list():
    global result
    with open("result_shared_memory.data", "bw") as f:
        buffer = ByteEncodeDecode.to_byte(result)
        f.write(buffer)
        f.close()



int_read_result = timeit.Timer(read_int).timeit(number=10)
result = proof_of_concept.sum(a,b)
int_write_result = timeit.Timer(write_int).timeit(number=10)
int_read_list_result = timeit.Timer(read_int_list).timeit(number=10)
result = proof_of_concept.sum(a,b)
int_write_list_result = timeit.Timer(write_int_list).timeit(number=10)
float_read_result = timeit.Timer(read_float).timeit(number=10)
result = proof_of_concept.sum(a,b)
float_write_result = timeit.Timer(write_float).timeit(number=10)
float_read_list_result = timeit.Timer(read_float_list).timeit(number=10)
result = proof_of_concept.sum(a,b)
float_write_list_result = timeit.Timer(write_float_list).timeit(number=10)

result_str =  'Type      \tRead Time(s)\tWrite Time(s)\tRead Time Mean(s)\tWrite Time Mean(s)\n'
result_str += 'Int       \t' + str(int_read_result) + '\t' + str(int_write_result) + '\t' + str(int_read_result / 10) + '\t' + str(int_write_result / 10) + '\n'
result_str += 'Int List  \t' + str(int_read_list_result) + '\t' + str(int_write_list_result) + '\t' + str(int_read_list_result / 10) + '\t' + str(int_write_list_result / 10) + '\n'
result_str += 'Float     \t' + str(float_read_result) + '\t' + str(float_write_result) + '\t' + str(float_read_result / 10) + '\t' + str(float_write_result / 10) + '\n'
result_str += 'Float List\t' + str(float_read_list_result) + '\t' + str(float_write_list_result) + '\t' + str(float_read_list_result / 10) + '\t' + str(float_write_list_result / 10) + '\n'

with open("benchmarking/byte_encode_benchmarking_results.txt", "w") as f:
        # memory-map the file, size 0 means whole file
        f.write(result_str)
        f.close()

# read_int()
# result = proof_of_concept.sum(a, b)
# write_int()
# read_int_list()
# result = proof_of_concept.sum(a, b)
# write_int_list()
# read_float()
# result = proof_of_concept.sum(a, b)
# write_float()
# read_float_list()
# result = proof_of_concept.sum(a, b)
# write_float_list()