using Kavenegar;

namespace MyServices;

public class SMSSender : ISMSSender
{
	public async Task TrySendAsync(string receptor, string token, string token2 = "", string token3 = "", string template = "verify2", int iCustomerID = 1)
	{
		KavenegarApi api = InitialKavenegar(iCustomerID);

		try
		{
			await api.VerifyLookup(receptor, token, token2, token3, template);
		}
		catch
		{
		}
	}

	public static void Send(string receptor, string message, string sender = "1000035159", int iCustomerID = 1)
	{
		KavenegarApi api = InitialKavenegar(iCustomerID);
		api.Send(sender, receptor, message);
	}

	private static KavenegarApi InitialKavenegar(int iCustomerID)
	{
		SiteSettings siteSettings = new();

		return iCustomerID switch
		{
			1 => new(siteSettings.KavenegarAPIKeyNazhoFactor),
			2 => new(siteSettings.KavenegarAPIKeyDRGH),
			3 => new(siteSettings.KavenegarAPIKeyAbolfazl),
			_ => new(siteSettings.KavenegarAPIKeyDefault),
		};
	}
}
