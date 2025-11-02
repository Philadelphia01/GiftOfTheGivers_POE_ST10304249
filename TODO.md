# POE - Gift of the Givers Foundation Unit Testing and Deployment

## New Feature: Search Donations by Resource Type
- [x] Modify DonationController Index action to accept search parameter
- [x] Add filtering logic for resource type search
- [x] Pass search parameter to view via ViewBag

## Recent Progress and Remaining Issues

### Completed
- [x] Created xUnit test project (DisasterAlleviationFoundation.Tests)
- [x] Written unit tests for models (Donation, VolunteerTask, DisasterReport)
- [x] Written unit tests for controllers (HomeController, DonationController - partial)
- [x] Fixed null reference exception in DonationController Details method (duplicate DonorUser loading)
- [x] Added null checks for TempData in DonationController methods
- [x] New Feature: Search Donations by Resource Type implemented

### Not Completed / Remaining
- [ ] Complete unit tests for all controllers (Volunteer, DisasterReport, etc.)
- [ ] Fix failing DonationController tests (2/19 failing due to User.IsInRole null in test context)
- [ ] Achieve high code coverage (>80%)
- [ ] Run unit tests and capture results
- [ ] Write integration tests for database interactions
- [ ] Write integration tests for API endpoints
- [ ] Test user authentication and authorization
- [ ] Run integration tests and capture results
- [ ] Set up Apache JMeter for load testing
- [ ] Perform load and stress testing
- [ ] Set up Selenium for UI testing
- [ ] Conduct usability testing
- [ ] Update Azure DevOps pipeline for CI/CD
- [ ] Generate test reports with code coverage
- [ ] Document usability feedback
- [ ] Provide deployment pipeline YAML configuration

## Testing Tasks

### Unit Testing (15 Marks)
- [ ] Create xUnit test project (DisasterAlleviationFoundation.Tests)
- [ ] Write unit tests for models (validation, properties)
- [ ] Write unit tests for controllers (CRUD operations, authorization)
- [ ] Write unit tests for business logic (if any)
- [ ] Achieve high code coverage (>80%)
- [ ] Run unit tests and capture results

### Integration Testing (15 Marks)
- [ ] Write integration tests for database interactions (EF Core)
- [ ] Write integration tests for API endpoints (controllers)
- [ ] Test data retrieval, updates, deletions
- [ ] Test user authentication and authorization
- [ ] Run integration tests and capture results

### Load and Stress Testing (15 + 15 Marks)
- [ ] Set up Apache JMeter for load testing
- [ ] Simulate concurrent users (e.g., 100 users on donation page)
- [ ] Measure response times, throughput, resource utilization
- [ ] Perform stress testing (extreme conditions, spikes)
- [ ] Identify bottlenecks and failure points
- [ ] Run tests and capture metrics/logs

### User Interface Testing (5 + 5 Marks)
- [ ] Set up Selenium for functional UI testing
- [ ] Test form submissions, navigation, error handling
- [ ] Simulate user interactions (login, create donation, etc.)
- [ ] Conduct usability testing with fictitious users
- [ ] Gather feedback on navigation, layout, accessibility
- [ ] Document feedback and improvement plans

## Deployment Strategies

### Automated Deployment to Azure (10 Marks)
- [ ] Update Azure DevOps pipeline for CI/CD
- [ ] Add build, test, package, deploy stages
- [ ] Configure triggers for automatic deployment
- [ ] Set up Azure App Service deployment
- [ ] Include error handling and rollback mechanisms

## Submission Deliverables

### Test Reports (10 Marks)
- [ ] Generate detailed test reports (code coverage, pass/fail rates)
- [ ] Include screenshots, logs, videos of test execution
- [ ] Document issues and improvements

### Usability Feedback (5 Marks)
- [ ] Summarize fictitious user feedback
- [ ] Discuss plans to address pain points

### Deployment Pipeline Configuration (5 Marks)
- [ ] Provide pipeline YAML configuration
- [ ] Ensure reliable deployment with rollback
