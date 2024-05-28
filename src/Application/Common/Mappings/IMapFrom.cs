using AutoMapper;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Mappings;

public interface IMapFrom<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}
