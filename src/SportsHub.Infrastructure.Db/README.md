# SportsHub.Infrastructure.Db

This project contains the data access layer implementation for the Sports Hub application, providing database connectivity, Entity Framework Core configuration, and data persistence services.

## Overview

The `SportsHub.Infrastructure.Db` project implements the repository pattern and Unit of Work pattern to provide a clean abstraction over data access operations. It uses Entity Framework Core with PostgreSQL as the database provider.

## Database Setup

### Option 1: Local PostgreSQL (Recommended for Development)

For local development on macOS, use the provided setup script:

```bash
# Make the script executable
chmod +x setup_postgres_local.sh

# Run the setup script
./setup_postgres_local.sh
```

This script will:
- Install PostgreSQL 17 via Homebrew (if not already installed)
- Create the required database and user
- Configure all necessary permissions
- Test the connection
- Provide connection details

**Database Configuration:**
- **Database**: `net_be_genai_plgrnd_db`
- **Username**: `postgres`
- **Password**: `example`
- **Host**: `localhost`
- **Port**: `5432`

### Option 2: Docker (Alternative)

If you prefer Docker, use the docker-compose setup from the project root:

```bash
# From project root
docker compose up db -d
```

This will expose PostgreSQL on port `45432`.

### Option 3: Manual Installation

For manual PostgreSQL installation, create the database and user with these SQL commands:

```sql
-- Create user
CREATE USER postgres WITH PASSWORD 'example' CREATEDB SUPERUSER;

-- Create database
CREATE DATABASE net_be_genai_plgrnd_db OWNER postgres;

-- Grant privileges
GRANT ALL PRIVILEGES ON DATABASE net_be_genai_plgrnd_db TO postgres;
```
