namespace BLL.Infrastructure
{
    /// <summary>
    /// Information about operation success.
    /// </summary>
    public class IdentityOperation
    {
        public bool Succeedeed { get; private set; }
        public string Message { get; private set; }
        public string Property { get; private set; }

        public IdentityOperation(bool succeedeed, string message, string prop)
        {
            Succeedeed = succeedeed;
            Message = message;
            Property = prop;
        }
    }
}
