namespace SimpleBankAPI.Exceptions;

public class NoSearchResultsException : Exception
{
    public NoSearchResultsException()
        : base("No accounts matched the provided search/filter criteria.")
    {
    }
}