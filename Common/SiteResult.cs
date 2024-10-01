namespace Common;

public class SiteResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }

    public SiteResult(bool isSuccess, string message = "")
    {
        IsSuccess = isSuccess;
        Message = message;
    }
}

public class SiteResult<TData> : SiteResult
    where TData : class
{
    public TData Data { get; set; }


    public SiteResult(bool isSuccess, TData data, string message = "")
        : base(isSuccess, message)
    {
        Data = data;
    }
}
