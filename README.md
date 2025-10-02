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

**For Local Development (Optional):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DisasterAlleviationDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### **3. Configure Azure AD (Optional)**
Update `appsettings.json` for Azure AD integration:
```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "yourtenant.onmicrosoft.com",
    "TenantId": "your-tenant-id-guid",
    "ClientId": "your-client-id-guid",
    "CallbackPath": "/signin-oidc"
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

# Run on specific port
dotnet run --urls http://localhost:5182

# Run with hot reload
dotnet watch run
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
- **Local Development**: `http://localhost:5182`
- **HTTPS**: `https://localhost:7182` (with SSL certificate)

---

## 👤 **User Accounts & Testing**

### **Creating Test Accounts**
1. Navigate to the registration page
2. Create multiple user accounts for testing
3. Test user isolation by logging in as different users

### **Testing User Isolation**
1. **User A**: Create donations, tasks, and reports
2. **User B**: Verify they cannot see User A's data
3. **URL Testing**: Try accessing other users' data via URL manipulation
4. **Admin Testing**: Test admin privileges (if configured)

### **Sample Test Data**
```
User 1: john.doe@example.com
User 2: jane.smith@example.com
Admin: admin@disasterrelief.com / Admin123!
```

### **Admin Account**
A default admin account is automatically created when you first run the application:
- **Email**: `admin@disasterrelief.com`
- **Password**: `Admin123!`
- **Role**: Administrator with full system access

**Admin Features:**
- **Modern Admin Dashboard** - Analytics-style interface with statistics and recent activity
- **Direct Dashboard Access** - Admins are automatically redirected to the dashboard upon login
- **View All Data** - Access to all donations, disaster reports, and volunteer tasks from all users
- **Inventory Management** - Real-time inventory tracking and distribution management
- **User Management** - Oversee volunteer coordination and task assignments
- **System Analytics** - Dashboard with key metrics and performance indicators
- **Quick Actions** - Dedicated buttons for "View Incidents", "View Donations", and "Manage Volunteers"
- **Website Access** - "View Website" button to access the main public site when needed

---

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

### **Manual Testing Checklist**
- [ ] User registration and login
- [ ] Data isolation between users
- [ ] CRUD operations for all entities
- [ ] Admin functionality (if applicable)
- [ ] Responsive design on different devices
- [ ] Security: unauthorized access attempts

### **Database Testing**
```bash
# Reset database for fresh testing
dotnet ef database drop --force
dotnet ef database update
```

### **Security Testing**
- Test user isolation by attempting to access other users' data
- Verify authentication requirements on all protected pages
- Test authorization for admin-only features

---

## 🐛 **Troubleshooting**

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

#### **Authentication Issues**
- Verify Azure AD configuration
- Check callback URLs
- Ensure proper permissions

---

## 📚 **Additional Resources**

### **Documentation**
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Azure Active Directory](https://docs.microsoft.com/azure/active-directory/)
- [Bootstrap Documentation](https://getbootstrap.com/docs/)

### **Learning Resources**
- [Microsoft Learn - ASP.NET Core](https://docs.microsoft.com/learn/paths/aspnet-core-web-app/)
- [Entity Framework Core Tutorial](https://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx)
- [Azure Documentation](https://docs.microsoft.com/azure/)

---

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

*Built with ❤️ for disaster relief and community support*

