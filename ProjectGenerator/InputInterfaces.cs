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

//[DbEntity]
//public interface IUserSettingDefault : IEntity, IHasName
//{
//    [Key]
//    public string Name { get; }    //primary key, unique   //TODO decide if you want primary key string, or primary key ID, and Name will just be a unique key constraint.
//    public string Value { get; }
//}

[DbEntity]
public interface IObjectType : IHasId, IHasName
{
    public string AuditCorrelationId { get; }
    public ICollection<IAttributeDefinitionObjectType> AttributeDefinitionObjectTypes { get; set; }
    [NotInDb]
    public ICollection<IAttributeDefinition> AttributeDefinitions { get; set; }
}

[DbEntity]
public interface IAttributeDefinitionObjectType
{
    [Key]
    public IAttributeDefinition AttributeDefinition { get; }
    [Key]
    public IObjectType ObjectType { get; }
    public string AuditCorrelationId { get; set; }
}

[DbEntity]
public interface IAttributeDefinition : IHasId, IHasName
{
    public string Type { get; set; }
}
