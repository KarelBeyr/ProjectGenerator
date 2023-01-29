using System.Text;

namespace ProjectGenerator;

public class CommandsGenerator : GeneratorBase
{
    //generates commands for service layer
    //create, update, delete

    public void Generate(DataModel dataModel)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"using {GeneratedProjectNamespace}.Models;");    //to potentially reuse base models
        sb.AppendLine();
        sb.AppendLine($"namespace {GeneratedProjectNamespace}.Commands;");
        sb.AppendLine();
        foreach (var cls in dataModel.Classes.Values.Where(e => e.IsModel))
        {
            sb.AppendLine($"public partial class Create{cls.Name}Command");
            GenerateFields(cls.Fields, sb, "createModel");

            sb.AppendLine($"public partial class Update{cls.Name}Command");
            GenerateFields(cls.Fields, sb, "updateModel");
        }
        File.WriteAllText($"{BasePath}Commands.cs", sb.ToString());
    }

    public override bool ShouldGenerateField(Field field, string action)
    {
        if (field.IsOnlyInDb) return false;
        if (field.IsOnlyCreate && action == "updateModel") return false;
        if (field.IsPrimaryKey && action == "createModel") return false;
        return true;
    }
}
