def filter_long_words(words: list[str]) -> list[str]:
    return list(filter(lambda word: len(word) > 3, words))


def main():
    user_input = input("Введіть слова через пробіл: ")
    words = user_input.split()  # Розбиваємо рядок у список слів
    filtered_words = filter_long_words(words)
    print("\nСлова довші за 3 символи:")
    print(filtered_words)


if __name__ == "__main__":
    main()