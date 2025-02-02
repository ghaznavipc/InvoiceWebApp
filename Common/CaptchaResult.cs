namespace Common;

public class CaptchaResult
{
    public required string CaptchaCode { get; set; }
    public required byte[] CaptchaByteData { get; set; }
    public string CaptchBase64Data => Convert.ToBase64String(CaptchaByteData);
    public DateTime Timestamp { get; set; }
}
