import tkinter as tk
from tkinter import ttk, messagebox
from abc import ABC, abstractmethod

# --- Класи співробітників ---
class Spivrobitnyk(ABC):
    def __init__(self, name, age, schedule):
        self.name = name
        self.age = age
        self.schedule = schedule

    def show_basic_info(self):
        return f"{self.name}, Вік: {self.age}, Графік: {self.schedule}"

    @abstractmethod
    def show_full_info(self):
        pass

    @abstractmethod
    def get_schedule(self):
        pass

class Sluzhbovets(Spivrobitnyk):
    def __init__(self, name, age, schedule, department, position):
        super().__init__(name, age, schedule)
        self.department = department
        self.position = position

    def show_full_info(self):
        return f"{self.show_basic_info()}, Відділ: {self.department}, Посада: {self.position}"

    def get_schedule(self):
        return self.schedule

    def promote(self, new_position):
        self.position = new_position

class Robochyi(Spivrobitnyk):
    def __init__(self, name, age, schedule, specialization, experience):
        super().__init__(name, age, schedule)
        self.specialization = specialization
        self.experience = experience

    def show_full_info(self):
        return f"{self.show_basic_info()}, Спеціалізація: {self.specialization}, Досвід: {self.experience} років"

    def get_schedule(self):
        return self.schedule

    def increase_experience(self):
        self.experience += 1

# --- GUI частина ---
class EmployeeApp:
    def __init__(self, root):
        self.root = root
        self.root.title("База Співробітників")
        self.employees = []

        self.create_widgets()

    def create_widgets(self):
        self.frame_input = ttk.LabelFrame(self.root, text="Додати співробітника")
        self.frame_input.grid(row=0, column=0, padx=10, pady=10)

        ttk.Label(self.frame_input, text="Ім'я:").grid(row=0, column=0, sticky="w")
        self.entry_name = ttk.Entry(self.frame_input)
        self.entry_name.grid(row=0, column=1)

        ttk.Label(self.frame_input, text="Вік:").grid(row=1, column=0, sticky="w")
        self.entry_age = ttk.Entry(self.frame_input)
        self.entry_age.grid(row=1, column=1)

        ttk.Label(self.frame_input, text="Графік роботи:").grid(row=2, column=0, sticky="w")
        self.entry_schedule = ttk.Entry(self.frame_input)
        self.entry_schedule.grid(row=2, column=1)

        ttk.Label(self.frame_input, text="Тип:").grid(row=3, column=0, sticky="w")
        self.combo_type = ttk.Combobox(self.frame_input, values=["Службовець", "Робочий"], state="readonly")
        self.combo_type.grid(row=3, column=1)
        self.combo_type.bind("<<ComboboxSelected>>", self.update_type_fields)

        self.extra_label1 = ttk.Label(self.frame_input, text="")  # динамічні поля
        self.extra_label1.grid(row=4, column=0, sticky="w")
        self.extra_entry1 = ttk.Entry(self.frame_input)
        self.extra_entry1.grid(row=4, column=1)

        self.extra_label2 = ttk.Label(self.frame_input, text="")
        self.extra_label2.grid(row=5, column=0, sticky="w")
        self.extra_entry2 = ttk.Entry(self.frame_input)
        self.extra_entry2.grid(row=5, column=1)

        ttk.Button(self.frame_input, text="Додати", command=self.add_employee).grid(row=6, column=0, columnspan=2, pady=5)

        ttk.Separator(self.root, orient="horizontal").grid(row=1, column=0, sticky="ew", padx=10)

        self.frame_output = ttk.LabelFrame(self.root, text="Результати")
        self.frame_output.grid(row=2, column=0, padx=10, pady=10)

        ttk.Button(self.frame_output, text="Показати всіх", command=self.show_all).grid(row=0, column=0, pady=5)
        ttk.Button(self.frame_output, text="Пошук за графіком", command=self.find_same_schedule).grid(row=0, column=1, pady=5)

        self.text_output = tk.Text(self.frame_output, width=80, height=15)
        self.text_output.grid(row=1, column=0, columnspan=2)

    def update_type_fields(self, event=None):
        selected = self.combo_type.get()
        if selected == "Службовець":
            self.extra_label1.config(text="Відділ:")
            self.extra_label2.config(text="Посада:")
        else:
            self.extra_label1.config(text="Спеціалізація:")
            self.extra_label2.config(text="Досвід (років):")

    def add_employee(self):
        try:
            name = self.entry_name.get()
            age = int(self.entry_age.get())
            schedule = self.entry_schedule.get()
            type_ = self.combo_type.get()
            extra1 = self.extra_entry1.get()
            extra2 = self.extra_entry2.get()

            if type_ == "Службовець":
                emp = Sluzhbovets(name, age, schedule, extra1, extra2)
            else:
                experience = int(extra2)
                emp = Robochyi(name, age, schedule, extra1, experience)

            self.employees.append(emp)
            messagebox.showinfo("Успіх", "Співробітника додано!")
        except Exception as e:
            messagebox.showerror("Помилка", f"Некоректні дані: {e}")

    def show_all(self):
        self.text_output.delete("1.0", tk.END)
        for emp in self.employees:
            self.text_output.insert(tk.END, emp.show_full_info() + "\n" + "-"*60 + "\n")

    def find_same_schedule(self):
        self.text_output.delete("1.0", tk.END)
        schedule_map = {}
        for emp in self.employees:
            sched = emp.get_schedule()
            schedule_map.setdefault(sched, []).append(emp)

        found = False
        for sched, emps in schedule_map.items():
            if len(emps) > 1:
                found = True
                self.text_output.insert(tk.END, f"Графік: {sched}\n")
                for emp in emps:
                    self.text_output.insert(tk.END, "  - " + emp.show_basic_info() + "\n")
                self.text_output.insert(tk.END, "-"*60 + "\n")

        if not found:
            self.text_output.insert(tk.END, "Немає співробітників з однаковим графіком.\n")

# --- Запуск ---
if __name__ == "__main__":
    root = tk.Tk()
    app = EmployeeApp(root)
    root.mainloop()
