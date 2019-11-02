using AutoMapper;

namespace BLL.Configurations.FactoryMethod
{
    public interface IWrappedMapper
    {
        IMapper CreateMapping();
    }
}
