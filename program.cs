using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Developer EF Core exception filter removed for simplified local setup

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

// Identity + EF Core store
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Azure AD (Microsoft Identity Web) - DISABLED for now
// builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

// Razor Pages (for Identity UI)
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection(); // Disabled to avoid redirect issues
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Apply migrations and seed admin role/user
using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
	
	dbContext.Database.Migrate();
	
	// Seed Admin Role
	await SeedAdminRoleAndUser(userManager, roleManager);
}

// Method to seed admin role and user
async Task SeedAdminRoleAndUser(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
	// Create Admin role if it doesn't exist
	if (!await roleManager.RoleExistsAsync("Admin"))
	{
		await roleManager.CreateAsync(new IdentityRole("Admin"));
		Console.WriteLine("Admin role created successfully.");
	}
	
	// Create default admin user if it doesn't exist
	var adminEmail = "admin@disasterrelief.com";
	var adminUser = await userManager.FindByEmailAsync(adminEmail);
	
	if (adminUser == null)
	{
		adminUser = new ApplicationUser
		{
			UserName = adminEmail,
			Email = adminEmail,
			FullName = "System Administrator",
			EmailConfirmed = true
		};
		
		var result = await userManager.CreateAsync(adminUser, "Admin123!");
		
		if (result.Succeeded)
		{
			await userManager.AddToRoleAsync(adminUser, "Admin");
			Console.WriteLine($"Default admin user created: {adminEmail} / Admin123!");
		}
		else
		{
			Console.WriteLine("Failed to create admin user:");
			foreach (var error in result.Errors)
			{
				Console.WriteLine($"- {error.Description}");
			}
		}
	}
	else
	{
		// Ensure existing admin user has the Admin role
		if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
		{
			await userManager.AddToRoleAsync(adminUser, "Admin");
			Console.WriteLine($"Admin role added to existing user: {adminEmail}");
		}
	}
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Identity UI endpoints
app.MapRazorPages();

app.Run();
