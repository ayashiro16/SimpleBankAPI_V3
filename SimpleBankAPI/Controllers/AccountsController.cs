using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SimpleBankAPI.Interfaces;
using SimpleBankAPI.Models.Requests;
using SimpleBankAPI.Models.Responses;
using SimpleBankAPI.Exceptions;
using SimpleBankAPI.Models.Responses.DTOs;

namespace SimpleBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;
        private readonly IMapper _mapper;
        
        public AccountsController(IAccountsService accountsService, IMapper mapper)
        {
            _accountsService = accountsService;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Retrieves account from database
        /// </summary>
        /// <param name="id">The account ID</param>
        /// <returns>The account associated with the provided ID</returns>
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<AccountDto>> GetAccount(Guid id)
        {
            try
            {
                var account = await _accountsService.FindAccount(id);
                return _mapper.Map<AccountDto>(account);
            }
            catch (Exception e)
            {
                return e switch
                {
                    AccountNotFoundException => NotFound(e.Message),
                    _ => throw e
                };
            }
        }
        
        /// <summary>
        /// Retrieves the converted account balances for a given list of currencies
        /// </summary>
        /// <param name="id">The account ID</param>
        /// <param name="currencyCodes">List of currency codes; empty request returns all currencies</param>
        /// <returns>List of converted user balances</returns>
        [HttpGet("{id:Guid}/converts")]
        [HttpGet("{id:Guid}/converts/{currencyCodes?}")]
        public async Task<ActionResult<IEnumerable<ConvertCurrency>>> GetConvertedCurrency(Guid id, string? currencyCodes = "")
        {
            try
            {
                var converted = await _accountsService.GetConvertedCurrency(id, currencyCodes);
                return new ActionResult<IEnumerable<ConvertCurrency>>(converted);
            }
            catch (Exception e)
            {
                return e switch
                {
                    ArgumentException => BadRequest(e.Message),
                    NullAccountException => Problem(e.Message),
                    HttpRequestException => Problem(e.Message),
                    _ => throw e
                };
            }
        }
        
        /// <summary>
        /// Create and store a new account with the provided user's name
        /// </summary>
        /// <param name="request">The name of the account holder</param>
        /// <returns>The account details of the newly created account</returns>
        [HttpPost]
        public async Task<ActionResult<AccountDto>> PostNewAccount([FromBody] CreateAccount request)
        {
            try
            {
                var account = await _accountsService.CreateAccount(request.Name);
                return _mapper.Map<AccountDto>(account);
            }
            catch (Exception e)
            {
                return e switch
                {
                    ArgumentException => BadRequest(e.Message),
                    _ => throw e
                };
            }
        }

        /// <summary>
        /// Creates deposit to add funds to an account
        /// </summary>
        /// <param name="id">The account ID</param>
        /// <param name="request">The amount to be deposited</param>
        /// <returns>The account details of the account following the deposit</returns>
        [HttpPost("{id:Guid}/deposits")]
        public async Task<ActionResult<AccountBalanceDto>> PostDepositFunds(Guid id, [FromBody] GetAmount request)
        {
            try
            {
                var account = await _accountsService.DepositFunds(id, request.Amount);
                return _mapper.Map<AccountBalanceDto>(account);
            }
            catch (Exception e)
            {
                return e switch
                {
                    AccountNotFoundException => NotFound(e.Message),
                    ArgumentException => BadRequest(e.Message),
                    _ => throw e
                };
            }
        }

        /// <summary>
        /// Creates withdrawal to take from an account
        /// </summary>
        /// <param name="id">The account ID</param>
        /// <param name="request">The amount to be withdrawn</param>
        /// <returns>The account details of the account following the withdrawal</returns>
        [HttpPost("{id:Guid}/withdrawals")]
        public async Task<ActionResult<AccountBalanceDto>> PostWithdrawFunds(Guid id, [FromBody] GetAmount request)
        {
            try
            {
                var account = await _accountsService.WithdrawFunds(id, request.Amount);
                return _mapper.Map<AccountBalanceDto>(account);
            }
            catch (Exception e)
            {
                return e switch
                {
                    AccountNotFoundException => NotFound(e.Message),
                    ArgumentOutOfRangeException => BadRequest(e.Message),
                    InvalidOperationException => BadRequest(e.Message),
                    _ => throw e
                };
            }
        }

        /// <summary>
        /// Creates a transfer that takes funds from sender account and deposits to receiver account
        /// </summary>
        /// <param name="request">Sender's account ID, Recipient's account ID, and amount to be transferred</param>
        /// <returns>The account details of both the sender and the recipient following the transfer</returns>
        [HttpPost("transfers")]
        public async Task<ActionResult<Transfer>> PostTransferFunds([FromBody] TransferFunds request)
        {
            try
            {
                var accounts = await _accountsService.TransferFunds(request.SenderId, request.RecipientId, request.Amount);
                return accounts;
            }
            catch (Exception e)
            {
                return e switch
                {
                    AccountNotFoundException => NotFound(e.Message),
                    ArgumentOutOfRangeException => BadRequest(e.Message),
                    InvalidOperationException => BadRequest(e.Message),
                    _ => throw e
                };
            }
        }
    }
}