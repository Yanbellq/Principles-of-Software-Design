import pandas as pd

df = pd.read_csv('train.csv')

print("1. Перші 5 рядків:")
print(df.head())

print("\n2. Останні 5 рядків:")
print(df.tail())

print("\n3. Розмір датафрейму (рядки, стовпці):")
print(df.shape)

print("\n4. Назви стовпців:")
print(df.columns)

print("\n5. Типи даних у стовпцях:")
print(df.dtypes)

print("\n6. Приклади loc та iloc:")
print("loc: вибір рядка з індексом 0 (ім'я, вік):")
print(df.loc[0, ['Name', 'Age']])
print("\niloc: перші 3 рядки, стовпці 2-4:")
print(df.iloc[:3, 2:5])

print("\n7. Статистика (describe):")
print(df.describe())

print("\n8. Розподіл значень для стовпця 'Sex':")
print(df['Sex'].value_counts())

print("\n9. Унікальні значення стовпця 'Embarked':")
print(df['Embarked'].unique())

print("\n10. Кореляційна матриця:")
print(df.corr(numeric_only=True))

print("\n11. Випадкова вибірка (3 рядки):")
print(df.sample(3))