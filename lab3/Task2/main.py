import tkinter as tk
from tkinter import ttk
import re

class App:
    def __init__(self, root):
        self.root = root
        self.root.title("Моё приложение")
        
        # Создаем и размещаем виджеты
        self.label = ttk.Label(root, text="Привет, мир!")
        self.label.pack(padx=20, pady=20)
        
        self.button = ttk.Button(root, text="Нажми меня", command=self.button_click)
        self.button.pack(padx=20, pady=10)
        
        self.text = tk.Text(root, height=10, width=50)
        self.text.pack(padx=20, pady=10)
        
        self.find_button = ttk.Button(root, text="Найти .com адреса", command=self.find_com_addresses)
        self.find_button.pack(padx=20, pady=10)
        
        self.result_label = ttk.Label(root, text="")
        self.result_label.pack(padx=20, pady=10)
        
        self.remove_entry = ttk.Entry(root)
        self.remove_entry.pack(padx=20, pady=10)
        
        self.remove_button = ttk.Button(root, text="Удалить подстроку", command=self.remove_substring)
        self.remove_button.pack(padx=20, pady=10)
        
        self.replace_entry = ttk.Entry(root)
        self.replace_entry.pack(padx=20, pady=10)
        
        self.replace_with_entry = ttk.Entry(root)
        self.replace_with_entry.pack(padx=20, pady=10)
        
        self.replace_button = ttk.Button(root, text="Заменить подстроку", command=self.replace_substring)
        self.replace_button.pack(padx=20, pady=10)
        
    def button_click(self):
        self.label.config(text="Кнопка была нажата!")
        
    def find_com_addresses(self):
        text = self.text.get("1.0", tk.END)
        com_addresses = re.findall(r'\bhttps?://\S+\.com\b', text)
        self.result_label.config(text=f"Найдено .com адресов: {len(com_addresses)}")
        
    def remove_substring(self):
        text = self.text.get("1.0", tk.END)
        substring = self.remove_entry.get()
        new_text = text.replace(substring, "")
        self.text.delete("1.0", tk.END)
        self.text.insert(tk.END, new_text)
        
    def replace_substring(self):
        text = self.text.get("1.0", tk.END)
        substring = self.replace_entry.get()
        replace_with = self.replace_with_entry.get()
        new_text = text.replace(substring, replace_with)
        self.text.delete("1.0", tk.END)
        self.text.insert(tk.END, new_text)

# Создаем главное окно
root = tk.Tk()
app = App(root)

# Запускаем главный цикл обработки событий
root.mainloop()