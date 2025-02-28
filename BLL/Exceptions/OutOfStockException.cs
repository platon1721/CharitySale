namespace BLL.Exceptions;


/// <summary>
/// Exception that is thrown when a product is out of stock.
/// </summary>
public class OutOfStockException: Exception
{
    
    /// <summary>
    /// Initializes a new instance of the exception class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public OutOfStockException(string message) : base(message)
    {
        
    }
}