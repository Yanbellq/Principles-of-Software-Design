import tkinter as tk
from tkinter import filedialog, scrolledtext
import re

def create_file():
    text = input_text.get("1.0", tk.END).strip()
    with open("Task2/db/TF_1.txt", "w", encoding="utf-8") as f:
        f.write(text)
    status_label.config(text="Файл TF_1.txt створено")

def analyze_file():
    try:
        with open("Task2/db/TF_1.txt", "r", encoding="utf-8") as f:
            text = f.read()
        words = re.findall(r"\b\w{1,16}\b", text)
        word_count = {}
        for word in words:
            length = len(word)
            if length not in word_count:
                word_count[length] = []
            word_count[length].append(word)
        
        with open("Task2/db/TF_2.txt", "w", encoding="utf-8") as f:
            for length in sorted(word_count.keys()):
                line = f"{', '.join(word_count[length])} ({len(word_count[length])})\n"
                f.write(line)
        
        status_label.config(text="Файл TF_2.txt створено")
    except FileNotFoundError:
        status_label.config(text="Файл TF_1.txt не знайдено")

def display_results():
    try:
        with open("Task2/db/TF_2.txt", "r", encoding="utf-8") as f:
            result_text.delete("1.0", tk.END)
            result_text.insert(tk.END, f.read())
    except FileNotFoundError:
        status_label.config(text="Файл TF_2.txt не знайдено")

root = tk.Tk()
root.title("Text File Analyzer")

input_text = scrolledtext.ScrolledText(root, width=75, height=15)
input_text.pack()

btn_create = tk.Button(root, text="Створити TF_1.txt", command=create_file)
btn_create.pack()

btn_analyze = tk.Button(root, text="Аналізувати TF_1.txt", command=analyze_file)
btn_analyze.pack()

btn_display = tk.Button(root, text="Відобразити TF_2.txt", command=display_results)
btn_display.pack()

result_text = scrolledtext.ScrolledText(root, width=75, height=15)
result_text.pack()

status_label = tk.Label(root, text="")
status_label.pack()

root.mainloop()
