﻿using System.Text;

namespace ProjectGenerator;

public class CommandsGenerator : GeneratorBase
{
    //generates commands for service layer
    //create, update, delete

    public void Generate(DataModel dataModel)
    {
        var sb = new IndentingStringBuilder();
        sb.AppendLine($"using {Program.GeneratedProjectNamespace}.Models;");    //to potentially reuse base models. Only some generated project will need this using, we can add some conditional logic later.
        sb.AppendLine();
        sb.AppendLine($"namespace {Program.GeneratedProjectNamespace}.Commands;");
        sb.AppendLine();
        foreach (var cls in dataModel.Classes.Values.Where(e => e.IsModel))
        {
            sb.AppendLine($"public partial class Create{cls.Name}Command");
            GenerateFields(cls.Fields, sb, "createModel");

            sb.AppendLine($"public partial class Update{cls.Name}Command");
            GenerateFields(cls.Fields, sb, "updateModel");

            sb.AppendLine($"public partial class Delete{cls.Name}Command");
            GenerateFields(cls.Fields, sb, "deleteModel");
        }
        Directory.CreateDirectory($"{Program.BasePath}Services");
        File.WriteAllText($"{Program.BasePath}Services\\Commands.g.cs", sb.ToString());
    }

    public override bool ShouldGenerateField(Field field, string action)
    {
        if (field.IsOnlyInDb) return false;
        if (field.IsOnlyCreate && action == "updateModel") return false;
        if (field.IsPrimaryKey && field.IsAutogeneratedKey && action == "createModel") return false;
        if (action == "deleteModel" && !field.IsPrimaryKey) return false;
        return true;
    }
}
