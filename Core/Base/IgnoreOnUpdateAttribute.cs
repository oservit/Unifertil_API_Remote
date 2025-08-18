namespace Domain.Base
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IgnoreOnUpdateAttribute : Attribute
    {
    }
}
