using Newtonsoft.Json;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Models;

public class BaseResponse
{
    [JsonProperty("responseCode")]
    public string? Code { get; set; }

    [JsonProperty("responseMessage")]
    public string? Message { get; set; }

    [JsonProperty("responseDataSource")]
    public string? DataSource { get; set; }

    [JsonProperty("responseError")]
    public Error? Error { get; set; }

    public BaseResponse()
    {

    }

    protected void SetOk(string dataSource = "N/A")
    {
        Code = "200";
        Message = "90001";
        DataSource = dataSource;
        Error = null;

    }

    protected void SetError(string responseCode = "500",
    string errorCode = "",
    string devErrorMessage = "",
    string endUserErrorHeader = "",
    string endUserErrorMessage = "",
    string dataSource = "N/A")
    {
        Code = responseCode;
        Message = devErrorMessage;
        DataSource = dataSource;
        Error = new Error
        {
            Code = errorCode,
            Header = endUserErrorHeader,
            Message = endUserErrorMessage,
        };
    }

    public static BaseResponse Ok() => new()
    {
        Code = "200",
        Message = "90001",
        DataSource = "SBP",
        Error = null
    };

    public static BaseResponse Error500(string errorCode = "N/A", string dataSource = "N/A",
        string devErrorMessage = "", string endUserHeader = "พบข้อผิดพลาด",
        string endUserMessage = "พบข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ") => new()
        {
            Code = "500",
            Message = devErrorMessage,
            DataSource = dataSource,
            Error = new Error
            {
                Code = errorCode,
                Message = endUserMessage,
                Header = endUserHeader
            }
        };

    public static BaseResponse ErrorXXX(string responseCode = "500", string errorCode = "",
        string dataSource = "", string devErrorMessage = "", string endUserHeader = "",
        string endUserMessage = "") => new()
        {
            Code = responseCode,
            Message = devErrorMessage,
            DataSource = dataSource,
            Error = new Error
            {
                Code = errorCode,
                Header = endUserHeader,
                Message = endUserMessage
            }
        };

    public static BaseResponse Ok<T>(T data) => new BaseResponse<T>(data);
}

public class BaseResponse<T> : BaseResponse
{
    [JsonProperty("data")]
    public T Data { get; }

    public BaseResponse(T data) : base()
    {
        SetOk();
        this.Data = data;
    }

}