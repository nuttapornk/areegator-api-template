namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.ReqRes;

public record GetWeatherForecastRequest
{
    public Guid CountryId { get; set; }
    public DateOnly DateBegin { get; set; }
    public DateOnly DateEnd { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int? TemperatureC { get; set; }
    public string? Summaries { get; set; }
}
