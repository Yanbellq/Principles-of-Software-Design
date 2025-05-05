from typing import Iterable

def capitalize_words(words: Iterable[str]) -> Iterable[str]:
    return map(str.capitalize, words)

def get_user_input():
    print("Введіть слова через пробіл (наприклад: python java c++):")
    user_input = input().strip()
    return user_input.split() if user_input else []

if __name__ == "__main__":
    words = get_user_input()
    result = capitalize_words(words)
    print("Результат:")
    print(list(result))