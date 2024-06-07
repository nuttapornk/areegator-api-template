namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Exceptions;

public class InternalServerErrorException : Exception
{
    public string ErrorCode { get; }
    public string ErrorMessage { get; }
    public InternalServerErrorException() : base()
    {
    }

    public InternalServerErrorException(string message) : base(message)
    {
    }

    public InternalServerErrorException(string message,Exception innerException) : base(message,innerException)
    {
    }

    public InternalServerErrorException(string name, object key) 
        : base($"Entity \"{name}\" ({key}) was internal error.")
    {
    }

    public InternalServerErrorException(string message,string errorCode = "",string errorMessage="" ) : base(message)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public InternalServerErrorException(string message, Exception innerException, string errorCode = "", string errorMessage = "") : base(message, innerException)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public InternalServerErrorException(string name, object key, string errorCode = "", string errorMessage = "")
        : base($"Entity \"{name}\" ({key}) was internal error.")
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

}
