from typing import Iterator
import ast

def walk_tree(data: dict) -> Iterator[str]:
    for key, value in data.items():
        yield key
        if isinstance(value, dict):
            yield from walk_tree(value)

def input_tree() -> dict:
    print("Введіть дерево у вигляді словника (наприклад: {'a': {'b': 1}, 'c': 2}):")
    while True:
        try:
            user_input = input().strip()
            return ast.literal_eval(user_input)
        except (ValueError, SyntaxError):
            print("Неправильний формат. Спробуйте ще раз (наприклад: {'a': {'b': 1}}):")

if __name__ == "__main__":
    tree = input_tree()
    print("Результат обходу дерева:")
    print(list(walk_tree(tree)))