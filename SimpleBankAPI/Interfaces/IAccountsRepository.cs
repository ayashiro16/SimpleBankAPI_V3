using PaginationMetadata = SimpleBankAPI.Models.PaginationMetadata;
using GetAccountsQuery = SimpleBankAPI.Models.Requests.GetAccountsQuery;
using Account = SimpleBankAPI.Models.Entities.Account;

namespace SimpleBankAPI.Interfaces;

public interface IAccountsRepository
{
    ValueTask<Account?> Get(Guid id);
    (List<Account>, PaginationMetadata) GetAll(GetAccountsQuery query);
    void Add(Account account);
    void Update(Account account, decimal amount);
}