#!/bin/bash

# Sports Hub PostgreSQL Local Setup Script
# This script sets up PostgreSQL locally on macOS for .NET development

set -e  # Exit on any error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Database configuration
DB_NAME="net_be_genai_plgrnd_db"
DB_USER="postgres"
DB_PASSWORD="example"
DB_PORT="5432"

echo -e "${BLUE}🏗️  Sports Hub PostgreSQL Local Setup${NC}"
echo "======================================"

# Function to print colored output
print_status() {
    echo -e "${GREEN}✅ $1${NC}"
}

print_warning() {
    echo -e "${YELLOW}⚠️  $1${NC}"
}

print_error() {
    echo -e "${RED}❌ $1${NC}"
}

print_info() {
    echo -e "${BLUE}ℹ️  $1${NC}"
}

# Check if running on macOS
if [[ "$OSTYPE" != "darwin"* ]]; then
    print_error "This script is designed for macOS. For other operating systems, please install PostgreSQL manually."
    exit 1
fi

# Check if Homebrew is installed
if ! command -v brew &> /dev/null; then
    print_warning "Homebrew is not installed. Installing Homebrew first..."
    /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
    
    # Add Homebrew to PATH for Apple Silicon Macs
    if [[ $(uname -m) == "arm64" ]]; then
        echo 'eval "$(/opt/homebrew/bin/brew shellenv)"' >> ~/.zshrc
        eval "$(/opt/homebrew/bin/brew shellenv)"
    fi
    
    print_status "Homebrew installed successfully"
else
    print_status "Homebrew is already installed"
fi

# Update Homebrew
print_info "Updating Homebrew..."
brew update

# Check if PostgreSQL is already installed
if brew list postgresql@17 &>/dev/null; then
    print_status "PostgreSQL 17 is already installed"
else
    print_info "Installing PostgreSQL 17..."
    brew install postgresql@17
    print_status "PostgreSQL 17 installed successfully"
fi

# Start PostgreSQL service
print_info "Starting PostgreSQL service..."
if brew services list | grep postgresql@17 | grep started > /dev/null; then
    print_status "PostgreSQL service is already running"
else
    brew services start postgresql@17
    print_status "PostgreSQL service started"
fi

# Wait a moment for PostgreSQL to start
sleep 3

# Add PostgreSQL to PATH if not already there
PG_PATH="/opt/homebrew/opt/postgresql@17/bin"
if [[ ":$PATH:" != *":$PG_PATH:"* ]]; then
    print_info "Adding PostgreSQL to PATH..."
    echo "export PATH=\"$PG_PATH:\$PATH\"" >> ~/.zshrc
    export PATH="$PG_PATH:$PATH"
    print_status "PostgreSQL added to PATH"
fi

# Function to run SQL commands
run_sql() {
    local sql="$1"
    local db="${2:-postgres}"
    echo "$sql" | psql -h localhost -p $DB_PORT -U $(whoami) -d "$db" -v ON_ERROR_STOP=1
}

# Create database and user
print_info "Setting up database and user..."

# Check if we can connect to PostgreSQL
if ! psql -h localhost -p $DB_PORT -U $(whoami) -d postgres -c '\q' 2>/dev/null; then
    print_error "Cannot connect to PostgreSQL. Please check if the service is running."
    print_info "Try running: brew services restart postgresql@17"
    exit 1
fi

# Create the postgres user if it doesn't exist
print_info "Creating database user '$DB_USER'..."
if psql -h localhost -p $DB_PORT -U $(whoami) -d postgres -t -c "SELECT 1 FROM pg_roles WHERE rolname='$DB_USER'" | grep -q 1; then
    print_status "User '$DB_USER' already exists"
else
    run_sql "CREATE USER $DB_USER WITH PASSWORD '$DB_PASSWORD' CREATEDB SUPERUSER;"
    print_status "User '$DB_USER' created successfully"
fi

# Create the database if it doesn't exist
print_info "Creating database '$DB_NAME'..."
if psql -h localhost -p $DB_PORT -U $(whoami) -d postgres -lqt | cut -d \| -f 1 | grep -qw $DB_NAME; then
    print_status "Database '$DB_NAME' already exists"
else
    run_sql "CREATE DATABASE $DB_NAME OWNER $DB_USER;"
    print_status "Database '$DB_NAME' created successfully"
fi

# Grant privileges
print_info "Granting privileges..."
run_sql "GRANT ALL PRIVILEGES ON DATABASE $DB_NAME TO $DB_USER;"
print_status "Privileges granted successfully"

# Test the connection with the new user
print_info "Testing database connection..."
if PGPASSWORD=$DB_PASSWORD psql -h localhost -p $DB_PORT -U $DB_USER -d $DB_NAME -c '\q' 2>/dev/null; then
    print_status "Database connection test successful!"
else
    print_error "Database connection test failed"
    exit 1
fi

# Display connection information
echo ""
echo -e "${BLUE}🎉 PostgreSQL Setup Complete!${NC}"
echo "================================"
echo "Database Name: $DB_NAME"
echo "Username: $DB_USER"
echo "Password: $DB_PASSWORD"
echo "Host: localhost"
echo "Port: $DB_PORT"
echo ""
echo -e "${BLUE}Connection String:${NC}"
echo "Server=localhost;Database=$DB_NAME;User ID=$DB_USER;Password=$DB_PASSWORD;Port=$DB_PORT"
echo ""
echo -e "${BLUE}Manual Connection:${NC}"
echo "psql -h localhost -p $DB_PORT -U $DB_USER -d $DB_NAME"
echo ""
echo -e "${BLUE}Next Steps:${NC}"
echo "1. Navigate to your .NET project: cd ../SportsHub.Api"
echo "2. Run the application: dotnet run"
echo "3. Migrations will be applied automatically on startup"
echo ""
echo -e "${YELLOW}Note:${NC} PostgreSQL service will start automatically on system boot."
echo "To stop: brew services stop postgresql@17"
echo "To restart: brew services restart postgresql@17" 