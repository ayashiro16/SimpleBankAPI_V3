using System.Collections.Immutable;
using IFormatter = SimpleBankAPI.Interfaces.IFormatter;

namespace SimpleBankAPI.Factories;

public class FormatterFactory : Interfaces.IFactory<IFormatter>
{
    private readonly IReadOnlyDictionary<string, IFormatter> _formatters;
    public IFormatter this[string key] => _formatters!.GetValueOrDefault(key);

    public FormatterFactory()
    {
        var formatterType = typeof(IFormatter);
        _formatters = formatterType.Assembly.ExportedTypes
            .Where(x => formatterType.IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false })
            .Select(x => Activator.CreateInstance(x))
            .Cast<IFormatter>()
            .ToImmutableDictionary(x => x.GetType().Name, x => x);
        Console.WriteLine("The number of formatters is: " + _formatters.Count);
    }

    public bool ContainsKey(string key) => _formatters.ContainsKey(key);
}