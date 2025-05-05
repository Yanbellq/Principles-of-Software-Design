import tkinter as tk
from tkinter import messagebox
from datetime import datetime

def calculate():
    try:
        start_time = datetime.strptime(entry_start.get(), '%H:%M:%S')
        end_time = datetime.strptime(entry_end.get(), '%H:%M:%S')
        rate_per_minute = int (entry_rate.get())

        duration = (end_time - start_time).total_seconds()
        if duration < 0:
            duration += 24 * 3600

        cost_hryvnias = (duration / 60) * (rate_per_minute / 100)

        label_result.config(
            text=f"Тривалість дзвінка: {int(duration)} сек\n"
                 f"Вартість дзвінка: {cost_hryvnias:.2f} грн"
        )
    except ValueError:
        messagebox.showerror("Помилка", "Будь ласка, введіть коректні значення!")


# GUI

root = tk.Tk()
root.title("Розрахунок вартості дзвінка")

tk.Label(root, text="Час початку (ГГ:ХХ:СС):").grid(row=0, column=0, sticky="e")
entry_start = tk.Entry(root)
entry_start.grid(row=0, column=1)

tk.Label(root, text="Час закінчення (ГГ:ХХ:СС):").grid(row=1, column=0, sticky="e")
entry_end = tk.Entry(root)
entry_end.grid(row=1, column=1)

tk.Label(root, text="Вартість 1 хв (у копійках):").grid(row=2, column=0, sticky="e")
entry_rate = tk.Entry(root)
entry_rate.grid(row=2, column=1)

tk.Button(root, text="Розрахувати", command=calculate).grid(row=3, columnspan=2, pady=10)

label_result = tk.Label(root, text="", fg="blue", font=("Arial", 12))
label_result.grid(row=4, columnspan=2)

root.mainloop()



