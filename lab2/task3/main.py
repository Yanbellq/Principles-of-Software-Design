import random

num_students = 20
num_disciplines = 10

grades = [[random.randint(1, 12) for _ in range(num_disciplines)] for _ in range(num_students)]

print("Оцінки студентів:")
for i, student_grades in enumerate(grades):
    if (i < 9):
        print(f"Студент {i + 1}:  {student_grades}")
    else:
        print(f"Студент {i + 1}: {student_grades}")

average_per_discipline = [sum(discipline_grades) / num_students for discipline_grades in zip(*grades)]

total_average = sum(sum(student_grades) for student_grades in grades) / (num_students * num_disciplines)

print("\nСередня успішність по кожній дисципліні:")
for i, avg in enumerate(average_per_discipline):
    if(i < 9):
        print(f"Дисципліна {i + 1}:  {avg:.2f}")
    else:
        print(f"Дисципліна {i + 1}: {avg:.2f}")

print(f"\nЗагальна успішність групи: {total_average:.2f}")