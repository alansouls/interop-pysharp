import proof_of_concept
import timeit
import random



a = 0
b = 0
result = 0

encoding = 'UTF-8'

def read_int():
    global a
    global b
    # Protocol: Each line of the file represents a parameter
    with open("param_int_shared_memory.txt", "r") as f:
        # memory-map the file, size 0 means whole file
        a = int(f.readline())
        b = int(f.readline())
        f.close()

def write_int():
    global result
    with open("result_shared_memory.txt", "w") as f:
        # memory-map the file, size 0 means whole file
        f.write(str(result))
        f.close()

def read_int_list():
    global a
    global b
    # Protocol: Each line of the file represents a parameter
    with open("param_array_int_shared_memory.txt", "r") as f:
        # memory-map the file, size 0 means whole file
        a = [int(i) for i in f.readline().split(',')]
        b = [int(i) for i in f.readline().split(',')]
        f.close()

def write_int_list():
    global result
    with open("result_shared_memory.txt", "w") as f:
        # memory-map the file, size 0 means whole file
        f.write(str(result))
        f.close()

def read_float():
    global a
    global b
    with open("param_float_shared_memory.txt", "r") as f:
        # memory-map the file, size 0 means whole file
        a = float(f.readline())
        b = float(f.readline())
        f.close()

def write_float():
    global result
    with open("result_shared_memory.txt", "w") as f:
        # memory-map the file, size 0 means whole file
        f.write(str(result))
        f.close()

def read_float_list():
    global a
    global b
    # Protocol: Each line of the file represents a parameter
    with open("param_array_float_shared_memory.txt", "r") as f:
        # memory-map the file, size 0 means whole file
        a = [float(i) for i in f.readline().split(',')]
        b = [float(i) for i in f.readline().split(',')]
        f.close()


def write_float_list():
    global result
    with open("result_shared_memory.txt", "w") as f:
        # memory-map the file, size 0 means whole file
        f.write(str(result))
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

with open("benchmarking/file_encode_benchmarking_resuls.txt", "w") as f:
        # memory-map the file, size 0 means whole file
        f.write(result_str)
        f.close()