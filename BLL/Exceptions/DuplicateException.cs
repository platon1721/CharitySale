namespace BLL.Exceptions;


/// <summary>
/// Exception that is thrown when a duplicate entry is encountered.
/// </summary>
public class DuplicateException: Exception
{
    /// <summary>
    /// Initializes a new instance of the Duplicationerror class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DuplicateException(string message) : base(message)
    {
        
    }
}