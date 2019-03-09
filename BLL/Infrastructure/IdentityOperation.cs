namespace BLL.Infrastructure
{
    /// <summary>
    /// Information about operation success.
    /// </summary>
    public class IdentityOperation
    {
        public bool Succedeed { get; private set; }
        public string Message { get; private set; }
        public string Property { get; private set; }

        public IdentityOperation(bool succedeed, string message, string prop)
        {
            Succedeed = succedeed;
            Message = message;
            Property = prop;
        }
    }
}
