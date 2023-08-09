namespace SimpleBankAPI.Exceptions;

public class NoAccountsException : Exception
{
    public NoAccountsException()
        : base("No accounts found in the system.") {}
}