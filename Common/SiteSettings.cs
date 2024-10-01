namespace Common;

public class SiteSettings
{
	public string? URL { get; set; }
	public string? ElmahPath { get; set; }
	public string? KavenegarAPIKeyDefault { get; set; }
	public string? KavenegarAPIKeyDRGH { get; set; }
	public string? KavenegarAPIKeyAbolfazl { get; set; }
	public string? KavenegarAPIKeyNazhoFactor { get; set; }
	public JwtSettings? JwtSettings { get; set; }
	public IdentitySettings? IdentitySettings { get; set; }
	public EmailConfiguration? EmailConfiguration { get; set; }
}

public class IdentitySettings
{
	public bool PasswordRequireDigit { get; set; }
	public int  PasswordRequiredLength { get; set; }
	public bool PasswordRequireNonAlphanumic { get; set; }
	public bool PasswordRequireUppercase { get; set; }
	public bool PasswordRequireLowercase { get; set; }
	public bool RequireUniqueEmail { get; set; }
}
public class JwtSettings
{
	public string? SecretKey { get; set; }
	public string? Encryptkey { get; set; }
	public string? Issuer { get; set; }
	public string? Audience { get; set; }
	public int NotBeforeMinutes { get; set; }
	public int ExpirationMinutes { get; set; }
}
public class EmailConfiguration
{
	public string? SmtpServer { get; set; }
	public int SmtpPort { get; set; }
	public string? SmtpUsername { get; set; }
	public string? SmtpPassword { get; set; }
	public string? FromName { get; set; }
	public string? FromAddress { get; set; }
}