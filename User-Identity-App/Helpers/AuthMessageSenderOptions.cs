namespace User_Identity_App.Helpers
{
    public class AuthMessageSenderOptions
    {
        public string? ApiKey { get; set; }
        public string? SendGridKey { get; internal set; }
    }
}
