using System.Globalization;

namespace Application.Exceptions;

public abstract class ApiException : Exception
{
    protected ApiException() : base () { }

    protected ApiException(string message) : base(message) { }

    protected ApiException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}