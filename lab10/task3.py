from typing import Iterator
import math


def float_range(start: float, stop: float, step: float) -> Iterator[float]:

    if step == 0:
        raise ValueError("Крок не може дорівнювати нулю")


    if (step > 0 and start >= stop) or (step < 0 and start <= stop):
        return

    current = start
    while True:
        if (step > 0 and current >= stop) or (step < 0 and current <= stop):
            break

        yield round(current, 10)
        current += step


        if math.isclose(current, stop, rel_tol=1e-9, abs_tol=1e-9):
            break


def get_float_input(prompt: str) -> float:

    while True:
        try:
            return float(input(prompt))
        except ValueError:
            print("Будь ласка, введіть коректне число!")


def get_yes_no_input(prompt: str) -> bool:

    while True:
        choice = input(prompt).lower()
        if choice == 'так':
            return True
        elif choice == 'ні':
            return False
        else:
            print("Будь ласка, введіть 'так' або 'ні'")


def main():
    print("Генератор діапазону з плаваючою крапкою")
    print("--------------------------------------")

    while True:
        print("\nВведіть параметри діапазону:")
        try:
            start = get_float_input("Початкове значення (start): ")
            stop = get_float_input("Кінцеве значення (stop, не включається): ")
            step = get_float_input("Крок (step): ")

            print("\nРезультат:")
            result = list(float_range(start, stop, step))

            if not result:
                print("Діапазон не містить жодного значення за заданих параметрів")
            else:
                for num in result:
                    print(num)

            print(f"\nУсього згенеровано {len(result)} значень")

        except ValueError as e:
            print(f"\nПомилка: {e}")

        if not get_yes_no_input("\nБажаєте продовжити? (так/ні): "):

            break


if __name__ == "__main__":
    main()