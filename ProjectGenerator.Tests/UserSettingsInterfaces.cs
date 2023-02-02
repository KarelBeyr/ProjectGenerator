using ProjectGenerator.Annotations;

namespace ProjectGenerator.Tests;

[DbEntity, Model]
public interface IUserSettingDefault
{
    [PrimaryKey]
    public string Name { get; }
    public string Value { get; }
}

[DbEntity, Model]
public interface IUserSetting
{
    [PrimaryKey]
    public string Name { get; }

    [PrimaryKey]
    [ControllerFromHeader("Authorization")]
    public string UserId { get; }
    public string Value { get; }
}