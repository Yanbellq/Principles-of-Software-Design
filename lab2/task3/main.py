import random

# Кількість студентів і дисциплін
num_students = 20
num_disciplines = 10

# Генерація випадкових оцінок в діапазоні від 0 до 100
grades = [[random.randint(1, 12) for _ in range(num_disciplines)] for _ in range(num_students)]

# Виведення оцінок
print("Оцінки студентів:")
for i, student_grades in enumerate(grades):
    print(f"Студент {i + 1}: {student_grades}")

# Обчислення середньої успішності по кожній дисципліні
average_per_discipline = [sum(discipline_grades) / num_students for discipline_grades in zip(*grades)]

# Обчислення загальної успішності групи
total_average = sum(sum(student_grades) for student_grades in grades) / (num_students * num_disciplines)

# Виведення результатів
print("\nСередня успішність по кожній дисципліні:")
for i, avg in enumerate(average_per_discipline):
    print(f"Дисципліна {i + 1}: {avg:.2f}")

print(f"\nЗагальна успішність групи: {total_average:.2f}")