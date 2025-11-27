---
description: Run the full FoodClub application (Frontend + Backend microservices)
---

# Running the Full FoodClub Application

This workflow will help you run the entire FoodClub application stack, including:
- **Frontend**: Next.js application
- **Backend**: Users and Events microservices with PostgreSQL databases

## Prerequisites

Before running the application, ensure you have:
- Docker and Docker Compose installed
- Node.js and npm installed (for frontend development)

## Option 1: Run Everything with Docker (Recommended for Backend)

### 1. Start the Backend Microservices

From the project root directory, run:

```bash
docker-compose up -d
```

This will start:
- **Users API** on `http://localhost:5001`
- **Events API** on `http://localhost:5002`
- **PostgreSQL databases** for both microservices

To view logs:
```bash
docker-compose logs -f
```

To stop the services:
```bash
docker-compose down
```

### 2. Start the Frontend

Navigate to the frontend directory and install dependencies (first time only):

```bash
cd src/foodclub-ui-next
npm install
```

Run the development server:

```bash
npm run dev
```

The frontend will be available at `http://localhost:3000`

## Option 2: Run Backend Locally (Development)

If you prefer to run the microservices locally without Docker:

### 1. Start Users Microservice

```bash
cd src/UsersMicroservice/src/Users.Api
dotnet run
```

### 2. Start Events Microservice (in a new terminal)

```bash
cd src/EventsMicroservice/src/Events.Api
dotnet run
```

**Note**: You'll need to configure PostgreSQL databases manually for this option.

### 3. Start Frontend (in a new terminal)

```bash
cd src/foodclub-ui-next
npm run dev
```

## Verification

Once everything is running, verify:

1. **Frontend**: Open `http://localhost:3000` in your browser
2. **Users API**: Check `http://localhost:5001` (or the port shown in terminal if running locally)
3. **Events API**: Check `http://localhost:5002` (or the port shown in terminal if running locally)

## Troubleshooting

- **Port conflicts**: If ports 3000, 5000, or 5001 are already in use, you'll need to stop other services or modify the ports
- **Docker issues**: Run `docker-compose down -v` to remove volumes and start fresh
- **Frontend dependencies**: If you encounter issues, try deleting `node_modules` and running `npm install` again
