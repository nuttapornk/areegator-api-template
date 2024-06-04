namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Services;

public interface ITraceId
{
    string Read();
    void Write(string traceId);
}
