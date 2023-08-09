using System.Text;

namespace SimpleBankAPI.Formatters.CsvFormatters;

public class Transfer : Interfaces.IFormatter
{
    public void Format(StringBuilder buffer, object item)
    {
        var transfer = (Models.Responses.Transfer)item;
        buffer.AppendLine($"\"{transfer.Sender?.Id}\",\"{transfer.Sender?.Name}\",{transfer.Sender?.Balance}");
        buffer.AppendLine($"\"{transfer.Recipient?.Id}\",\"{transfer.Recipient?.Name}\",{transfer.Recipient?.Balance}");
    }
}