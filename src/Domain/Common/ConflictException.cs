namespace Domain.Common;

/// <summary>
/// Raised when a mutation violates a relational constraint (foreign key or unique key); surfaced as HTTP 409.
/// </summary>
public sealed class ConflictException : Exception
{
    public ConflictException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
