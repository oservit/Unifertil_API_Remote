namespace Domain.Common
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IgnoreOnUpdateAttribute : Attribute
    {
    }
}
