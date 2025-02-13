using Microsoft.EntityFrameworkCore;
using SoftPlcPortal.Infrastructure.Tables;

namespace SoftPlcPortal.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<PlcConfig> PlcConfigs { get; set; }
    public DbSet<DataBlock> DataBlocks { get; set; }
    public DbSet<DbField> DbFields { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlcConfig>(e =>
        {
            e.HasIndex(e => new { e.Name }).IsUnique();
            e.HasIndex(e => new { e.Address, e.PlcPort }).IsUnique();
        });

        modelBuilder.Entity<DataBlock>(e =>
        {
            e.HasIndex(e => new { e.Name }).IsUnique();
            e.HasIndex(e => new { e.PlcConfigKey, e.Number }).IsUnique();
        });

        modelBuilder.Entity<DbField>(e =>
        {
            //e.HasIndex(e => new { e.DataBlockKey, e.Name }).IsUnique();
            //e.HasIndex(e => new { e.DataBlockKey, e.ByteOffset, e.BitOffset }).IsUnique();
        });
    }
}
