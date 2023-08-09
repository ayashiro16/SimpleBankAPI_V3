using System.Text;

namespace SimpleBankAPI.Interfaces;

public interface IFormatter
{
    void Format(StringBuilder buffer, object item);
}