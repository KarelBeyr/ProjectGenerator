using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGenerator.Tests
{
    public static class Resources
    {
        public static string SimpleThingControllerCreate = @"    /// <summary>
    /// Creates new SimpleThing
    /// </summary>
    /// <param name=""request"">Model describing the new SimpleThing</param>
    /// <returns>New Id assigned to new SimpleThing</returns>
    /// <response code=""201"">New Id assigned to new SimpleThing</response>
    /// <response code=""400"">If request is wrong</response>
    [HttpPut]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> Create([FromBody] CreateSimpleThingModel request)
    {
        var id = await _simpleThingService.Create(new CreateSimpleThingCommand
        {
            Value = request.Value,
        });
        return CreatedAtAction(nameof(Get), new { id = id }, new { id = id });
    }";

        public static string SimpleThingControllerGet = @"    /// <summary>
    /// Gets SimpleThing
    /// </summary>
    /// <param name=""id"">Id of SimpleThing to get details</param>
    /// <returns>Detail of given SimpleThing</returns>
    /// <response code=""200"">Detail of given SimpleThing</response>
    /// <response code=""404"">If invalid <paramref name=""Id""/> was passed.</response>
    [HttpGet(""{Id}"")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SimpleThingModel>> Get([FromRoute] int id)
    {
        var res = await _simpleThingService.Get(id);
        return Ok(res);
    }";

        public static string ConfigurationControllerCreate = @"    /// <summary>
    /// Creates new Configuration
    /// </summary>
    /// <param name=""request"">Model describing the new Configuration</param>
    /// <returns>New Key assigned to new Configuration</returns>
    /// <response code=""201"">New Key assigned to new Configuration</response>
    /// <response code=""400"">If request is wrong</response>
    [HttpPut]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Create([FromBody] CreateConfigurationModel request)
    {
        await _configurationService.Create(new CreateConfigurationCommand
        {
            Key = request.Key,
            ServiceName = request.ServiceName,
            Value = request.Value,
            Encrypted = request.Encrypted,
            AuditCorrelationId = Request.Headers[""AuditCorrelationId""],
        });
        return CreatedAtAction(nameof(Get), new { key = request.Key, serviceName = request.ServiceName }, new { key = request.Key, serviceName = request.ServiceName });
    }";

        public static string ConfigurationControllerGet = @"    /// <summary>
    /// Gets Configuration
    /// </summary>
    /// <param name=""key"">Key of Configuration to get details</param>
    /// <param name=""serviceName"">ServiceName of Configuration to get details</param>
    /// <returns>Detail of given Configuration</returns>
    /// <response code=""200"">Detail of given Configuration</response>
    /// <response code=""404"">If invalid <paramref name=""Key""/> was passed.</response>
    [HttpGet(""{Key}"")]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConfigurationModel>> Get([FromRoute] string key, [FromQuery] string serviceName)
    {
        var res = await _configurationService.Get(key, serviceName);
        return Ok(res);
    }";

        public static string ConfigurationControllerUpdate = @"    /// <summary>
    /// Updates Configuration
    /// </summary>
    /// <param name=""key"">Key of Configuration to be updated</param>
    /// <param name=""serviceName"">ServiceName of Configuration to be updated</param>
    /// <param name=""request"">Complete new state of the Configuration</param>
    /// <response code=""204"">Update was successful.</response>
    /// <response code=""404"">If invalid <paramref name=""key""/> was passed.</response>
    /// <response code=""400"">If update cannot be performed, contains reason why.</response>
    [HttpPatch(""{key}"")]
    public async Task<ActionResult> Update([FromRoute] string key, [FromQuery] string serviceName, [FromBody] UpdateConfigurationModel request)
    {
        await _configurationService.Update(new UpdateConfigurationCommand
        {
            Key = key,
            ServiceName = serviceName,
            Value = request.Value,
            Encrypted = request.Encrypted,
        });
        return NoContent();
    }";

        public static string ConfigurationControllerDelete = @"    /// <summary>
    /// Deletes Configuration
    /// </summary>
    /// <param name=""key"">Key of Configuration to be deleted</param>
    /// <param name=""serviceName"">ServiceName of Configuration to be deleted</param>
    /// <response code=""204"">Delete was successful.</response>
    /// <response code=""404"">If invalid <paramref name=""key""/> was passed.</response>
    [HttpDelete(""{key}"")]
    public async Task<ActionResult> Delete([FromRoute] string key, [FromQuery] string serviceName)
    {
        await _configurationService.Delete(new DeleteConfigurationCommand
        {
            Key = key,
            ServiceName = serviceName,
        });
        return NoContent();
    }";

        public static string UserSettingControllerCreate = @"    /// <summary>
    /// Creates new UserSetting
    /// </summary>
    /// <param name=""request"">Model describing the new UserSetting</param>
    /// <returns>New Name assigned to new UserSetting</returns>
    /// <response code=""201"">New Name assigned to new UserSetting</response>
    /// <response code=""400"">If request is wrong</response>
    [HttpPut]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Create([FromBody] CreateUserSettingModel request)
    {
        await _userSettingService.Create(new CreateUserSettingCommand
        {
            Name = request.Name,
            Value = request.Value,
            Authorization = Request.Headers[""Authorization""],
        });
        return CreatedAtAction(nameof(Get), new { name = request.Name }, new { name = request.Name });
    }";

        public static string UserSettingControllerGet = @"    /// <summary>
    /// Gets UserSetting
    /// </summary>
    /// <param name=""name"">Name of UserSetting to get details</param>
    /// <returns>Detail of given UserSetting</returns>
    /// <response code=""200"">Detail of given UserSetting</response>
    /// <response code=""404"">If invalid <paramref name=""Name""/> was passed.</response>
    [HttpGet(""{Name}"")]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserSettingModel>> Get([FromRoute] string name)
    {
        var res = await _userSettingService.Get(name, Request.Headers[""Authorization""]);
        return Ok(res);
    }";

        public static string ConfigurationServiceGet = @"    async Task<ConfigurationModel> IConfigurationService.Get(string key)
    {
        var entity = await _configurationRepository.Get(key);
        if (entity == null) throw new NotFoundException(""Configuration with given name not found"");
        var model = new ConfigurationModel
        {
            Key = entity.Key,
            ServiceName = entity.ServiceName,
            Value = entity.Value,
            Encrypted = entity.Encrypted,
        };
        return model;
    }";

        public static string ConfigurationServiceCreate = @"    async Task IConfigurationService.Create(CreateConfigurationCommand command)
    {
        //TODO EnsurePermissionAndAccess(...) 
        var found = await _configurationRepository.Get(command.ServiceName, command.Key);
        if (found != null) throw new InvalidStateException(""Configuration with given primaryKeys already exists"");
        var entity = new Configuration
        {
            ServiceName = command.ServiceName,
            Key = command.Key,
            Value = command.Value,
            Encrypted = command.Encrypted,
        };
        _configurationRepository.Save(entity);
        await _unitOfWork.SaveChanges();
    }";

        public static string Models_UserSettingModel = @"public partial class UserSettingModel
{
    public string Name { get; set; }
    public string UserId { get; set; }
    public string Value { get; set; }
}";

        public static string Models_ConfigurationModel = @"public partial class ConfigurationModel
{
    public string ServiceName { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public bool Encrypted { get; set; }
}";

    }
}
