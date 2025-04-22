from Zadachi136modul import * # подключаем модуль
import sys

testcheck()
if int(sys.argv[1]) == 0:
    print("Введите число больше 0")
else:
    n = int(sys.argv[1])
    a = []

    if str(sys.argv[2]) not in ("y", "n"):
        print("Введите y или n")
    else:
        check = str(sys.argv[2])
        if check == "y":
            randomi(n, a)
            print(a)
            suma(n, a)
        else:
            if len(sys.argv)-3 == n:
                for i in range(3, len(sys.argv)):
                    a.append(float(sys.argv[i]))
                print(a)
                suma(n, a)
            else:
                print("n меньше указаного количества символов")



