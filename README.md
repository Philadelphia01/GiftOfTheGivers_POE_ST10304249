# 🌟 Gift of the givers aplication

A comprehensive, secure web application designed to support disaster relief operations through efficient resource management, volunteer coordination, and incident reporting. Built with modern web technologies and enterprise-level security features.

---

## 🚀 **Application Overview**

The Disaster Alleviation Foundation platform is a full-featured web application that enables organizations to:
- **Manage disaster incidents** with detailed reporting and tracking
- **Coordinate donations** from multiple sources with inventory management
- **Organize volunteer activities** with task assignment and communication
- **Facilitate secure communication** between volunteers and administrators
- **Maintain user privacy** with comprehensive data isolation

---

## ✨ **Key Features**

### 🔐 **Security & Authentication**
- **Azure Active Directory Integration** - Enterprise-grade authentication
- **ASP.NET Core Identity** - User management and role-based access
- **User Data Isolation** - Each user can only access their own data
- **Admin Role Management** - Special privileges for administrators
- **Secure Session Management** - Protected user sessions

### 📊 **Disaster Management**
- **Incident Reporting** - Submit detailed disaster reports with location and type
- **Report Management** - Edit, view, and delete personal reports
- **Admin Oversight** - Administrators can view all reports for coordination
- **Real-time Updates** - Track incident status and response progress

### 🎁 **Donation Management**
- **Resource Tracking** - Manage donations by type, quantity, and date
- **Donation Status** - Track pending, approved, and distributed donations
- **Inventory System** - Real-time inventory management for administrators
- **Distribution Tracking** - Monitor donation distribution to beneficiaries
- **User Donations** - Users can only manage their own donations

### 👥 **Volunteer Coordination**
- **Task Management** - Create, assign, and track volunteer tasks
- **Personal Dashboard** - Each volunteer sees only their assigned tasks
- **Task Categories** - Organize tasks by type and priority
- **Progress Tracking** - Monitor task completion and status
- **Communication System** - Secure messaging between volunteers

### 💬 **Communication System**
- **Secure Messaging** - Internal communication platform
- **Message Categories** - General, Task Updates, Emergency, Announcements
- **Read Receipts** - Track message delivery and reading status
- **Task-Related Messages** - Link messages to specific volunteer tasks

### 🎨 **User Interface**
- **Responsive Design** - Works on desktop, tablet, and mobile devices
- **Bootstrap 5** - Modern, professional styling
- **Intuitive Navigation** - Easy-to-use interface for all user types
- **Real-time Feedback** - Success and error messages for user actions
- **Accessibility** - Designed with accessibility best practices

### 🎛️ **Admin Dashboard**
- **Modern Analytics Interface** - Clean, professional dashboard design
- **Statistics Overview** - Real-time metrics for users, donations, reports, and tasks
- **Quick Action Buttons** - Direct access to "View Incidents", "View Donations", "Manage Volunteers"
- **Recent Activity Feed** - Live updates on recent donations and disaster reports
- **Sidebar Navigation** - Easy access to all admin functions
- **No Navbar/Footer** - Dedicated full-screen admin experience
- **Automatic Redirection** - Admins are taken directly to dashboard upon login

---

## 🛠 **Technology Stack**

### **Backend Technologies**
- **ASP.NET Core 8.0** - Modern web framework
- **Entity Framework Core 8.0** - Object-relational mapping
- **SQL Server** - Robust database management
- **Azure SQL Database** - Cloud database hosting
- **C# 12** - Latest language features

### **Frontend Technologies**
- **Razor Pages** - Server-side rendering
- **Bootstrap 5.3** - Responsive CSS framework
- **Bootstrap Icons** - Comprehensive icon library
- **jQuery** - JavaScript functionality
- **HTML5 & CSS3** - Modern web standards

### **Authentication & Security**
- **Microsoft Identity Web** - Azure AD integration
- **ASP.NET Core Identity** - User management
- **JWT Tokens** - Secure authentication
- **Data Protection API** - Secure data handling

