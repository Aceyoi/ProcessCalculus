import numpy as np
import math

def testcheck():
    test = 5
    assert (list(generate1(test)) == [0,1,2,3,4])
    assert (list(generate2(test)) == [0,1,4,9,16])
    assert (list(generate3(test)) == [1, 1, 2, 6, 24])
    assert (list(generate4(test)) == [2, 4, 8, 16, 32])
    assert (list(generate5(test)) == [4, 11, 31, 89, 259])
    assert ([round(test, 2) for test in generate6(test)] == [1.00, 2.00, 2.00, 1.33, 0.67])
    assert ([round(test, 2) for test in generate7(test)] == [0.00, 1.00, 1.50, 1.83, 2.08])
    assert ([round(test, 2) for test in generate8(test)] == [0.00, 1.00, 0.50, 0.83, 0.58])
    assert ([round(test, 2) for test in generate9(test)] == [0.00, 1.00, 3.00, 5.00, 6.83])
    pass

def generate1(n):
    for i in range(n):
        yield i

def generate2(n):
    for i in range(n):
        yield i**2

def generate3(n):
    for i in range(n):
            yield math.factorial(i)

def generate4(n):
    for i in range(n):
            yield 2 ** (i + 1)

def generate5(n):
    for i in range(n):
            yield 2 ** i + 3 ** (i + 1)

def generate6(n):
    for i in range(n):
            yield 2 ** i / math.factorial(i)

def generate7(n):
    for i in range(n):
            yield sum(1 / k for k in range(1, i + 1))

def generate8(n):
    for i in range(n):
            yield sum((-1) ** (k + 1) / k for k in range(1, i + 1))

def generate9(n):
    for i in range(n):
            yield i * sum(1 / math.factorial(k) for k in range(1, i + 1))

n = int(input("Введите n "))
testcheck()
print(list(generate1(n)))
print(list(generate2(n)))
print(list(generate3(n)))
print(list(generate4(n)))
print(list(generate5(n)))
#print(list(generate6(n)))
print([round(n, 2) for n in generate6(n)])
#print(list(generate7(n)))
print([round(n, 2) for n in generate7(n)])
#print(list(generate8(n)))
print([round(n, 2) for n in generate8(n)])
#print(list(generate9(n)))
print([round(n, 2) for n in generate9(n)])
