# SecureProject

SecureProject is a cybersecurity decision support web application built to help organizations, especially SMEs, make informed investment decisions. It enables users to compare cybersecurity products, receive tailored recommendations, and download reports that guide strategic security planning.

## Features

- Product comparison dashboard to evaluate multiple tools side-by-side
- Guided Security Posture Builder that generates recommendations based on user profile
- Search and filtering options by category, deployment type, and more
- Downloadable PDF reports for offline review or stakeholder discussions
- User registration and login for saving comparisons and recommendations
- Admin interface for managing product data

## Tech Stack

- **Frontend**: React with Material UI
- **Backend**: ASP.NET Core 8 (C#) RESTful API
- **Database**: Microsoft SQL Server, accessed via Entity Framework Core
- **Authentication**: JWT-based access control with ASP.NET Identity
- **Other Tools**: Axios (API requests), SelectPdf (PDF generation)

## Installation

### Prerequisites

- .NET 8 SDK
- Node.js v18 or later
- SQL Server 2019 or later
- Git (optional)

### Setup Instructions

1. Clone the repository:
   ```
   git clone https://github.com/mngunawardana/Secure_Project.git
   ```

2. Backend setup:
   ```
   cd backend
   dotnet restore
   # Update appsettings.json with your SQL Server connection string
   dotnet ef database update
   dotnet run
   ```

3. Frontend setup:
   ```
   cd frontend
   npm install
   npm start
   ```

4. Access the app at:
   ```
   http://localhost:3000
   ```

## Usage

- Visit the Home page to learn about the platform
- Navigate to the Compare page to select and compare products
- Use the Posture Builder to generate a tailored security strategy
- View and download results as PDF reports
- Log in to save comparisons and recommendations
- Admin users can manage product listings via a protected dashboard

## Testing

- API tested using Postman and browser dev tools
- Functional testing validated core features and workflows
- Usability tested informally with feedback-driven UI improvements
- Security features include hashed passwords, HTTPS, and JWT protection

## Known Limitations

- Static product dataset used for demo purposes
- Recommendations are rule-based and may not rank multiple options
- No feedback loop or user rating system implemented yet

## Future Work

- Integrate real-time vendor APIs for product data
- Implement ranking and scoring in recommendation engine
- Add user review and rating features
- Improve mobile responsiveness
- Support integration with ITSM tools

## Documentation

- Final report included in project: `10899270 - Final Report.pdf`
- GitHub repository: https://github.com/mngunawardana/Secure_Project.git

## Author

Meth Gunawardana  
BSc (Hons) Computer Security  
Plymouth Index Number: 10899270  
Supervisor: Mr. Chamara Disanayake
