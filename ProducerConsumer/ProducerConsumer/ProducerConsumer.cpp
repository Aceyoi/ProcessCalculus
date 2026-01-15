// Сапожниковым Ю.С.
// Используемые библиотеки
#include <iostream>
#include "Producer.h"
#include "Consumer.h"
#include "ArrayManager.h"

using namespace std;

int main()
{

    setlocale(LC_ALL, "Russian");

    // Ввод размера маассива
    int N;
    cout << "ВВедите размер массива: ";
    cin >> N;

    // Создаём объекты классов
    producer<int> producer(N);
    consumer<int> consumer;

    // Producer создаёт массив
    ArrayManager Array = producer.Produce();

    cout << "Массив: ";
    for (int x : Array.GetNumbers())
        cout << x << " ";
    cout << endl;

    // Consumer сортирует массив
    consumer.Consume(Array);

    cout << "Отсортированный массив: ";
    for (int x : Array.GetNumbers())
        cout << x << " ";
    cout << endl;

    // Проверка на сортировку
    if (Array.IsSorted())
        cout << "Массив отсортирован.\n";
    else
        cout << "Массив НЕ отсортирован.\n";

    return 0;
}