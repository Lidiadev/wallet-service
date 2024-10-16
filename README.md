## Architecture Overview
Thw Wallet Service is designed to handle funds for users in a sports betting platform. 
It provides functionality for creating wallets, adding and removing funds, and checking the wallet state. 

### Key Components
This project follows Clean Architecture principles:

- **API Layer**: RESTful API endpoints for client interaction.
- **Application Layer**: Contains command and query handlers.
- **Domain Layer**: Core business logic and entities.
- **Infrastructure Layer**: Data access and caching.

### Technologies Used
- ASP.NET Core
- Entity Framework Core
- SQL Server
- Redis
- MediatR

Caching Strategy: Redis caching has been used to reduce database load for frequent balance checks.

### Future improvements 
- Add UTs and integration tests

# Running the solution
## Prerequisites

Before you start, ensure you have the following installed:

- [Docker](https://docs.docker.com/get-docker/)
- [Docker Compose](https://docs.docker.com/compose/install/)

## Getting Started

Follow these steps to run the Wallet API using Docker Compose:

### 1. Clone the Repository

Clone the project repository to your local machine:

```bash
git clone https://github.com/Lidiadev/wallet-service
cd <repository-directory>
```

### 2. Set Up Environment Variables

Replace the `TBD` values in the `.env.local` file in the root of the project directory with your actual database connection string and Redis connection string:

```dotenv
DEFAULT_CONNECTION_STRING=TBD
REDIS_CONNECTION_STRING=TBD
SA_PASSWORD=TBD
```

### 3. Build and Run the Services

Use Docker Compose to build and run the services defined in the `docker-compose.yml` file. In the root of the project directory, run the following command:

```bash
docker-compose up --build
```

This command will:

- Build the `wallet.api` image from the specified Dockerfile.
- Start the `wallet.api`, `db` (MSSQL), and `redis` (Redis) services.

### 4. Access the Services

Once the services are running, you can access the Wallet API through the specified port:

- `http://localhost:5023`

You can access the Wallet API Swagger using the following link:
You can access the Wallet API Swagger using the following link:

- `http://localhost:5023/swagger/index.html`