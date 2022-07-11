using Microsoft.EntityFrameworkCore;
using ThingFinder.Data;
using ThingFinder.Models.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => {
	var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

	options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
	   .AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
	   .AddRoles<Role>()
	   .AddEntityFrameworkStores<ApplicationDbContext>();

var mvc = builder.Services.AddControllersWithViews();

if (builder.Environment.IsDevelopment())
{
	mvc.AddRazorRuntimeCompilation();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();