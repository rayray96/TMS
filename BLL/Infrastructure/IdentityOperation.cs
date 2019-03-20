namespace BLL.Infrastructure
{
    /// <summary>
    /// Information about operation success.
    /// </summary>
    public class IdentityOperation
    {
        public bool Succeeded { get; private set; }
        public string Message { get; private set; }
        public string Property { get; private set; }

        public IdentityOperation(bool succeeded, string message, string prop)
        {
            Succeeded = succeeded;
            Message = message;
            Property = prop;
        }
    }
}
