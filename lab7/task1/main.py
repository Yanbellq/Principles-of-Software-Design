import tkinter as tk
from tkinter import messagebox
from collections import namedtuple

# Іменований кортеж
Product = namedtuple('Product', ['name', 'price', 'supplier', 'expiry'])

# Початкові дані
products_data = [
    Product("Хліб", 20, "Коваль", "01.06.2025"),
    Product("Молоко", 35, "Іваненко", "05.05.2025"),
    Product("Цукор", 25, "Петренко", "15.07.2025"),
    Product("Сіль", 15, "Кравчук", "30.12.2025"),
    Product("Крупа", 40, "Бондар", "11.08.2025"),
    Product("Яйця", 30, "Сидоренко", "22.04.2025"),
    Product("Олія", 50, "Гриценко", "17.09.2025"),
]

current_products = list(products_data)  # Список для оновлення

# Обчислення соціальних продуктів
def social_products(products):
    avg_price = sum(p.price for p in products) / len(products)
    social = [p.name for p in products if p.price < avg_price]
    result = f"Середня ціна: {avg_price:.2f}\nПродукти {', '.join(social)} входять в перелік соціальних."
    return result

# Оновити ціни
def update_individual_prices():
    global current_products
    try:
        updated = []
        for i, entry in enumerate(price_entries):
            new_price = float(entry.get())
            updated.append(current_products[i]._replace(price=new_price))
        current_products = updated
        messagebox.showinfo("Оновлено", "Ціни оновлено успішно.")
    except ValueError:
        messagebox.showerror("Помилка", "Всі ціни повинні бути числами!")

# Показати результати
def show_social_products():
    output.delete('1.0', tk.END)
    result = social_products(current_products)
    output.insert(tk.END, result)

# GUI
root = tk.Tk()
root.title("Редагування цін продуктів")

tk.Label(root, text="Введіть нову ціну для кожного продукту:").pack(pady=5)

frame = tk.Frame(root)
frame.pack()

price_entries = []

# Створення полів введення для кожного продукту
for i, product in enumerate(current_products):
    tk.Label(frame, text=product.name).grid(row=i, column=0, padx=5, pady=2, sticky='e')
    entry = tk.Entry(frame, width=10)
    entry.insert(0, str(product.price))
    entry.grid(row=i, column=1, padx=5, pady=2)
    price_entries.append(entry)

# Кнопки
tk.Button(root, text="Оновити ціни", command=update_individual_prices).pack(pady=5)
tk.Button(root, text="Показати соціальні продукти", command=show_social_products).pack(pady=5)

# Вивід
output = tk.Text(root, width=50, height=10)
output.pack(pady=10)

root.mainloop()
