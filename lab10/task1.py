def filter_even_numbers(nums: list[int]) -> list[int]:
    return [num for num in nums if num % 2 == 0]


input_string = input("Введіть числа через пробіл: ")
numbers = list(map(int, input_string.split()))

print(filter_even_numbers(numbers))