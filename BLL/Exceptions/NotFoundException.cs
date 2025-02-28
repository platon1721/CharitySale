namespace BLL.Exceptions;


/// <summary>
/// Exception that is thrown when a requested resource is not found.
/// </summary>
public class NotFoundException : Exception
{
    
    /// <summary>
    /// Initializes a new instance of the NotFoundException class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NotFoundException(string message) : base(message)
    {
        
    }
}