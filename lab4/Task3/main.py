import tkinter as tk
from tkinter import messagebox, filedialog, StringVar
import os
import logging

def setup_logging():
    with open("Task3/db/Session_log.txt", "w") as f:
        f.write("Дія 1: додаток запущено\n")
    logging.basicConfig(filename="Task3/db/Session_log.txt", level=logging.INFO, format='%(message)s')

def import_data():
    try:
        with open("Task3/db/Input_data.txt", "r", encoding="utf-8") as f:
            data = f.read().strip()
        if not data:
            messagebox.showerror("Помилка", "Файл порожній, введіть дані")
            return
        global num1, num2
        parts = data.split()
        if len(parts) != 2 or not parts[0].isdigit() or not parts[1].isdigit():
            messagebox.showerror("Помилка", "Недопустимі значення введених параметрів")
            return
        num1, num2 = int(parts[0]), int(parts[1])
        logging.info("Дія 2: обрано 'Імпортувати вхідні дані'")
        messagebox.showinfo("Імпорт", "Дані імпортовано успішно")
    except FileNotFoundError:
        messagebox.showerror("Помилка", "Файл Input_data.txt не знайдено")

def calculate():
    if num1 is None or num2 is None:
        messagebox.showerror("Помилка", "Спочатку імпортуйте дані")
        return
    operation = selected_op.get()
    logging.info(f"Дія 3: обрано арифметичну операцію '{operation}'")
    
    try:
        if operation == "+":
            result = num1 + num2
        elif operation == "-":
            result = num1 - num2
        elif operation == "*":
            result = num1 * num2
        elif operation == "/":
            if num2 == 0:
                messagebox.showerror("Помилка", "Ділення на 0 заборонено")
                return
            result = num1 / num2
        elif operation == "^":
            result = num1 ** num2
        else:
            messagebox.showerror("Помилка", "Невідома операція")
            return
        
        output = f"{num1} {operation} {num2}, Результат: {result}\n"
        result_label.config(text=output)
        logging.info("Дія 4: обрано 'Обчислити вираз'")
        return output
    except Exception as e:
        messagebox.showerror("Помилка", str(e))

def export_result():
    result = calculate()
    if result:
        with open("Task3/db/Output_data.txt", "a", encoding="utf-8") as f:
            f.write(result)
        logging.info("Дія 5: обрано 'Експортувати результат у файл'")
        messagebox.showinfo("Експорт", "Результат збережено у файл")

def on_close():
    logging.info("Дія 6: додаток закрито")
    root.destroy()

setup_logging()
num1, num2 = None, None

root = tk.Tk()
root.title("Арифметичний калькулятор")
root.protocol("WM_DELETE_WINDOW", on_close)

selected_op = StringVar(value="+")
operations = ["+", "-", "*", "/", "^"]

tk.Button(root, text="Імпортувати вхідні дані", command=import_data).pack()
for op in operations:
    tk.Radiobutton(root, text=op, variable=selected_op, value=op).pack(anchor="w")
tk.Button(root, text="Обчислити вираз", command=calculate).pack()
tk.Button(root, text="Експортувати результат у файл", command=export_result).pack()

result_label = tk.Label(root, text="Результат: ")
result_label.pack()

root.mainloop()
