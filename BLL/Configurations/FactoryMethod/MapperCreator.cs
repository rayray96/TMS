using AutoMapper;

namespace BLL.Configurations.FactoryMethod
{
    public abstract class MapperCreator
    {
        IWrappedMapper _wrappedMapper;

        public abstract IWrappedMapper FactoryMethod();

        public IMapper GetMapper()
        {
            _wrappedMapper = FactoryMethod();

            return _wrappedMapper.CreateMapping();
        }
    }
}
