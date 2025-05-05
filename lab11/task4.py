from typing import Callable


def has_uppercase(password: str) -> bool:

    return any(char.isupper() for char in password)


def has_digit(password: str) -> bool:

    return any(char.isdigit() for char in password)


def is_long_enough(password: str) -> bool:

    return len(password) >= 8


def has_special_char(password: str) -> bool:

    special_chars = "!@#$%^&*()"
    return any(char in special_chars for char in password)


def no_spaces(password: str) -> bool:

    return ' ' not in password


def validate_password(password: str) -> bool:

    rules: list[Callable[[str], bool]] = [
        has_uppercase,
        has_digit,
        is_long_enough,
        has_special_char,
        no_spaces
    ]
    return all(rule(password) for rule in rules)


def get_password_input():

    print("Введіть пароль для перевірки (вимоги:")
    print("- Мінімум 8 символів")
    print("- Хоча б одна велика літера (A-Z)")
    print("- Хоча б одна цифра (0-9)")
    print("- Хоча б один спецсимвол (!@#$%^&*())")
    print("- Без пробілів")
    return input("Пароль: ").strip()


if __name__ == "__main__":
    password = get_password_input()
    is_valid = validate_password(password)

    if is_valid:
        print(" Пароль відповідає всім вимогам!")
    else:
        print(" Пароль не відповідає вимогам. Перевірте правила.")