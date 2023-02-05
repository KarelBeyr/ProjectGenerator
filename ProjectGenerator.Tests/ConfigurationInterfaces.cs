using ProjectGenerator.Annotations;

namespace ProjectGenerator.Tests;
[DbEntity, Model]
public interface IConfiguration
{
    [PrimaryKey(IsAutogonerated = false)]
    //[CommentSummary("Configuration key")]
    public string Key { get; }
    [PrimaryKey(IsAutogonerated = false, IsOptional = true)]
    //[CommentSummary("Name of service. Optional. Null means shared configuration for all services.")]
    public string ServiceName { get; }
    [CommentSummary("Configuration value")]
    public string Value { get; }

    //[CommentSummary("Flag, if given configuration value should be encrypted.")]
    public bool Encrypted { get; }  //questionable

    [ControllerFromHeader("AuditCorrelationId")]
    [OnlyInDb]
    public string AuditCorrelationId { get; }
}