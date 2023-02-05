using ProjectGenerator.Annotations;

namespace ProjectGenerator.Tests;

[DbEntity, Model]
public interface ISimpleThing
{
    [PrimaryKey]
    public int Id { get; }
    public string Value { get; }
}