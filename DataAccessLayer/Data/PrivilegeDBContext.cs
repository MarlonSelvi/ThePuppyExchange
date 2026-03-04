using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

public class PrivilegeDBContext : DbContext
{
    public PrivilegeDBContext(DbContextOptions<PrivilegeDBContext> options) : base(options) { }

    public DbSet<UserPrivilegeModel> AccountPrivileges { get; set; } = default!;
}