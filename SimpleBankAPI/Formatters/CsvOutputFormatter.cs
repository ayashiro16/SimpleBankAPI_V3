using System.Collections;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using SimpleBankAPI.Factories;
using SimpleBankAPI.Interfaces;

namespace SimpleBankAPI.Formatters;

public class CsvOutputFormatter : TextOutputFormatter
{
    private readonly IFactory<IFormatter?> _formatters;

    public CsvOutputFormatter()
    {
        _formatters = new FormatterFactory();
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();
        if (response.StatusCode != 200)
        {
            buffer.Append(context.Object);
            await response.WriteAsync(buffer.ToString(), selectedEncoding);
            return;
        }
        var type = context.Object.GetType();
        if (typeof(IEnumerable).IsAssignableFrom(type))
        {
            var collection = (IEnumerable<object>)context.Object;
            type = collection.GetType()
                .GetGenericArguments()
                .FirstOrDefault(x => _formatters.ContainsKey(x.Name));
            var formatter = _formatters[type.Name];
            foreach (var item in collection)
            {
                formatter.Format(buffer, item);
            }
        }
        else
        {
            var formatter = _formatters[context.Object.GetType().Name];
            formatter.Format(buffer, context.Object!);
        }
        
        await response.WriteAsync(buffer.ToString(), selectedEncoding);
    }
}
