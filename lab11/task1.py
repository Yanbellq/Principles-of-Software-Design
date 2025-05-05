def sort_by_age(people: list[dict]) -> list[dict]:
    return sorted(people, key=lambda person: person["age"])


def main():
    people = []
    n = int(input("Скільки людей хочете ввести? "))
    for _ in range(n):
        name = input("Введіть ім'я: ")
        age = int(input("Введіть вік: "))
        people.append({"name": name, "age": age})

    sorted_people = sort_by_age(people)
    print("\nВідсортований список:")
    for person in sorted_people:
        print(person)


if __name__ == "__main__":
    main()