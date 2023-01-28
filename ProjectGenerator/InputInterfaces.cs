using ProjectGenerator.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ProjectGenerator.InputInterfaces;

public interface IHasName
{
    public string Name { get; }
}

public interface IHasId
{
    [Key]
    public int Id { get; }
}

public interface IEntity : IHasId 
{ }

public interface IObjectType : IEntity, IHasId, IHasName
{
    public string AuditCorrelationId { get; }
    public ICollection<IAttributeDefinitionObjectType> AttributeDefinitionObjectTypes { get; set; }
    [NotInDb]
    public ICollection<IAttributeDefinition> AttributeDefinitions { get; set; }
}

public interface IAttributeDefinitionObjectType : IEntity
{
    [Key]
    public IAttributeDefinition AttributeDefinition { get;  }
    [Key]
    public IObjectType ObjectType { get; }
    public string AuditCorrelationId { get; set; }
}

public interface IAttributeDefinition : IEntity, IHasId, IHasName
{ 
    public string Type { get; set; }
}
