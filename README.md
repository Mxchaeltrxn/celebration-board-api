# Celebration Board Api

## Description

This is the api for the Celebration Board app, a forum where users can celebrate their achievements and write the things they are grateful for. The idea is to create a place on the internet where people can inspire each other to achieve more in their life because I think the internet currently holds too much negativity. (This app does not promote [toxic positivity](https://www.urbandictionary.com/define.php?term=Toxic%20Positivity).)

## Local Development

Prerequisite

- [Dotnet runtime 6.0.1](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Dotnet sdk 6.0.1](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Postgres 14.1](https://www.postgresql.org/download/)

Steps to set up your local repository:

1. Clone the repository.

```sh
git clone git@github.com:<YOUR_GITHUB_ACCOUNT>/celebration-board-api.git
```

2. Change into the repository directory.

```sh
cd celebration-board-api
```

3. Fill in any empty application config fields in `src/CelebrationBoard.Api/appsettings.Development.json`.

4. Add user secrets

```sh
dotnet user-secrets "CelebrationBoardConnectionString" "<Db connection string>"
dotnet user-secrets "CelebrationBoardUsersConnectionString" "<Db connection string>"
```

4. Start application

```sh
dotnet run --project src/CelebrationBoard.Api
```

5. Visit application at `https://localhost:7034`.

Running tests

1. Run tests

```sh
# Run from root directory to ensure all tests are run.
dotnet test
```

Viewing api documentation

1. Start the application

```sh
dotnet run --project src/CelebrationBoard.Api
```

2. View Swagger documentation via `https://localhost:7046/swagger/index.html`