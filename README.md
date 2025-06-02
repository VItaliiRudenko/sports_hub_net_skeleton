# Sports-Hub Application .NET Back-End

## Project Description

This is a draft pet project for testing Generative AI on different software engineering tasks. It is planned to evolve and grow over time. Specifically, this repo will be a .NET playground. As for now, we only have the Angular application as a front-end, but in the future, we plan to extend it to other technologies. The application's legend is based on the sports-hub application description from the following repo: [Sports-Hub](https://github.com/dark-side/sports-hub).

## Available Front-End applications
- [Angular](https://github.com/dark-side/sports_hub_angular_skeleton)

## Testing

### Running Tests
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/SportsHub.Api.UnitTests/
dotnet test tests/SportsHub.Api.IntegrationTests/
```

## Dependencies

- Docker
- Docker Compose

The mentioned dependencies can be installed using the official documentation [here](https://docs.docker.com/compose/install/).

## Setup and Running the Application

### Clone the Repositories

To run the web application with the React front-end, clone the following repositories within the same folder:

```sh
git clone https://github.com/dark-side/sports_hub_net_skeleton.git
git clone https://github.com/dark-side/sports_hub_angular_skeleton.git
```

### Run Docker Compose

Navigate to the back-end application directory and run:

```sh
docker compose up
```

### Attach to the Backend Container

Run `docker ps` and copy the `backend` application container ID. Then, connect to the container with the following command:

```sh
docker exec -ti <CONTAINER ID> /bin/bash
```

### Database migrations

Migrations are automatically applied on application start.

### Configuration

The application supports the following configuration sections:

#### SMTP Configuration (Optional)
```json
{
  "Smtp": {
    "Host": "smtp.example.com",
    "Port": "587",
    "Username": "your-email@example.com",
    "Password": "your-password"
  }
}
```

**Note**: If SMTP configuration is missing, the email service will gracefully degrade and log warnings instead of crashing the application.

### Running on Windows (Tips & Tricks)

While running the App on Windows 11 using WSL, you may face issues related to Unix-style line endings (especially if you are storing the project(s) under the host machine filesystem, not the WSL one (e.g., the project is cloned to the disc `c:` or any other disk you have instead of being cloned to the WSL filesystem). Working within the WSL filesystem is a best practice when developing on Windows, as it helps prevent line ending and permission issues that can arise when using the Windows filesystem. I'm just reminding you that this will save you time and headaches for future projects.

If you are still reading this, please ensure your host machine converts related script(s) to Unix-style line endings.
```sh
# Install dos2unix if not already installed
sudo apt-get install dos2unix

# Convert all files in the project directory to Unix-style line endings
find . -type f -exec dos2unix {} \;

# Convert one file (example)
dos2unix app/appsettings.json
```
Also, if you face issues with `bin` directory files not being executable, you can fix it with the following commands:
```sh
# check current permissions on the file
ls -l app/SportsHub.Api

# ensure the file is executable
chmod +x app/SportsHub.Api
```

### Accessing the Application
To access the application in a browser locally, open the following URL:
- Mac, Linux - `http://localhost:3000/`
- Windows - `http://127.0.0.1:3000/`

## Development

### Project Structure
```
src/
├── SportsHub.Api/              # Web API layer
├── SportsHub.Domain/           # Domain entities and business logic
├── SportsHub.Infrastructure.Db/ # Database access layer
└── SportsHub.Infrastructure.Db.Migrations/ # EF migrations

tests/
├── SportsHub.Api.UnitTests/    # Unit tests
└── SportsHub.Api.IntegrationTests/ # Integration tests
```

All services include comprehensive XML documentation for IntelliSense support.

## License

Licensed under either of

- [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0)
- [MIT license](http://opensource.org/licenses/MIT)

Just to let you know, at your option.

## Contribution
Unless you explicitly state otherwise, any contribution intentionally submitted for inclusion in your work, as defined in the Apache-2.0 license, shall be dual licensed as above, without any additional terms or conditions.

**Should you have any suggestions, please create an Issue for this repository**
