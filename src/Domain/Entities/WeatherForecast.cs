namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;

public partial class WeatherForecast
{
    public Guid CountryId { get; set; }

    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summaries { get; set; }

    public virtual Country Country { get; set; } = null!;
}
