using System.Text;

namespace SimpleBankAPI.Formatters.CsvFormatters;

public class ConvertCurrency : Interfaces.IFormatter
{
    public void Format(StringBuilder buffer, object item)
    {
        var currency = (Models.Responses.ConvertCurrency)item;
        buffer.AppendLine($"\"{currency.CurrencyCode}\",{currency.ConvertedBalance}");
    }
}