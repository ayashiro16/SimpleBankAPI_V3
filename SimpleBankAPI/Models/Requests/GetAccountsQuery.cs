using System.ComponentModel.DataAnnotations;

namespace SimpleBankAPI.Models.Requests;

public class GetAccountsQuery
{
    private int _pageSize = 10;
    
    /// <summary>
    /// filter for names that contain this string
    /// </summary>
    public string? FilterTerm { get; set; }
    
    /// <summary>
    /// search for names that match this string exactly
    /// </summary>
    public string? SearchTerm { get; set; }
    
    /// <summary>
    /// Allowed values:
    /// "Name"
    /// "Balance"
    /// </summary>
    public string? SortBy { get; init; }
    
    /// <summary>
    /// Allowed values:
    /// "ASC" - Ascending order
    /// "DESC" - Descending order
    /// </summary>
    public string? SortOrder { get; init; }

    public int CurrentPage { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (value is > 0 and <= 100)
            {
                _pageSize = value;
            }
        }
    }
}