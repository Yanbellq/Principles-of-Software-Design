import math
import random

def check_point(x, y, R):
    # Boundaries of the square: it's located in the second and fourth quadrants of the coordinate plane
    inside_square = (-R <= x <= 0) and (0 <= y <= R)

    # Boundaries of the circles: check if the point is inside either of the two circles
    inside_top_circle = (x ** 2 + (y - R) ** 2 <= R ** 2)
    inside_bottom_circle = (x ** 2 + (y + R) ** 2 <= R ** 2)

    # Check point inclusion
    if inside_square:
        return "Success"  # The point is inside the shaded region
    if inside_top_circle or inside_bottom_circle:
        return "Lose"  # The point is outside the shaded region
    return "On line"  # The point is on the boundary of the figure

# def main():
#     x = float(input("Введіть координату x: "))
#     y = float(input("Введіть координату y: "))
#     R = float(input("Введіть радіус R: "))

#     result = check_point(x, y, R)
#     print(f"Результат: {result}")



def main():
    try: 
        radius = float(input("Enter radius: "))
    except ValueError:
        print("Uncorrect input!")
        return

    shot(radius);


def shot(radius):
    header = "| {:<6} | {:<25} | {:<30} |".format("Number", "Coordinates (x, y)", "Result");
    separator = "-" * len(header)
    print(header)
    print(separator)

    for i in range(1, 11):
        x = random.uniform(-10, 10)
        y = random.uniform(-10, 10)

        result = check_point(x, y, radius)

        coord_str = "({:6.2f}, {:6.2f})".format(x, y)
        row = "| {:<6} | {:<25} | {:<30} |".format(i, coord_str, result)
        print(row);

if __name__ == "__main__":
    main()
