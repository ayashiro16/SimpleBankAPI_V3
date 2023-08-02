using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using SimpleBankAPI.Models.Responses;
using SimpleBankAPI.Models.Responses.DTOs;

namespace SimpleBankAPI.Formatters;

public class CsvOutputFormatter : TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type)
        => typeof(AccountDto).IsAssignableFrom(type)
           || typeof(IEnumerable<AccountDto>).IsAssignableFrom(type)
           || typeof(AccountBalanceDto).IsAssignableFrom(type)
           || typeof(IEnumerable<ConvertCurrency>).IsAssignableFrom(type)
           || typeof(Transfer).IsAssignableFrom(type);

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();
        switch (context.Object)
        {
            case IEnumerable<AccountDto> accountDtos:
                foreach (var accountDto in accountDtos)
                {
                    FormatCsv(buffer, accountDto);
                }
                break;
            case AccountDto dto:
                FormatCsv(buffer, dto);
                break;
            case AccountBalanceDto balance:
                FormatCsvBalance(buffer, balance);
                break;
            case IEnumerable<ConvertCurrency> conversions:
                foreach (var conversion in conversions)
                {
                    FormatCsvCurrencyConversions(buffer, conversion);
                }
                break;
            case Transfer transfer:
                FormatCsvTransfer(buffer, transfer);
                break;
        }

        await response.WriteAsync(buffer.ToString(), selectedEncoding);
    }

    private static void FormatCsv(StringBuilder buffer, AccountDto accountDto)
    {
        buffer.AppendLine($"\"{accountDto.Id}\",\"{accountDto.Name}\",{accountDto.Balance}");
    }

    private void FormatCsvBalance(StringBuilder buffer, AccountBalanceDto balance)
    {
        buffer.Append($"{balance.Balance}");
    }
    
    private void FormatCsvCurrencyConversions(StringBuilder buffer, ConvertCurrency currency)
    {
        buffer.AppendLine($"\"{currency.CurrencyCode}\",{currency.ConvertedBalance}");
    }

    private void FormatCsvTransfer(StringBuilder buffer, Transfer transfer)
    {
        buffer.AppendLine($"\"{transfer.Sender.Id}\",\"{transfer.Sender.Name}\",{transfer.Sender.Balance}");
        buffer.AppendLine($"\"{transfer.Recipient.Id}\",\"{transfer.Recipient.Name}\",{transfer.Recipient.Balance}");
    }
}