### **Development Tools**
- **Visual Studio 2022** - Integrated development environment
- **Entity Framework Migrations** - Database schema management
- **Azure DevOps** - CI/CD pipeline and source control
- **Git** - Version control system

---

## 📋 **Prerequisites**

### **Required Software**
- **[.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)** - Latest version
- **[Visual Studio 2022](https://visualstudio.microsoft.com/)** - Community, Professional, or Enterprise
- **[Azure SQL Database](https://azure.microsoft.com/products/sql-database/)** - Cloud database service
- **[Git](https://git-scm.com/)** - Version control
- **[SQL Server](https://www.microsoft.com/sql-server)** - LocalDB, Express, or full version (for local development)

### **Optional Tools**
- **[Azure Data Studio](https://azure.microsoft.com/products/data-studio/)** - Database management
- **[Postman](https://www.postman.com/)** - API testing
- **[Azure CLI](https://docs.microsoft.com/cli/azure/)** - Azure resource management

### **Azure Services**
- **Azure Active Directory** - For authentication (optional, can use local accounts)
- **Azure SQL Database** - For production deployment
- **Azure App Service** - For hosting (optional)

---

## 🚀 **Installation & Setup**

### **1. Clone the Repository**
```bash
git clone https://dev.azure.com/ST10304249/Gift_Of_The_Givers_Foundation/_git/Gift_Of_The_Givers_Foundation
cd DisasterAlleviationFoundation
```

### **2. Configure Database Connection**
Update `appsettings.json` with your Azure SQL Database connection:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:your-server.database.windows.net,1433;Initial Catalog=DisasterAlleviationDb;Persist Security Info=False;User ID=your-username;Password=your-password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}
```

### **4. Install Dependencies**
```bash
dotnet restore
```

### **5. Setup Database**
```bash
# Create and apply database migrations to Azure SQL Database
dotnet ef database update

# For fresh setup (be careful - this will delete all data)
dotnet ef database drop --force
dotnet ef database update

# Note: Ensure your Azure SQL Database is accessible and the connection string is correct
# The application will automatically seed an admin user on first run
```

### **6. Build the Application**
```bash
dotnet build
```

---

## ▶️ **Running the Application**

### **Development Mode**
```bash
# Run with default settings
dotnet run

# Run on specific port (recommended)
dotnet run --urls http://localhost:5186

# Run with hot reload
dotnet watch run --urls http://localhost:5186

# Run with Development environment
$env:ASPNETCORE_ENVIRONMENT="Development"; dotnet run --no-launch-profile --urls http://localhost:5186
```

### **Production Mode**
```bash
# Build for production
dotnet publish -c Release -o ./publish

# Run published application
cd publish
dotnet DisasterAlleviationFoundation.dll
```

### **Access the Application**
- **Local Development**: `http://localhost:5186` (or your configured port)
- **HTTPS**: `https://localhost:7186` (with SSL certificate)

### **🚀 Quick Login Reference**
| User Type | Email | Password | Access Level |
|-----------|-------|----------|--------------|
| **Admin** | `admin@disasterrelief.com` | `Admin123!` | Full system access + Admin Dashboard |
| **Regular User** | *Your registered email* | *Your password* | Personal data only |

**Admin Features**: Analytics dashboard, view all data, manage inventory, system oversight  
**User Features**: Personal donations, reports, tasks, messaging (isolated data)

---

## 👤 **User Accounts & Login Guide**

### **🔐 How to Login as Admin**
1. **Navigate to the Application**: Open `http://localhost:5186` in your browser
2. **Click Login**: Click the "Login" button in the top navigation bar
3. **Enter Admin Credentials**:
   - **Email**: `admin@disasterrelief.com`
   - **Password**: `Admin123!`
4. **Automatic Redirection**: Upon successful login, you'll be automatically redirected to the **Admin Dashboard**
5. **Admin Dashboard Features**:
   - View system statistics (users, donations, reports, tasks)
   - Access "View Incidents", "View Donations", "Manage Volunteers" buttons
   - See recent activity feed
   - Use "View Website" button to access the main site

### **👥 How to Login as Regular User**
1. **Navigate to the Application**: Open `http://localhost:5186` in your browser
2. **Register New Account** (if you don't have one):
   - Click "Register" in the top navigation
   - Fill in your details:
     - **Full Name**: Your full name
     - **Email**: Your email address
     - **Password**: Choose a secure password
     - **Confirm Password**: Repeat your password
   - Click "Register" to create your account
3. **Login with Your Account**:
   - Click "Login" in the top navigation
   - Enter your email and password
   - Click "Log in"


### **🧪 Testing User Isolation**
 
   **Sample Test User Details**:
   - **Full Name**: Test User
   - **Email**: testuser@gmail.com
   - **Password**: 123456

### **🎯 Login Troubleshooting**
- **Forgot Admin Password**: The admin password is `Admin123!` (case-sensitive)
- **Registration Issues**: Ensure all required fields are filled
- **Login Failures**: Check email format and password requirements
- **Account Locked**: Contact system administrator or check logs

### **Admin Account**
A default admin account is automatically created when you first run the application:
- **Email**: `admin@disasterrelief.com`
- **Password**: `Admin123!`
- **Role**: Administrator with full system access



## 🔒 **Security Features**

### **Implemented Security Measures**
- ✅ **User Authentication** - Required for all operations
- ✅ **Data Isolation** - Users can only access their own data
- ✅ **Authorization Checks** - Proper permission validation
- ✅ **Input Validation** - Protection against malicious input
- ✅ **CSRF Protection** - Anti-forgery tokens on forms
- ✅ **SQL Injection Prevention** - Parameterized queries via EF Core

### **Admin Privileges**
- View all donations and disaster reports
- Manage donation distribution
- Access inventory management
- System-wide oversight capabilities

---

## 🧪 **Testing & Quality Assurance**


### **Common Issues**

#### **Database Connection Issues**
```bash
# Check connection string in appsettings.json
# Ensure SQL Server is running
# Verify database exists
dotnet ef database update
```

#### **Migration Issues**
```bash
# Reset migrations
dotnet ef database drop --force
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

#### **Port Conflicts**
```bash
# Use different port
dotnet run --urls http://localhost:5183
```


## 📚 **Additional Resources**

### **Documentation**
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Azure Active Directory](https://docs.microsoft.com/azure/active-directory/)
- [Bootstrap Documentation](https://getbootstrap.com/docs/)


## 📞 **Support & Contact**

For technical support or questions about this application:

- **Repository**: [Azure DevOps Repository](https://dev.azure.com/ST10304249/Gift_Of_The_Givers_Foundation/_git/Gift_Of_The_Givers_Foundation)
- **Issues**: Create issues in the repository for bug reports
- **Documentation**: Refer to this README and inline code comments

---

## 🎯 **Project Status**

**Current Version**: 1.0.0  
**Status**: ✅ Production Ready  
**Last Updated**: October 2024  

### **Completed Features**
- ✅ User authentication and registration
- ✅ Disaster incident reporting and management
- ✅ Donation tracking and inventory management
- ✅ Volunteer task coordination
- ✅ Secure messaging system
- ✅ User data isolation and security
- ✅ Responsive web design
- ✅ Admin role functionality
- ✅ **Modern Admin Dashboard** - Analytics-style interface
- ✅ **Azure SQL Database** - Cloud database integration
- ✅ Azure DevOps CI/CD pipeline

### **Future Enhancements**
- 📱 Mobile application
- 🌍 Multi-language support
- 📊 Advanced reporting and analytics
- 🔔 Real-time notifications
- 📧 Email integration
- 🗺️ Geographic mapping integration

---


