namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Services;

public interface IEncryptionString
{
    string HashWithSHA256(string key);
}
