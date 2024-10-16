using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Wallet.Infrastructure.Repositories.Models;
using Microsoft.Extensions.Configuration;

namespace Wallet.Infrastructure.Persistence;

public class WalletContext(DbContextOptions<WalletContext> options) : DbContext(options)
{
    public DbSet<WalletModel> Wallets { get; set; }
    public DbSet<TransactionModel> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WalletModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Balance)
                .HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<TransactionModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18,2)");
            entity.HasOne(t => t.Wallet)
                .WithMany(w => w.Transactions)
                .HasForeignKey(t => t.WalletId);
        });
    }
}

public class WalletContextContextFactory : IDesignTimeDbContextFactory<WalletContext>
{
    public WalletContext CreateDbContext(string[] args = null)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WalletContext>();
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) 
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration["DEFAULT_CONNECTION_STRING"];

        optionsBuilder.UseSqlServer(connectionString);

        return new WalletContext(optionsBuilder.Options);
    }
}