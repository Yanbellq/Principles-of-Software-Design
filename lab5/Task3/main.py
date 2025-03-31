import tkinter as tk
from tkinter import messagebox

class TransportVehicle:
    def __init__(self, vehicle_type, speed, year):
        self.vehicle_type = vehicle_type
        self.speed = speed
        self.year = year

    def cost(self):
        return (self.speed * self.year) / 100

    def update_vehicle(self):
        self.year -= 5

    def info(self):
        return f"{self.vehicle_type}, {self.year}, Вартість: {self.cost():.2f}"

class Airplane(TransportVehicle):
    def __init__(self, speed, year, height):
        super().__init__("Літак", speed, year)
        self.height = height

    def cost(self):
        return self.speed * self.year

    def update_vehicle(self):
        self.year -= 3

class Ship(TransportVehicle):
    def __init__(self, speed, year, port):
        super().__init__("Корабель", speed, year)
        self.port = port

    def cost(self):
        return (self.speed * self.year) / 10

def create_vehicle():
    try:
        speed = int(speed_entry.get())
        year = int(year_entry.get())
        vehicle_type = vehicle_var.get()
        
        if vehicle_type == "Транспортний засіб":
            vehicle = TransportVehicle(vehicle_type, speed, year)
        elif vehicle_type == "Літак":
            height = int(extra_entry.get())
            vehicle = Airplane(speed, year, height)
        elif vehicle_type == "Корабель":
            port = extra_entry.get()
            vehicle = Ship(speed, year, port)
        
        vehicles.append(vehicle)
        update_display()
    except ValueError:
        messagebox.showerror("Помилка", "Неправильний формат введених даних")

def update_vehicles():
    for vehicle in vehicles:
        vehicle.update_vehicle()
    update_display()

def update_display():
    text_box.delete("1.0", tk.END)
    for vehicle in vehicles:
        text_box.insert(tk.END, vehicle.info() + "\n")

root = tk.Tk()
root.title("Транспортні засоби")

vehicles = []

vehicle_var = tk.StringVar(value="Транспортний засіб")
vehicle_label = tk.Label(root, text="Тип транспорту:")
vehicle_label.pack()
vehicle_menu = tk.OptionMenu(root, vehicle_var, "Транспортний засіб", "Літак", "Корабель")
vehicle_menu.pack()

speed_label = tk.Label(root, text="Швидкість (км/год):")
speed_label.pack()
speed_entry = tk.Entry(root)
speed_entry.pack()

year_label = tk.Label(root, text="Рік випуску:")
year_label.pack()
year_entry = tk.Entry(root)
year_entry.pack()

extra_label = tk.Label(root, text="Додатковий параметр:")
extra_label.pack()
extra_entry = tk.Entry(root)
extra_entry.pack()

create_button = tk.Button(root, text="Створити", command=create_vehicle)
create_button.pack()

update_button = tk.Button(root, text="Оновити", command=update_vehicles)
update_button.pack()

text_box = tk.Text(root, height=10, width=50)
text_box.pack()

root.mainloop()
