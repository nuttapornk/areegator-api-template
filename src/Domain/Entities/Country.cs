namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;

public partial class Country
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<WeatherForecast> WeatherForecasts { get; set; } = new List<WeatherForecast>();
}
