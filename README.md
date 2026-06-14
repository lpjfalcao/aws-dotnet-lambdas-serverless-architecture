# 🚀 AWS .NET Lambdas Serverless Architecture

A production-ready cloud architecture leveraging **AWS Lambda** functions built with **.NET 8** (or .NET 9) and structured under the rigorous principles of **Clean Architecture**. This repository serves as a blueprint for deploying highly scalable, decoupled, and maintainable serverless microservices.

<p align="center">
  <img src="https://img.shields.io/badge/AWS-232F3E?style=for-the-badge&logo=amazon-aws&logoColor=white" alt="AWS" />
  <img src="https://img.shields.io/badge/AWS_Lambda-FF9900?style=for-the-badge&logo=amazon-lambda&logoColor=white" alt="AWS Lambda" />
  <img src="https://img.shields.io/badge/.NET_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET" />
  <img src="https://img.shields.io/badge/Clean_Architecture-8A2BE2?style=for-the-badge" alt="Clean Architecture" />
</p>

---

## 📝 Project Description

This repository demonstrates how to combine the rapid scalability of **AWS Serverless compute** with the long-term maintainability of **Clean Architecture (Hexagonal/Onion Architecture)**. By segregating business rules from infrastructure concerns, this design ensures that the core domain logic remains isolated, highly testable, and completely independent of external framework dependencies or cloud provider specifics.

### Key Highlights
* **Domain-Driven Design (DDD) Influenced:** Isolated Core Domain containing business logic, aggregates, and specifications.
* **Low Cold-Start Tailoring:** Optimized C# compilation targets tailored specifically for fast serverless lifecycle execution.
* **Decoupled Messaging:** Out-of-the-box support for event-driven flows using cloud native patterns.

---

## 🗺️ Architectural Structure

The codebase is organized into multi-project layers enforcing strict dependency boundaries:

```text
├── 📂 src
│   ├── 🏢 Domain         # Core business logic, Entities and Interfaces
│   ├── ⚙️ Application    # Use Cases, DTOs, and Ports
│   ├── ☁️ Infrastructure # Database Contexts, External API Clients, AWS Service Implementations
│   └── ⚡ Presentation    # AWS Lambda Function Handlers (API Gateway / SQS / EventBridge triggers)
└── 📂 tests              # Unit, Integration, and Architecture-guard automated test suites
