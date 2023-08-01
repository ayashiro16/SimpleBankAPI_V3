using SimpleBankAPI.Exceptions;
using SimpleBankAPI.Models;
using SimpleBankAPI.Models.Requests;
using AccountContext = SimpleBankAPI.Data.AccountContext;
using IAccountsRepository = SimpleBankAPI.Interfaces.IAccountsRepository;
using Account = SimpleBankAPI.Models.Entities.Account;

namespace SimpleBankAPI.Repositories;

public class AccountsRepository : IAccountsRepository
{
    private readonly AccountContext _context;

    public AccountsRepository(AccountContext context)
    {
        _context = context;
    }
    
    public ValueTask<Account?> Get(Guid id)
    {
        return _context.FindAsync(id);
    }

    public (List<Account>, PaginationMetadata) GetAll(GetAccountsQuery query)
    {
        var collection = _context.GetAll();
        if (!collection.Any())
        {
            throw new NoAccountsException();
        }
        if (!string.IsNullOrEmpty(query.FilterTerm))
        {
            collection = collection.Where(account => account.Name.ToUpper().Contains(query.FilterTerm.Trim().ToUpper()));
        }
        if (!string.IsNullOrEmpty(query.SearchTerm))
        {
            collection = collection.Where(account => string.Equals(account.Name, query.SearchTerm.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }
        collection = query.SortBy?.ToUpper().Trim() switch
        {
            ("BALANCE") => collection.OrderBy(account => account.Balance),
            ("NAME") => collection.OrderBy(account => account.Name),
            _ => collection.OrderBy(account => account.Id)
        };
        if (query.SortOrder == "DESC")
        {
            collection = collection.Reverse();
        }
        var paginationMetadata = new PaginationMetadata(collection.Count(), query.PageSize, query.CurrentPage);
        var result = collection.Skip(query.PageSize * (query.CurrentPage - 1)).Take(query.PageSize).ToList();

        return (result, paginationMetadata);
    }

    public void Add(Account account)
    {
        _context.Add(account);
        _context.SaveChangesAsync();
    }

    public void Update(Account account, decimal amount)
    {
        account.Balance += amount;
        _context.SaveChangesAsync();
    }
}