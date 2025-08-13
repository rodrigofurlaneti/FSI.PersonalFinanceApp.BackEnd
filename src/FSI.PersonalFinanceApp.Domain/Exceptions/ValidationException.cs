﻿namespace FSI.PersonalFinanceApp.Domain.Exceptions
{
    public sealed class ValidationException : Exception
    {
        public IReadOnlyDictionary<string, string[]> Errors { get; }

        public ValidationException(string message)
            : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(string message, IDictionary<string, string[]> errors)
            : base(message)
        {
            Errors = new Dictionary<string, string[]>(errors);
        }
    }
}
