using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThingFinder.Models.Identity;

namespace ThingFinder.Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, string>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options) { }
}