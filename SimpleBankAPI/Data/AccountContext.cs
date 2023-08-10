using Microsoft.EntityFrameworkCore;
using Account = SimpleBankAPI.Models.Entities.Account;

namespace SimpleBankAPI.Data;

public class AccountContext : DbContext
{
    public DbSet<Account> Accounts { private get; init; }
    
    public AccountContext(DbContextOptions<AccountContext> options) : base(options) {}

    public void Add(Account account) => Accounts.Add(account);

    public ValueTask<Account?> FindAsync(Guid id) => Accounts.FindAsync(id);

    public IQueryable<Account> GetAll() => Accounts.AsQueryable();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>().HasData(
            new Account()
            {
                Name = "Lazy Susan",
                Balance = 100,
                Id = Guid.NewGuid()
            },
            new Account()
            {
                Name = "John Deere",
                Balance = 5000,
                Id = Guid.NewGuid()
            },
            new Account()
            {
                Name = "Mary Poppins",
                Balance = 25000,
                Id = Guid.NewGuid()
            });
        modelBuilder.Entity<Account>()
            .Property(e => e.Balance)
            .HasConversion<double>();
        base.OnModelCreating(modelBuilder);
    }
}