namespace FreeMobile.FreeMobileSmsApi.Models
{
    public enum SendSmsResult
    {
        Sent,
        MissingParameter,
        Throttled,
        ServiceNotActivated,
        ServerError
    }
}