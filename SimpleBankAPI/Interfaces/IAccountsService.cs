using SimpleBankAPI.Models.Responses;
using Account = SimpleBankAPI.Models.Entities.Account;
using GetAccountsQuery = SimpleBankAPI.Models.Requests.GetAccountsQuery;
using PaginationMetadata = SimpleBankAPI.Models.PaginationMetadata;

namespace SimpleBankAPI.Interfaces;

public interface IAccountsService
{
    Task<Account> CreateAccount(string name);
    ValueTask<Account> FindAccount(Guid id);
    (IEnumerable<Account>, PaginationMetadata) GetAllAccounts(GetAccountsQuery query);
    Task<Account> DepositFunds(Guid id, decimal amount);
    Task<Account> WithdrawFunds(Guid id, decimal amount);
    Task<Transfer> TransferFunds(Guid senderId, Guid recipientId, decimal amount);
    Task<IEnumerable<ConvertCurrency>> GetConvertedCurrency(Guid id, string currencyCode);
}