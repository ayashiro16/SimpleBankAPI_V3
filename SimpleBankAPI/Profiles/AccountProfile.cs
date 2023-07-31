using AutoMapper;
using SimpleBankAPI.Models.Entities;
using SimpleBankAPI.Models.Responses.DTOs;

namespace SimpleBankAPI.Profiles;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<Account, AccountDto>().ReverseMap();
        CreateMap<Account, AccountBalanceDto>();
    }
}