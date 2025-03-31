import tkinter as tk
from tkinter import messagebox

class Persona:
    def __init__(self, name="", age=0, gender=""):
        self.name = name
        self.age = age
        self.gender = gender

    def show(self):
        return f"Persona: {self.name}, Age: {self.age}, Gender: {self.gender}"

    def work(self):
        return "Persona is working in general."

    def __del__(self):
        print("Лабораторна робота виконана студентом 2 курсу ПІБ студента")


class Sluzhbovets(Persona):
    def __init__(self, name="", age=0, gender="", department=""):
        super().__init__(name, age, gender)
        self.department = department

    def show(self):
        return f"Sluzhbovets: {self.name}, Age: {self.age}, Gender: {self.gender}, Department: {self.department}"

    def work(self):
        return "Sluzhbovets is handling administrative tasks."


class Robitnyk(Persona):
    def __init__(self, name="", age=0, gender="", skill_level=0):
        super().__init__(name, age, gender)
        self.skill_level = skill_level

    def show(self):
        return f"Robitnyk: {self.name}, Age: {self.age}, Gender: {self.gender}, Skill Level: {self.skill_level}"

    def work(self):
        return "Robitnyk is performing physical labor."


class Inzhener(Persona):
    def __init__(self, name="", age=0, gender="", specialization=""):
        super().__init__(name, age, gender)
        self.specialization = specialization

    def show(self):
        return f"Inzhener: {self.name}, Age: {self.age}, Gender: {self.gender}, Specialization: {self.specialization}"

    def work(self):
        return "Inzhener is designing and developing solutions."


def display_info():
    name = name_entry.get()
    age = age_entry.get()
    gender = gender_entry.get()
    role = role_var.get()
    extra = extra_entry.get()

    if role == "Sluzhbovets":
        person = Sluzhbovets(name, age, gender, extra)
    elif role == "Robitnyk":
        person = Robitnyk(name, age, gender, extra)
    elif role == "Inzhener":
        person = Inzhener(name, age, gender, extra)
    else:
        person = Persona(name, age, gender)
    
    messagebox.showinfo("Information", person.show() + "\n" + person.work())

# GUI Setup
root = tk.Tk()
root.title("Employee Information")

# Labels and Inputs
tk.Label(root, text="Name:").grid(row=0, column=0)
name_entry = tk.Entry(root)
name_entry.grid(row=0, column=1)

tk.Label(root, text="Age:").grid(row=1, column=0)
age_entry = tk.Entry(root)
age_entry.grid(row=1, column=1)

tk.Label(root, text="Gender:").grid(row=2, column=0)
gender_entry = tk.Entry(root)
gender_entry.grid(row=2, column=1)

tk.Label(root, text="Role:").grid(row=3, column=0)
role_var = tk.StringVar(value="Persona")
role_menu = tk.OptionMenu(root, role_var, "Persona", "Sluzhbovets", "Robitnyk", "Inzhener")
role_menu.grid(row=3, column=1)

tk.Label(root, text="Extra Info:").grid(row=4, column=0)
extra_entry = tk.Entry(root)
extra_entry.grid(row=4, column=1)

# Submit Button
tk.Button(root, text="Show Info", command=display_info).grid(row=5, column=0, columnspan=2)

root.mainloop()
