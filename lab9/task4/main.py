import tkinter as tk
from tkinter import ttk, messagebox
from collections import deque

class TVProgramNode:
    def __init__(self, time, program):
        self.time = time  # Час у форматі HH:MM
        self.program = program  # Назва програми
        self.left = None
        self.right = None

class TVProgramTree:
    def __init__(self):
        self.root = None
    
    def insert(self, time, program):
        if not self.validate_time(time):
            return False
        
        new_node = TVProgramNode(time, program)
        
        if self.root is None:
            self.root = new_node
            return True
        
        # Використовуємо чергу для обходу дерева (BFS)
        queue = deque([self.root])
        
        while queue:
            current = queue.popleft()
            
            if not current.left:
                current.left = new_node
                return True
            else:
                queue.append(current.left)
                
            if not current.right:
                current.right = new_node
                return True
            else:
                queue.append(current.right)
        
        return False
    
    def delete(self, time):
        if self.root is None:
            return False
        
        if self.root.time == time and self.root.left is None and self.root.right is None:
            self.root = None
            return True
        
        # Знаходимо вузол для видалення та найглибший вузол
        node_to_delete = None
        last_node = None
        parent_of_last = None
        queue = deque([(self.root, None)])  # (node, parent)
        
        while queue:
            current, parent = queue.popleft()
            
            if current.time == time:
                node_to_delete = current
            
            if current.left:
                queue.append((current.left, current))
            if current.right:
                queue.append((current.right, current))
            
            last_node = current
            parent_of_last = parent
        
        if not node_to_delete:
            return False
        
        # Замінюємо вузол для видалення значенням найглибшого вузла
        node_to_delete.time = last_node.time
        node_to_delete.program = last_node.program
        
        # Видаляємо найглибший вузол
        if parent_of_last.left == last_node:
            parent_of_last.left = None
        else:
            parent_of_last.right = None
        
        return True
    
    def traverse(self):
        programs = []
        if self.root is None:
            return programs
        
        queue = deque([self.root])
        
        while queue:
            current = queue.popleft()
            programs.append((current.time, current.program))
            
            if current.left:
                queue.append(current.left)
            if current.right:
                queue.append(current.right)
        
        return programs
    
    def search(self, time):
        if self.root is None:
            return None
        
        queue = deque([self.root])
        
        while queue:
            current = queue.popleft()
            
            if current.time == time:
                return current.program
            
            if current.left:
                queue.append(current.left)
            if current.right:
                queue.append(current.right)
        
        return None
    
    def validate_time(self, time):
        try:
            hours, minutes = map(int, time.split(':'))
            return 0 <= hours < 24 and 0 <= minutes < 60
        except:
            return False

class TVProgramApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Програма телебачення на тиждень")
        
        self.tree = TVProgramTree()
        
        self.create_widgets()
        self.setup_layout()
    
    def create_widgets(self):
        # Вхідні дані
        self.time_label = ttk.Label(self.root, text="Час (HH:MM):")
        self.time_entry = ttk.Entry(self.root, width=10)
        
        self.program_label = ttk.Label(self.root, text="Назва програми:")
        self.program_entry = ttk.Entry(self.root, width=30)
        
        # Кнопки
        self.add_button = ttk.Button(self.root, text="Додати передачу", command=self.add_program)
        self.delete_button = ttk.Button(self.root, text="Видалити передачу", command=self.delete_program)
        self.search_button = ttk.Button(self.root, text="Пошук передачі", command=self.search_program)
        self.show_button = ttk.Button(self.root, text="Показати всю програму", command=self.show_programs)
        
        # Відображення результатів
        self.result_label = ttk.Label(self.root, text="Результати:")
        self.result_tree = ttk.Treeview(self.root, columns=('Time', 'Program'), show='headings', height=10)
        self.result_tree.heading('Time', text='Час')
        self.result_tree.heading('Program', text='Програма')
        self.result_tree.column('Time', width=100)
        self.result_tree.column('Program', width=300)
        
        # Статусний бар
        self.status_var = tk.StringVar()
        self.status_bar = ttk.Label(self.root, textvariable=self.status_var, relief=tk.SUNKEN)
    
    def setup_layout(self):
        # Розміщення елементів
        self.time_label.grid(row=0, column=0, padx=5, pady=5, sticky=tk.W)
        self.time_entry.grid(row=0, column=1, padx=5, pady=5)
        
        self.program_label.grid(row=1, column=0, padx=5, pady=5, sticky=tk.W)
        self.program_entry.grid(row=1, column=1, padx=5, pady=5)
        
        self.add_button.grid(row=2, column=0, padx=5, pady=5)
        self.delete_button.grid(row=2, column=1, padx=5, pady=5)
        self.search_button.grid(row=3, column=0, padx=5, pady=5)
        self.show_button.grid(row=3, column=1, padx=5, pady=5)
        
        self.result_label.grid(row=4, column=0, columnspan=2, padx=5, pady=5, sticky=tk.W)
        self.result_tree.grid(row=5, column=0, columnspan=2, padx=5, pady=5)
        
        self.status_bar.grid(row=6, column=0, columnspan=2, sticky=tk.W+tk.E)
    
    def add_program(self):
        time = self.time_entry.get()
        program = self.program_entry.get()
        
        if not time or not program:
            self.status_var.set("Помилка: Введіть час і назву програми")
            return
        
        if not self.tree.validate_time(time):
            self.status_var.set("Помилка: Невірний формат часу (використовуйте HH:MM)")
            return
        
        if self.tree.insert(time, program):
            self.status_var.set(f"Програма '{program}' додана на час {time}")
            self.time_entry.delete(0, tk.END)
            self.program_entry.delete(0, tk.END)
        else:
            self.status_var.set("Помилка: Не вдалося додати програму")
    
    def delete_program(self):
        time = self.time_entry.get()
        
        if not time:
            self.status_var.set("Помилка: Введіть час програми для видалення")
            return
        
        if not self.tree.validate_time(time):
            self.status_var.set("Помилка: Невірний формат часу (використовуйте HH:MM)")
            return
        
        if self.tree.delete(time):
            self.status_var.set(f"Програма на час {time} видалена")
            self.time_entry.delete(0, tk.END)
            self.program_entry.delete(0, tk.END)
        else:
            self.status_var.set(f"Програма на час {time} не знайдена")
    
    def search_program(self):
        time = self.time_entry.get()
        
        if not time:
            self.status_var.set("Помилка: Введіть час програми для пошуку")
            return
        
        if not self.tree.validate_time(time):
            self.status_var.set("Помилка: Невірний формат часу (використовуйте HH:MM)")
            return
        
        program = self.tree.search(time)
        
        if program:
            self.status_var.set(f"На час {time} заплановано: {program}")
        else:
            self.status_var.set(f"На час {time} програм не знайдено")
    
    def show_programs(self):
        programs = self.tree.traverse()
        self.result_tree.delete(*self.result_tree.get_children())
        
        if not programs:
            self.status_var.set("Програма телебачення порожня")
            return
        
        for time, program in programs:
            self.result_tree.insert('', tk.END, values=(time, program))
        
        self.status_var.set(f"Знайдено {len(programs)} програм у розкладі")

if __name__ == "__main__":
    root = tk.Tk()
    app = TVProgramApp(root)
    root.mainloop()