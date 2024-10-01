
namespace MyServices;

public interface ISMSSender
{
    Task TrySendAsync(string receptor, string token, string token2 = "", string token3 = "", string template = "verify2", int iCustomerID = 1);
}