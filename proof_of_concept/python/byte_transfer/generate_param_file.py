import random

size = 1000000

#Generate 5 random numbers between 10 and 30
randomListA = random.sample(range(0, 1000000), 1000000)
randomListB = random.sample(range(0, 1000000), 1000000)

toWriteA = ','.join([str(i) for i in randomListA])
toWriteB = ','.join([str(i) for i in randomListB])

with open("param_array_int_shared_memory.txt", "w") as f:
    f.write(toWriteA)
    f.write('\n')
    f.write(toWriteB)
    f.close()

randomListFloatA = []
randomListFloatB = []

for i in range(1000000):
    randomListFloatA.append(random.uniform(0, 1000))
    randomListFloatB.append(random.uniform(0, 1000))

toWriteA = ','.join([str(i) for i in randomListFloatA])
toWriteB = ','.join([str(i) for i in randomListFloatB])

with open("param_array_float_shared_memory.txt", "w") as f:
    f.write(toWriteA)
    f.write('\n')
    f.write(toWriteB)
    f.close()