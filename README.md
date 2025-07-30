# Job Platform API

A comprehensive .NET Core Web API for managing job postings, user registration, and job applications. This platform enables administrators to post jobs, users to apply for positions, and manage their profiles.

## Features

- **User Management**: User registration, login, and profile management
- **Job Management**: Job posting and retrieval with application tracking
- **Admin Panel**: Administrative controls for managing users and job postings
- **Profile System**: User profile creation and management
- **Job Applications**: Apply to jobs and track applications

## Technology Stack

- **.NET Core/ASP.NET Core** - Backend framework
- **RESTful API** - API architecture
- **Dependency Injection** - Service management


## API Endpoints

### User Management (`/user/user`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/user/user/register` | Register a new user |
| POST | `/user/user/login` | User authentication |
| GET | `/user/user/{id}` | Get user by ID |

### Job Management (`/job/job`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/job/job/{id}` | Get job details by ID |
| POST | `/job/job/apply` | Apply to a specific job |

### Profile Management (`/api/profile`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/profile/{userId}` | Get user profile |
| POST | `/api/profile` | Create or update user profile |

### Admin Operations (`/admin/admin`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/admin/admin/users` | Get all users (admin only) |
| POST | `/admin/admin/post-job` | Create a new job posting |

## Request/Response Examples

### User Registration
```json
POST /user/user/register
{
    "email": "user@example.com",
    "password": "securePassword123",
    "name": "John Doe"
}
```

### User Login
```json
POST /user/user/login
{
    "email": "user@example.com",
    "password": "securePassword123"
}
```

### Job Application
```json
POST /job/job/apply
{
    "jobId": "job123",
    "userId": "user456"
}
```

### Create Job Posting (Admin)
```json
POST /admin/admin/post-job
 {
    "Title": "Frontend Developer",
    "Role": "React Developer",
    "Description": "We are looking for a skilled frontend developer to join our team.",
    "Location": "Mumbai, India",
    "Salary": 850000,
    "Experience": 2,
    "JobType": "Full-Time",
    "Skills": ["React", "JavaScript", "HTML", "CSS"],
    "PostedDate": { "$date": "2025-07-30T10:00:00Z" }
  }
```

## Getting Started

### Prerequisites

- .NET 6.0 or later
- MongoDB
- Visual Studio 2022 or VS Code


The API will be available at ` https://localhost:7093` or `[http://localhost:5000](http://localhost:5210)`

## Configuration

Update `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "MongoDBSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "JobApplication-dotnet",
    "JobCollectionName": "Jobs",
    "UserCollectionName": "Users",
    "ProfileCollection" :  "Profiles"

  },
  "AllowedHosts": "*"
}

```

## Architecture

The application follows a clean architecture pattern:

- **Controllers**: Handle HTTP requests and responses
- **Services**: Contain business logic and data operations
- **Models**: Define data structures and entities
- **Dependency Injection**: Manages service lifetimes and dependencies

## Services

- **UserService**: Handles user registration, authentication, and management
- **JobService**: Manages job postings and applications
- **ProfileService**: Handles user profile operations

## Error Handling

The API returns appropriate HTTP status codes:

- `200 OK`: Successful operation
- `400 Bad Request`: Invalid request data
- `401 Unauthorized`: Authentication required
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Server error

### Production Deployment

1. Update `appsettings.Production.json` with production settings
2. Build the application:
   ```bash
   dotnet publish -c Release
   ```
3. Deploy to your preferred hosting platform (Azure, AWS, etc.)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.



---

**Note**: This is a backend API. For a complete solution, you'll need a frontend application to consume these endpoints.
