﻿using System.Text;
using System.Xml.Linq;

namespace ProjectGenerator;

public class ControllersGenerator : GeneratorBase
{
    //generates controllers
    public void Generate(DataModel dataModel)
    {
        foreach (var cls in dataModel.Classes.Values.Where(e => e.IsModel))
        {
            var serviceName = $"_{Utils.LowerCaseFirst(cls.Name)}Service";
            var pkField = cls.PrimaryKeyField();
            var pkFieldVarName = Utils.LowerCaseFirst(pkField.Name);
            var sb = new IndentingStringBuilder();

            sb.AppendLine("using Microsoft.AspNetCore.Http;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine($"using {GeneratedProjectNamespace}.Entities;");
            sb.AppendLine($"using {GeneratedProjectNamespace}.Models;");
            sb.AppendLine($"using {GeneratedProjectNamespace}.Commands;");
            sb.AppendLine($"using {GeneratedProjectNamespace}.Interfaces;");

            sb.AppendLine($"namespace {GeneratedProjectNamespace}.Controllers;");
            sb.AppendLine();

            GenerateXmlComment("summary", $"{cls.Name} controller", sb);
            sb.AppendLine($"[Route(\"api/[controller]\")]");
            sb.AppendLine($"public class {cls.Name}Controller : Controller");
            sb.IncreaseIndent();
            sb.AppendLine($"private readonly I{cls.Name}Service {serviceName};");
            sb.AppendLine($"");
            GenerateXmlComment("summary", "Creates new instance.", sb);
            sb.AppendLine($"public {cls.Name}Controller(I{cls.Name}Service {serviceName.Substring(1)})");
            sb.IncreaseIndent();
            sb.AppendLine($"{serviceName} = {serviceName.Substring(1)};");
            sb.DecreaseIndent();
            sb.AppendLine($"");

            ///////////////CREATE
            GenerateXmlComment("summary", $"Creates new {cls.Name}", sb);
            GenerateXmlComment("param name=\"request\"", $"Model describing the new {cls.Name}", sb, true);
            GenerateXmlComment("returns", $"New {pkField.Name} assigned to new {cls.Name}", sb, true);
            GenerateXmlComment("response code=\"201\"", $"New {pkField.Name} assigned to new {cls.Name}",sb, true);
            GenerateXmlComment("response code=\"400\"", $"If request is wrong", sb, true);
            sb.AppendLine($"[HttpPut]");
            sb.AppendLine($"[ProducesResponseType(typeof({cls.Name}), StatusCodes.Status200OK)]");
            sb.AppendLine($"[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]");
            sb.AppendLine($"public async Task<ActionResult<{pkField.TypeName}>> Create([FromBody] Create{cls.Name}Model request)");
            sb.IncreaseIndent();
            sb.AppendLine($"var id = await {serviceName}.Create(new Create{cls.Name}Command");
            sb.IncreaseIndent();
            foreach (var field in cls.Fields.Where(e => !e.IsPrimaryKey && !e.IsOnlyInDb))
            {
                sb.AppendLine($"{field.Name} = request.{field.Name},");
            }
            sb.DecreaseIndent(");");
            sb.AppendLine($"return CreatedAtAction(nameof(Get), new {{ {pkField.Name} = id }}, id);");
            sb.DecreaseIndent();
            sb.AppendLine();

            ///////////////GET
            GenerateXmlComment("summary", $"Gets {cls.Name}", sb);
            GenerateXmlComment($"param name=\"{pkFieldVarName}\"", $"{pkField.Name} of {cls.Name} to get details", sb, true);
            GenerateXmlComment("returns", $"Detail of given {cls.Name}", sb, true);
            GenerateXmlComment("response code=\"200\"", $"Detail of given {cls.Name}", sb, true);
            GenerateXmlComment("response code=\"404\"", $"If invalid <paramref name=\"{pkField.Name}\"/> was passed.", sb, true);
            sb.AppendLine($"[HttpGet(\"{{{pkFieldVarName}}}\")]");
            sb.AppendLine($"[ProducesResponseType(typeof({pkField.TypeName}), StatusCodes.Status201Created)]");
            sb.AppendLine($"[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]");
            sb.AppendLine($"public async Task<ActionResult<{cls.Name}>> Get([FromRoute] {pkField.TypeName} {pkFieldVarName})");
            sb.IncreaseIndent();
            sb.AppendLine($"var res = await {serviceName}.Get({pkFieldVarName});");
            sb.AppendLine($"return Ok(res);");
            sb.DecreaseIndent();
            sb.AppendLine();

            ///////////////UPDATE
            GenerateXmlComment("summary", $"Updates {cls.Name}", sb);
            GenerateXmlComment($"param name=\"{pkFieldVarName}\"", $"{pkField.Name} of {cls.Name} to be updated", sb, true);
            GenerateXmlComment($"param name=\"request\"", $"Complete new state of the {cls.Name}", sb, true);
            GenerateXmlComment("response code=\"204\"", $"Update was successful.", sb, true);
            GenerateXmlComment("response code=\"404\"", $"If invalid <paramref name=\"{pkFieldVarName}\"/> was passed.", sb, true);
            GenerateXmlComment("response code=\"400\"", $"If update cannot be performed, contains reason why.", sb, true);
            sb.AppendLine($"[HttpPatch(\"{{{pkFieldVarName}}}\")]");
            sb.AppendLine($"public async Task<ActionResult> Update([FromRoute] {pkField.TypeName} {pkFieldVarName}, [FromBody] Update{cls.Name}Model request)");
            sb.IncreaseIndent();
            sb.AppendLine($"await {serviceName}.Update(new Update{cls.Name}Command");
            sb.IncreaseIndent();
            foreach (var field in cls.Fields.Where(e => !e.IsPrimaryKey && !e.IsOnlyInDb && !e.IsOnlyCreate))
            {
                sb.AppendLine($"{field.Name} = request.{field.Name},");
            }
            sb.DecreaseIndent(");");
            sb.AppendLine($"return NoContent();");
            sb.DecreaseIndent();
            sb.AppendLine();

            ///////////////DELETE
            GenerateXmlComment("summary", $"Deletes {cls.Name}", sb);
            GenerateXmlComment($"param name=\"{pkFieldVarName}\"", $"{pkField.Name} of {cls.Name} to be deleted", sb, true);
            GenerateXmlComment("response code=\"204\"", $"Delete was successful.", sb, true);
            GenerateXmlComment("response code=\"404\"", $"If invalid <paramref name=\"{pkFieldVarName}\"/> was passed.", sb, true);
            sb.AppendLine($"[HttpDelete(\"{{{pkFieldVarName}}}\")]");
            sb.AppendLine($"public async Task<ActionResult> Delete([FromRoute] {pkField.TypeName} {pkFieldVarName})");
            sb.IncreaseIndent();
            sb.AppendLine($"await {serviceName}.Delete(new Delete{cls.Name}Command");
            sb.IncreaseIndent();
            sb.AppendLine($"{pkField.Name} = {pkFieldVarName}");
            sb.DecreaseIndent(");");
            sb.AppendLine($"return NoContent();");
            sb.DecreaseIndent();

            sb.DecreaseIndent();
            File.WriteAllText($"{BasePath}Controllers.{cls.Name}.g.cs", sb.ToString());
        }
    }

    public override bool ShouldGenerateField(Field field, string action)
    {
        if (field.IsOnlyInDb) return false;
        if (field.IsOnlyCreate && action == "updateModel") return false;
        if (field.IsPrimaryKey && action == "createModel") return false;
        if (field.IsPrimaryKey && action == "updateModel") return false;
        return true;
    }
}
