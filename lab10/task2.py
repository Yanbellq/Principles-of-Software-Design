class Countdown:
    def __init__(self, start: int):
        self.current = start

    def __iter__(self):
        return self

    def __next__(self):
        if self.current < 0:
            raise StopIteration
        else:
            value = self.current
            self.current -= 1
            return value


if __name__ == "__main__":
    start_number = int(input("Введіть початкове число для зворотного відліку: "))

    for n in Countdown(start_number):
        print(n)