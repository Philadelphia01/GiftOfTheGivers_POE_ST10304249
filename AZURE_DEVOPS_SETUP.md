# Azure DevOps Setup Guide - Disaster Alleviation Foundation

## Overview
This document outlines the Azure DevOps implementation for the Disaster Alleviation Foundation web application, including Git repository setup, branching strategy, and CI/CD pipeline configuration.

---

## 1. Git Repository Setup (10 Marks)

### Repository Information
- **Repository Name**: DisasterAlleviationFoundation
- **Platform**: Azure Repos (Azure DevOps)
- **Repository URL**: `https://dev.azure.com/{your-organization}/{project-name}/_git/DisasterAlleviationFoundation`

### Branching Strategy: GitFlow Implementation

#### Branch Structure:
```
main (production-ready code)
├── develop (integration branch)
│   ├── feature/user-authentication
│   ├── feature/disaster-reporting
│   ├── feature/donation-management
│   ├── feature/volunteer-coordination
│   └── feature/messaging-system
├── hotfix/critical-bug-fixes (if needed)
└── release/v1.0.0 (release preparation)
```

#### Branch Descriptions:
- **`main`**: Production-ready code, protected branch
- **`develop`**: Integration branch for features, auto-deploys to test environment
- **`feature/*`**: Individual feature development branches
- **`hotfix/*`**: Critical production fixes
- **`release/*`**: Release preparation and testing

### Repository Setup Steps:

1. **Initialize Local Git Repository**:
   ```bash
   cd DisasterAlleviationFoundation
   git init
   git add .
   git commit -m "Initial commit: ASP.NET Core web application with complete functionality"
   ```

2. **Connect to Azure Repos**:
   ```bash
   git remote add origin https://dev.azure.com/{your-org}/{project}/_git/DisasterAlleviationFoundation
   git branch -M main
   git push -u origin main
   ```

3. **Create Development Branch**:
   ```bash
   git checkout -b develop
   git push -u origin develop
   ```

### Commit Message Standards:
- **Format**: `type(scope): description`
- **Examples**:
  - `feat(auth): implement Azure AD authentication`
  - `fix(donations): resolve donation form validation issue`
  - `docs(readme): update setup instructions`
  - `refactor(ui): remove sidebar from volunteer pages`

---

## 2. Azure Pipelines Configuration (10 Marks)

### Pipeline Overview
- **File**: `azure-pipelines.yml`
- **Triggers**: Automated builds on push to main, develop, and feature branches
- **Stages**: Build → Test → Deploy to Test → Deploy to Production

### Pipeline Features:

#### Build Stage:
- ✅ .NET 8.0 SDK installation
- ✅ NuGet package restoration
- ✅ Application compilation
- ✅ Unit test execution (with code coverage)
- ✅ Application publishing
- ✅ Artifact creation

#### Deployment Stages:
- ✅ **Test Environment**: Auto-deploy from `develop` branch
- ✅ **Production Environment**: Auto-deploy from `main` branch
- ✅ **Environment Protection**: Manual approval gates for production

#### Quality Gates:
- ✅ Build must pass before deployment
- ✅ Tests must pass (if available)
- ✅ SonarQube code quality analysis
- ✅ Security scanning

### Pipeline Configuration Details:

```yaml
# Key Pipeline Settings
trigger:
  branches:
    include: [main, develop, feature/*]
    
pool:
  vmImage: 'ubuntu-latest'
  
variables:
  buildConfiguration: 'Release'
  dotNetVersion: '8.0.x'
```

### Deployment Environments:

#### Test Environment:
- **App Service**: `disaster-alleviation-test`
- **Database**: Azure SQL Database (Test)
- **URL**: `https://disaster-alleviation-test.azurewebsites.net`
- **Auto-Deploy**: Yes (from develop branch)

#### Production Environment:
- **App Service**: `disaster-alleviation-prod`
- **Database**: Azure SQL Database (Production)
- **URL**: `https://disaster-alleviation-prod.azurewebsites.net`
- **Auto-Deploy**: Yes (from main branch, with approval)

---

## 3. Collaboration Features (10 Marks)

### Pull Request Process:
1. **Feature Development**: Create feature branch from `develop`
2. **Code Review**: Submit pull request to merge into `develop`
3. **Review Requirements**:
   - Minimum 2 reviewers required
   - All comments must be resolved
   - Build pipeline must pass
   - Code coverage threshold: 80%

### Branch Protection Rules:
- **Main Branch**:
  - No direct commits allowed
  - Requires pull request
  - Requires 2 approvals
  - Requires passing build
  
- **Develop Branch**:
  - Requires pull request
  - Requires 1 approval
  - Requires passing build

### Team Collaboration:
- **Code Reviews**: Mandatory for all changes
- **Work Items**: Linked to commits and pull requests
- **Notifications**: Automated via Teams/Email
- **Documentation**: Updated with each feature

---

## 4. Implementation Checklist

### ✅ Completed:
- [x] ASP.NET Core 8.0 application development
- [x] Entity Framework Core with migrations
- [x] Azure AD authentication integration
- [x] Complete UI with Bootstrap styling
- [x] Pipeline YAML configuration created
- [x] .gitignore file configured
- [x] Documentation prepared

### 🔄 To Complete (Critical for Assignment):

#### Git Repository (Required for 10 Marks):
- [ ] Create Azure DevOps project
- [ ] Initialize Git repository locally
- [ ] Push code to Azure Repos
- [ ] Create develop branch
- [ ] Set up branch protection policies
- [ ] Create at least 3 feature branches
- [ ] Demonstrate pull request workflow

#### Azure Pipelines (Required for 10 Marks):
- [ ] Import azure-pipelines.yml to Azure DevOps
- [ ] Configure service connections for Azure
- [ ] Create test and production environments
- [ ] Set up deployment slots in Azure App Service
- [ ] Configure pipeline triggers
- [ ] Execute successful build and deployment
- [ ] Document pipeline runs with screenshots

---

## 5. Required Deliverables

### For Submission:
1. **Repository Link**: Accessible Azure Repos URL
2. **Pipeline Configuration**: Working azure-pipelines.yml
3. **Build Logs**: Screenshots of successful builds
4. **Deployment Evidence**: Screenshots of deployed applications
5. **Branch Structure**: Evidence of GitFlow implementation
6. **Pull Requests**: Examples of code review process

### Documentation Requirements:
- Repository access for instructor and teammates
- Descriptive commit messages throughout history
- README.md with setup instructions
- Pipeline configuration explanation
- Deployment environment details

---

## 6. Next Steps for Full Implementation

### Immediate Actions Required:
1. **Create Azure DevOps Organization/Project**
2. **Set up Azure Repos repository**
3. **Configure Azure Pipelines**
4. **Create Azure App Service instances**
5. **Set up service connections**
6. **Execute first successful deployment**

### Timeline Recommendation:
- **Day 1**: Azure DevOps setup and repository creation
- **Day 2**: Pipeline configuration and testing
- **Day 3**: Deployment setup and validation
- **Day 4**: Documentation and screenshots
- **Day 5**: Final testing and submission preparation

---

## Contact Information
- **Project**: Disaster Alleviation Foundation
- **Technology Stack**: ASP.NET Core 8.0, Entity Framework Core, Azure AD
- **Repository**: [To be provided after Azure Repos setup]
- **Pipeline**: [To be provided after Azure Pipelines setup]

---

*This document will be updated with actual URLs and screenshots once the Azure DevOps implementation is complete.*
