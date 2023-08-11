using System.Text;

namespace SimpleBankAPI.Formatters.CsvFormatters;

public class AccountDto : Interfaces.IFormatter
{
    public void Format(StringBuilder buffer, object item)
    {
        var account = (Models.Responses.DTOs.AccountDto)item;
        buffer.AppendLine($"\"{account.Id}\",\"{account.Name}\",{account.Balance}");
    }
}