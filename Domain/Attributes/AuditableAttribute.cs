namespace Domain.Attributes
{
    // This attribute is used to indicate that a class is auditable.
    // It can be applied to classes to mark them for auditing purposes.
    [AttributeUsage(AttributeTargets.Class)]
    public class AuditableAttribute : Attribute
    {
        // The class does not contain any members or methods,
        // as it serves solely as a marker attribute.
    }
}
