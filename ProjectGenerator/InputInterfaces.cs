using ProjectGenerator.Annotations;

namespace ProjectGenerator.InputInterfaces;

public interface IHasName
{
    public string Name { get; }
}

public interface IHasId
{
    [PrimaryKey]
    public int Id { get; }
}

//[DbEntity]
//public interface IUserSettingDefault : IEntity, IHasName
//{
//    [Key]
//    public string Name { get; }    //primary key, unique   //TODO decide if you want primary key string, or primary key ID, and Name will just be a unique key constraint.
//    public string Value { get; }
//}

[DbEntity, Model]
public interface IObjectType : IHasId, IHasName
{
    public string AuditCorrelationId { get; }
    [OnlyInDb]
    public ICollection<IAttributeDefinitionObjectType> AttributeDefinitionObjectTypes { get; set; }
    //[NotInDb]
    //public ICollection<IAttributeDefinition> AttributeDefinitions { get; set; }
    //[NotInDb]
    //public ICollection<int> AttributeDefinitionIds { get; set; }
}

[DbEntity]
public interface IAttributeDefinitionObjectType
{
    [PrimaryKey]
    public IAttributeDefinition AttributeDefinition { get; }
    [PrimaryKey]
    public IObjectType ObjectType { get; }
    public string AuditCorrelationId { get; set; }
}

[DbEntity, Model]
public interface IAttributeDefinition : IHasId, IHasName
{
    [OnlyCreate]
    public string Type { get; set; }
}
