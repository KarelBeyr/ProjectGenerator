using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGenerator.Tests
{
    public static class Resources
    {
        public static string ConfigurationControllerCreate = @"    /// <summary>
    /// Creates new Configuration
    /// </summary>
    /// <param name=""request"">Model describing the new Configuration</param>
    /// <returns>New Key assigned to new Configuration</returns>
    /// <response code=""201"">New Key assigned to new Configuration</response>
    /// <response code=""400"">If request is wrong</response>
    [HttpPut]
    [ProducesResponseType(typeof(Configuration), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Create([FromBody] CreateConfigurationModel request)
    {
        var id = await _configurationService.Create(new CreateConfigurationCommand
        {
            Key = request.Key,
            ServiceName = request.ServiceName,
            Value = request.Value,
            Encrypted = request.Encrypted,
            AuditCorrelationId = Request.Headers[""AuditCorrelationId""],
        });
        return CreatedAtAction(nameof(Get), new { Key = id }, id);
    }
";

        public static string UserSettingControllerCreate = @"    /// <summary>
    /// Creates new UserSetting
    /// </summary>
    /// <param name=""request"">Model describing the new UserSetting</param>
    /// <returns>New Name assigned to new UserSetting</returns>
    /// <response code=""201"">New Name assigned to new UserSetting</response>
    /// <response code=""400"">If request is wrong</response>
    [HttpPut]
    [ProducesResponseType(typeof(UserSetting), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Create([FromBody] CreateUserSettingModel request)
    {
        var id = await _userSettingService.Create(new CreateUserSettingCommand
        {
            Name = request.Name,
            Value = request.Value,
            Authorization = Request.Headers[""Authorization""],
        });
        return CreatedAtAction(nameof(Get), new { Name = id }, id);
    }
";
    }
}
