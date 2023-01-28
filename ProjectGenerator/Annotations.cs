using System.ComponentModel.DataAnnotations;

namespace ProjectGenerator.Annotations;

//[AttributeUsage(AttributeTargets.Parameter)]
public class NotInDbAttribute : System.Attribute
{
    public NotInDbAttribute()
    {
    }
}