import tkinter as tk
from tkinter import ttk, messagebox

class GraphApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Представлення графа")
        
        # Ваш граф
        self.vertices = ['a', 'b', 'c', 'd', 'e', 'f']
        self.edges = [
            ('b', 'b'), ('c', 'c'), ('f', 'f'),  # Петлі
            ('a', 'e'), ('b', 'e'), ('c', 'e'), ('c', 'f'),
            ('d', 'e'), ('d', 'f'), ('f', 'e')
        ]
        
        self.create_widgets()
        self.setup_layout()
    
    def create_widgets(self):
        # Інформація про граф
        self.graph_info = ttk.LabelFrame(self.root, text="Інформація про граф")
        self.vertices_label = ttk.Label(self.graph_info, text=f"Вершини: {', '.join(self.vertices)}")
        self.edges_label = ttk.Label(self.graph_info, text=f"Ребра: {', '.join([f'({u}-{v})' for u, v in self.edges])}")
        
        # Вибір представлення
        self.representation_frame = ttk.LabelFrame(self.root, text="Представлення графа")
        self.representation_var = tk.StringVar(value="adjacency")
        self.adjacency_btn = ttk.Radiobutton(self.representation_frame, text="Матриця суміжності",
                                            variable=self.representation_var, value="adjacency")
        self.incidence_btn = ttk.Radiobutton(self.representation_frame, text="Матриця інцидентності",
                                           variable=self.representation_var, value="incidence")
        
        # Кнопки
        self.show_btn = ttk.Button(self.root, text="Показати матрицю", command=self.show_matrix)
        self.exit_btn = ttk.Button(self.root, text="Вийти", command=self.root.quit)
        
        # Відображення матриці
        self.matrix_frame = ttk.LabelFrame(self.root, text="Матриця графа")
        self.matrix_text = tk.Text(self.matrix_frame, height=10, width=60, state='disabled', font=('Courier', 10))
        self.scrollbar = ttk.Scrollbar(self.matrix_frame, orient='vertical', command=self.matrix_text.yview)
        self.matrix_text.configure(yscrollcommand=self.scrollbar.set)
    
    def setup_layout(self):
        # Розміщення елементів
        self.graph_info.grid(row=0, column=0, padx=10, pady=5, sticky='ew', columnspan=2)
        self.vertices_label.pack(anchor='w', padx=5, pady=2)
        self.edges_label.pack(anchor='w', padx=5, pady=2)
        
        self.representation_frame.grid(row=1, column=0, padx=10, pady=5, sticky='ew', columnspan=2)
        self.adjacency_btn.pack(anchor='w', padx=5, pady=2)
        self.incidence_btn.pack(anchor='w', padx=5, pady=2)
        
        self.show_btn.grid(row=2, column=0, padx=5, pady=5, sticky='e')
        self.exit_btn.grid(row=2, column=1, padx=5, pady=5, sticky='w')
        
        self.matrix_frame.grid(row=3, column=0, columnspan=2, padx=10, pady=5, sticky='nsew')
        self.matrix_text.pack(side='left', fill='both', expand=True)
        self.scrollbar.pack(side='right', fill='y')
        
        # Налаштування розтягування
        self.root.columnconfigure(0, weight=1)
        self.root.columnconfigure(1, weight=1)
        self.root.rowconfigure(3, weight=1)
    
    def create_adjacency_matrix(self):
        """Створює матрицю суміжності для графа з петлями"""
        size = len(self.vertices)
        matrix = [[0]*size for _ in range(size)]
        
        for u, v in self.edges:
            i = self.vertices.index(u)
            j = self.vertices.index(v)
            matrix[i][j] += 1
            if u != v:  # Для петлі додаємо тільки один раз
                matrix[j][i] += 1
                
        return matrix
    
    def create_incidence_matrix(self):
        """Створює матрицю інцидентності для графа з петлями"""
        vertices_count = len(self.vertices)
        edges_count = len(self.edges)
        matrix = [[0]*edges_count for _ in range(vertices_count)]
        
        for edge_idx, (u, v) in enumerate(self.edges):
            i = self.vertices.index(u)
            j = self.vertices.index(v)
            matrix[i][edge_idx] += 1
            if u != v:  # Для петлі додаємо тільки один раз
                matrix[j][edge_idx] += 1
                
        return matrix
    
    def show_matrix(self):
        matrix_type = self.representation_var.get()
        self.matrix_text.config(state='normal')
        self.matrix_text.delete(1.0, tk.END)
        
        if matrix_type == "adjacency":
            matrix = self.create_adjacency_matrix()
            self.display_matrix(matrix, self.vertices, self.vertices, "Матриця суміжності")
        else:
            matrix = self.create_incidence_matrix()
            edge_labels = [f"e{i+1}" for i in range(len(self.edges))]
            self.display_matrix(matrix, self.vertices, edge_labels, "Матриця інцидентності")
        
        self.matrix_text.config(state='disabled')
    
    def display_matrix(self, matrix, row_labels, col_labels, title):
        """Відображає матрицю у текстовому полі"""
        # Заголовок
        self.matrix_text.insert(tk.END, f"{title}\n\n")
        
        # Заголовки стовпців
        header = "    " + "  ".join(f"{label:>4}" for label in col_labels) + "\n"
        self.matrix_text.insert(tk.END, header)
        
        # Рядки матриці
        for i, row in enumerate(matrix):
            row_str = f"{row_labels[i]:<2}  " + "  ".join(f"{val:>4}" for val in row) + "\n"
            self.matrix_text.insert(tk.END, row_str)

if __name__ == "__main__":
    root = tk.Tk()
    app = GraphApp(root)
    root.mainloop()