namespace SimpleBankAPI.Models.Responses.DTOs;

public record AccountDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Balance { get; init; }
}