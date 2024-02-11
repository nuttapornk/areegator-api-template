namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Common;

public class RedisConfig
{
    public string EndPoint { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ChannelPrefix { get; set; }
    public int DefautlDatabase { get; set; }
    public bool Ssl { get; set; }
    public int ConnectRetry { get; set; }
}
