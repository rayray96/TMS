namespace BLL.Configurations.FactoryMethod
{
    public class CommonCreator : MapperCreator
    {
        public override IWrappedMapper FactoryMethod()
        {
            return new CommonMapper();
        }
    }
}
