import pandas as pd

titles = pd.read_csv('netflix_data.csv')
users = pd.read_csv('netflix_users.csv')
movies = pd.read_csv('Netflix_Dataset_Movie.csv')
ratings = pd.read_csv('Netflix_Dataset_Rating.csv')

print("Titles columns:", titles.columns)
print(titles.head(2))

print("Users columns:", users.columns)
print(users.head(2))

print("Movies columns:", movies.columns)
print(movies.head(2))

print("Ratings columns:", ratings.columns)
print(ratings.head(2))

merged_movies_ratings = pd.merge(movies, ratings, on='Movie_ID', how='inner', indicator=True)
print("\nMerged movies and ratings:")
print(merged_movies_ratings.head())

merged_users_ratings = pd.merge(users, ratings, on='User_ID', how='inner')
print("\nMerged users and ratings:")
print(merged_users_ratings.head())


movies.set_index('Movie_ID', inplace=True)
ratings.set_index('Movie_ID', inplace=True)

joined_movies_ratings = movies.join(ratings, how='left', lsuffix='_movies', rsuffix='_ratings')
print("\nJoined movies and ratings:")
print(joined_movies_ratings.head())
