import pandas as pd
import matplotlib.pyplot as plt

url = 'https://raw.githubusercontent.com/datasciencedojo/datasets/master/titanic.csv'
df = pd.read_csv(url)

print("Інформація про датасет:")
print(df.info())

print("\nПерші 5 записів:")
print(df.head())

print("\nОстанні 10 записів:")
print(df.tail(10))

not_survived = df[df['Survived'] == 0].copy()
print("\nКількість людей, які не вижили:", len(not_survived))

class_distribution = not_survived['Pclass'].value_counts()
print("\nРозподіл за класами серед тих, хто не вижив:")
print(class_distribution)

def age_category(age):
    if pd.isnull(age):
        return 'Невідомо'
    elif age < 12:
        return 'Діти'
    elif age < 18:
        return 'Підлітки'
    elif age < 30:
        return 'Молодь'
    elif age < 60:
        return 'Працездатний вік'
    else:
        return 'Пенсійний вік'

not_survived['AgeCategory'] = not_survived['Age'].apply(age_category)

age_class_dist = pd.crosstab(
    index=not_survived['AgeCategory'],
    columns=not_survived['Pclass'],
    colnames=['Клас']
)

print("\nРозподіл за віковими категоріями та класами:")
print(age_class_dist)

age_class_dist.plot(kind='bar', figsize=(12, 6))
plt.title('Розподіл невиживших за віком та класом білета')
plt.xlabel('Вікова категорія')
plt.ylabel('Кількість людей')
plt.legend(title='Клас білета')
plt.grid(True)
plt.show()