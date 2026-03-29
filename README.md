# FlowDesk Task Board - Backend API

**Project:** FlowDesk Task Board
**Technology:** ASP.NET Core 9
**IDE:** Visual Studio 2022
**Database:** SQL Server

---

## 1. Project Overview

This is the backend service for **FlowDesk Task Board**, a lightweight task and project management system for distributed teams. The service supports:

* Task creation, assignment, priority, and due dates
* Project overview with filtering and sorting
* Task management and workflow (To Do → In Progress → Done)
* Archiving completed or canceled tasks
* Role-based access control
* Validation and error handling

Authentication uses **Token Cookie-based sessions**. Users must register and login to use the system.

---

## 2. Prerequisites

* Visual Studio 2022 (latest update)
* .NET 9 SDK
* SQL Server instance (local or remote)

---

## 3. Getting Started

### Step 1: Clone the Repository

```bash
git clone https://github.com/nimeshajpn2/flowdesk-taskboard.git
cd FlowDeskTaskboardApi
```

### Step 2: Configure the Database

1. Open `appsettings.json`.
2. Add your SQL Server connection string under `ConnectionStrings`.
3. Make sure the database `flowdesk_taskboard_dev_db` exists (create it if needed).

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=flowdesk_taskboard_dev_db;Trusted_Connection=True;"
}
```

### Step 3: Run EF Core Migrations

* Open **Tools > NuGet Package Manager > Package Manager Console** in Visual Studio.
* Run:

```powershell
dotnet ef database update
```

This creates all tables and seeds initial data.

### Step 4: Run the Project

* Press `F5` or run the project from Visual Studio.
* The API will start, typically at `https://localhost:5001`.

---

## 4. Authentication

* Users must **register first**.
* Then **login** to get access using Token Cookie authentication.

---

## 5. Notes

* Ensure your SQL Server connection in `appsettings.json` is correct.
* EF Core migrations must run before starting the application.
* Use Postman or another API client to explore endpoints.

---

## 6. Future Improvements (Optional)

* Pagination & filtering for task/project lists
* Unit & integration tests for key business logic
* Global exception handling and structured logging
* Deploy to cloud provider for live testing

---

**Enjoy managing tasks efficiently with FlowDesk!**
