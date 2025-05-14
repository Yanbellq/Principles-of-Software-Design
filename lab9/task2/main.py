import tkinter as tk
from tkinter import filedialog, messagebox

class FootballTeamsApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Розподіл футбольної команди на групи")
        
        self.players = []
        
        self.create_widgets()
    
    def create_widgets(self):
        self.load_button = tk.Button(self.root, text="Завантажити список гравців", command=self.load_players)
        self.load_button.pack(pady=10)
        
        self.original_label = tk.Label(self.root, text="Початковий список гравців (20 осіб):")
        self.original_label.pack()
        
        self.original_listbox = tk.Listbox(self.root, width=50, height=10)
        self.original_listbox.pack(pady=5)
        
        self.process_button = tk.Button(self.root, text="Розподілити на групи", command=self.split_teams)
        self.process_button.pack(pady=10)
        
        self.teams_frame = tk.Frame(self.root)
        self.teams_frame.pack(pady=10)
        
        self.team1_label = tk.Label(self.teams_frame, text="Група 1 (решта гравців):")
        self.team1_label.grid(row=0, column=0, padx=10)
        
        self.team1_listbox = tk.Listbox(self.teams_frame, width=30, height=10)
        self.team1_listbox.grid(row=1, column=0, padx=10)
        
        self.team2_label = tk.Label(self.teams_frame, text="Група 2 (кожен 12-й з усіх 20):")
        self.team2_label.grid(row=0, column=1, padx=10)
        
        self.team2_listbox = tk.Listbox(self.teams_frame, width=30, height=10)
        self.team2_listbox.grid(row=1, column=1, padx=10)
        
        self.save_button = tk.Button(self.root, text="Зберегти результати", command=self.save_results)
        self.save_button.pack(pady=10)
    
    def load_players(self):
        file_path = filedialog.askopenfilename(filetypes=[("Text files", "*.txt"), ("All files", "*.*")])
        
        if file_path:
            try:
                with open(file_path, 'r', encoding='utf-8') as file:
                    self.players = [line.strip() for line in file.readlines() if line.strip()]
                
                if len(self.players) != 20:
                    messagebox.showerror("Помилка", f"У файлі має бути рівно 20 гравців. Знайдено: {len(self.players)}")
                    self.players = []
                    return
                
                self.original_listbox.delete(0, tk.END)
                self.team1_listbox.delete(0, tk.END)
                self.team2_listbox.delete(0, tk.END)
                
                for player in self.players:
                    self.original_listbox.insert(tk.END, player)
                
                messagebox.showinfo("Успіх", "Список гравців успішно завантажено!")
            except Exception as e:
                messagebox.showerror("Помилка", f"Не вдалося завантажити файл: {str(e)}")
    
    def split_teams(self):
        if not self.players:
            messagebox.showerror("Помилка", "Спочатку завантажте список гравців!")
            return
        
        self.team1_listbox.delete(0, tk.END)
        self.team2_listbox.delete(0, tk.END)
        
        # Створюємо копію списку для роботи
        players_copy = self.players.copy()
        group2 = []
        current_index = 0  # Починаємо з першого гравця
        
        # Формуємо другу групу (кожен 12-й)
        while len(group2) < 10:
            # Обчислюємо індекс гравця (12-й у кільцевому списку)
            current_index = (current_index + 11) % len(players_copy)
            selected_player = players_copy[current_index]
            
            # Додаємо гравця до другої групи
            group2.append(selected_player)
            
            # Видаляємо гравця з копії списку, щоб не вибирати його знову
            players_copy.pop(current_index)
            
            # Якщо видалили гравця, індекс може вийти за межі
            if current_index >= len(players_copy) and len(players_copy) > 0:
                current_index = current_index % len(players_copy)
        
        # Перша група - решта гравців
        group1 = players_copy
        
        # Заповнюємо списки
        for player in group1:
            self.team1_listbox.insert(tk.END, player)
        
        for player in group2:
            self.team2_listbox.insert(tk.END, player)
    
    def save_results(self):
        if not self.players or self.team1_listbox.size() == 0:
            messagebox.showerror("Помилка", "Немає даних для збереження!")
            return
        
        file_path = filedialog.asksaveasfilename(defaultextension=".txt", filetypes=[("Text files", "*.txt"), ("All files", "*.*")])
        
        if file_path:
            try:
                with open(file_path, 'w', encoding='utf-8') as file:
                    file.write("Група 2 (кожен 12-й з усіх 20 гравців):\n")
                    for i in range(self.team2_listbox.size()):
                        file.write(f"{self.team2_listbox.get(i)}\n")
                    
                    file.write("\nГрупа 1 (решта гравців):\n")
                    for i in range(self.team1_listbox.size()):
                        file.write(f"{self.team1_listbox.get(i)}\n")
                
                messagebox.showinfo("Успіх", "Результати успішно збережено!")
            except Exception as e:
                messagebox.showerror("Помилка", f"Не вдалося зберегти файл: {str(e)}")

if __name__ == "__main__":
    root = tk.Tk()
    app = FootballTeamsApp(root)
    root.mainloop()