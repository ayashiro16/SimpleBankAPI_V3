using System.Text;

namespace SimpleBankAPI.Formatters.CsvFormatters;

public class AccountBalanceDto : Interfaces.IFormatter
{
    public void Format(StringBuilder buffer, object item) => buffer.Append($"{((Models.Responses.DTOs.AccountBalanceDto)item).Balance}");
}