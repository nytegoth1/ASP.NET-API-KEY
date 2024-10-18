using Microsoft.EntityFrameworkCore;

public class ApiKeyContext : DbContext
{
    public DbSet<ApiKey> ApiKeys { get; set; }

    public ApiKeyContext(DbContextOptions<ApiKeyContext> options) : base(options) { }
}
