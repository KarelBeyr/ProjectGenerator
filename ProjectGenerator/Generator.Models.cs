﻿using System.Text;

namespace ProjectGenerator;

public class ModelsGenerator : GeneratorBase
{
    //generates models for controllers that come from REST API
    //create, update, delete

    public void Generate(DataModel dataModel)
    {
        var sb = new IndentingStringBuilder();
        sb.AppendLine($"namespace {GeneratedProjectNamespace}.Models;");
        sb.AppendLine();
        foreach (var cls in dataModel.Classes.Values.Where(e => e.IsModel))
        {
            GenerateXmlCommentSummary(cls, sb);
            sb.AppendLine($"public partial class {cls.Name}Model");
            GenerateFields(cls.Fields, sb, "model");

            sb.AppendLine($"public partial class Create{cls.Name}Model");
            GenerateFields(cls.Fields, sb, "createModel");

            sb.AppendLine($"public partial class Update{cls.Name}Model");
            GenerateFields(cls.Fields, sb, "updateModel");
        }
        File.WriteAllText($"{BasePath}Models.g.cs", sb.ToString());
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