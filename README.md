# Reactivities

### Back-end

```
dotnet --info
dotnet new list
```

```
dotnet new sln
dotnet new webapi -n API -controllers
dotnet new classlib -n Domain
dotnet new classlib -n Application
dotnet new classlib -n Persistence
dotnet new classlib -n Infrastructure
dotnet sln add API
dotnet sln add Domain
dotnet sln add Application
dotnet sln add Persistence
dotnet sln add Infrastructure
```

### Run project

Go into the `API` folder: <br />
`dotnet run` <br />
`dotnet watch` <br />

### Troubleshooting

<a href="./images/Troubleshooting+Self+signed+certificates+in+.Net.pdf">Download PDF</a>.</p>

### Optional(mise)

Inside the root folder:

```
touch .mise.toml
touch .env
```

Inside .mise.toml

```
[env._]
file = ".env"
```

Then in the Rider/Terminal:

```
mise trust
```

### Migrations

Inside the `root` folder:

```
dotnet ef migrations add InitialCreate -p Persistence -s API
mise run db-sync-appDbContext
```

Drop the database:

```
mise run db-drop
```

Update dotnet tool:

```
dotnet ef tool update dotnet-ef -g
```

Remove migations (remove the last migration)

```
dotnet ef migrations remove -p Persistence -s API
```

Drop Database

```
dotnet ef database drop -p Persistence -s API
```

### Git

Inside the `root` folder:

```
dotnet new gitignore
```

### Mise

```
mise doctor
```

### User Limit Issue

```
sudo sysctl fs.inotify.max_user_instances=512
```

### Build

Inside client folder:

```
pnpm run build
```

### Run docker file

Inside the root folder:

```
docker compose up -d
```
