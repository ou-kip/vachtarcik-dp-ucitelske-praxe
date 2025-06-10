namespace Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string typeName, string id) : base($"The {typeName} with Id: {id} is not available.")
        { 
        }
    }
}
