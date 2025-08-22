
namespace Domain.Common
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SequenceAttribute : Attribute
    {
        public string SequenceName { get; }

        public SequenceAttribute(string sequenceName)
        {
            SequenceName = sequenceName;
        }
    }
}
