# ⚡ Energy Monitoring Platform – Microservices Architecture

A distributed, event-driven energy monitoring platform built using a **microservices architecture**, combining **Java (Spring Boot)** and **.NET (ASP.NET Core)** services, orchestrated with **Docker Compose**, **RabbitMQ**, **PostgreSQL**, and **Traefik**.

This project simulates a real-world backend system for managing users, devices, and real-time energy consumption analytics.

---

## 🧩 System Overview

The platform consists of multiple independent microservices, each responsible for a specific business domain:

- **Authorization Service** – Authentication & JWT issuance
- **User Service** – User management
- **Device Service** – Device registration & lifecycle
- **Monitoring Service (.NET)** – Energy consumption aggregation & analytics
- **Frontend** – Simple UI for interacting with the system
- **RabbitMQ** – Asynchronous communication between services
- **PostgreSQL** – Shared database infrastructure
- **Traefik** – Reverse proxy & API gateway

All services are containerized and orchestrated via **Docker Compose**.


---

## 🔧 Microservices Breakdown

### 🔐 Authorization Service (Java – Spring Boot)
- Issues and validates JWT tokens
- Central authentication provider
- Integrated with PostgreSQL

### 👥 User Service (Java – Spring Boot)
- Manages users
- Emits `USER_CREATED` events via RabbitMQ
- Protected using JWT authentication

### ⚙️ Device Service (Java – Spring Boot)
- Manages registered devices
- Emits `DEVICE_CREATED` events
- Sends periodic device measurements

### 📈 Monitoring Service (.NET – ASP.NET Core)
> **Reimplemented from Java into .NET to demonstrate cross-stack microservices integration**

Responsibilities:
- Consumes RabbitMQ events (`DEVICE_CREATED`, measurements)
- Aggregates energy consumption **per hour**
- Stores aggregated data in PostgreSQL
- Exposes REST APIs for analytics and charts

Key features:
- ASP.NET Core Web API
- Entity Framework Core (PostgreSQL)
- JWT Authentication
- Swagger / OpenAPI documentation
- Docker multi-stage build
- Integrated into Traefik routing

### 🎨 Frontend
- Simple UI for testing platform flows
- Routed through Traefik

---

## 🐇 Messaging (RabbitMQ)

RabbitMQ is used for **asynchronous, event-driven communication**:

- `USER_CREATED` → consumed by Device Service
- `DEVICE_CREATED` → consumed by Monitoring Service
- Device measurements → consumed by Monitoring Service

This decouples services and mimics real-world scalable systems.

---

## 🗄 Database (PostgreSQL)

- Shared PostgreSQL instance
- Each microservice uses its **own schema / tables**
- Managed via Docker volumes
- Monitoring Service uses EF Core for persistence

---

## 🌐 API Gateway & Routing (Traefik)

Traefik acts as:
- Reverse proxy
- Entry point for all APIs
- Path-based routing

Example routes:
- `/api/auth/**`
- `/api/users/**`
- `/api/devices/**`
- `/api/monitoring/**`

---

## ▶️ How to Run the Project

### 1️⃣ Prerequisites
- Docker & Docker Compose
- Git

### 2️⃣ Start the entire platform

From the **root directory** (where `docker-compose.yml` is located):

```bash
docker compose build
docker compose up
```

### 🔍 Access Points

**Frontend**  
http://localhost/

**Monitoring API (Swagger)**  
http://localhost:8084/swagger

**RabbitMQ Management UI**  
http://localhost:15672  
(user: `guest` / pass: `guest`)

**Traefik Dashboard**  
http://localhost:8080


### 🔐 Security

- JWT-based authentication across services  
- Stateless APIs  
- Role-based access control  
- Sensitive operations protected at both API and service level  


### 🧠 What This Project Demonstrates

- Real-world microservices architecture  
- Event-driven backend design  
- Java & .NET interoperability within the same system  
- Docker & container orchestration using Docker Compose  
- Reverse proxy & API gateway with Traefik  
- Clean separation of concerns  
- Backend engineering best practices  


### 🚀 Future Improvements

- Centralized logging (ELK / OpenTelemetry)  
- Distributed tracing  
- CI/CD pipelines  
- Kubernetes deployment  
- Metrics & monitoring (Prometheus / Grafana)  
- API rate limiting  


### 👤 Author

**Melisa Barna**  
Computer Engineering Student  
Aspiring Backend / .NET Engineer  

This project was built as part of an advanced distributed systems and backend engineering exploration, focusing on scalability, clean architecture, and continuous learning.





