namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.models;

public class ExternalApiConfig
{
    public string BaseUrl { get; set; }
    public int RequestTimeout { get; set; } = 10;
    public bool ProxyEnable { get; set; }
    public string ProxyUrl { get; set; }
    public bool UseDefaultCredentials { get; set; }
    public string ProxyUser { get; set; }
    public string ProxyPass { get; set; }
}
