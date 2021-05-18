import proof_of_concept
import mmap

a = 1
b = 2
with open("result_shared_memory", "wb+") as f:
    # memory-map the file, size 0 means whole file
    mm = mmap.mmap(f.fileno(), 4)
    a = mm.read(4)
    mm.close()

result = proof_of_concept.sum(a,b)

resultHex = "{:02x}".format(result) 

with open("result_shared_memory", "wb+") as f:
    # memory-map the file, size 0 means whole file
    mm = mmap.mmap(f.fileno(), 4)
    mm.write(bytes.fromhex(resultHex))
    mm.close()