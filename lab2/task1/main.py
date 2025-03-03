def calculate_difference(arr):
    sum_even_indices = sum(arr[i] for i in range(len(arr)) if i % 2 == 0)
    sum_indices_multiple_of_three = sum(arr[i] for i in range(len(arr)) if i % 3 == 0)
    
    difference = sum_even_indices - sum_indices_multiple_of_three
    return difference

def main():
    # Приклад масиву
    arr = [1, -2, 3, 4, -5, 6, 7, -8, 9, 10]
    
    difference = calculate_difference(arr)
    print(f"Різниця між сумою елементів з парними індексами та сумою елементів, індекси яких кратні трьом: {difference}")

if __name__ == "__main__":
    main()