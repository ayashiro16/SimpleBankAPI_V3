namespace SimpleBankAPI.Models.Responses.DTOs;

public record AccountDto(Guid Id, string Name, decimal Balance);