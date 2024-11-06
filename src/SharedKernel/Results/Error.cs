namespace SharedKernel.Results;

public sealed class Error : IEquatable<Error>
{
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static Error None => new(string.Empty, string.Empty);

    public string Code { get; }

    public string Message { get; }

    public static implicit operator string(Error error) => error?.Code ?? string.Empty;

    public static bool operator ==(Error a, Error b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Error a, Error b) => !(a == b);

    public bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Code == other.Code && Message == other.Message;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj is not Error error)
        {
            return false;
        }

        return Equals(error);
    }

    public override int GetHashCode() => HashCode.Combine(Code, Message);
}