namespace BLL.Configurations.FactoryMethod
{
    public class IdentityCreator : MapperCreator
    {
        public override IWrappedMapper FactoryMethod()
        {
            return new IdentityMapper();
        }
    }
}
