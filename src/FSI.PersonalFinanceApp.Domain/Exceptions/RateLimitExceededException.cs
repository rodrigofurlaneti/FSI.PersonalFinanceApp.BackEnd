namespace FSI.PersonalFinanceApp.Domain.Exceptions
{
    public class RateLimitExceededException : Exception { public RateLimitExceededException(string m) : base(m) { } }
}
