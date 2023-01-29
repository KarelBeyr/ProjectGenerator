using System.Text;

namespace ProjectGenerator;

public class EntitiesGenerator : GeneratorBase
{
    public void Generate(DataModel dataModel)
    {
        var sb = new IndentingStringBuilder();
        sb.AppendLine("using Microsoft.EntityFrameworkCore;");
        sb.AppendLine("using Newtonsoft.Json;");
        sb.AppendLine($"using {GeneratedProjectNamespace}.Interfaces;");
        sb.AppendLine();
        sb.AppendLine($"namespace {GeneratedProjectNamespace}.Entities;");
        sb.AppendLine();
        foreach (var cls in dataModel.Classes.Values.Where(e => e.IsDbEntity))
        {
            var ifacesString = GetInterfacesString(cls);
            sb.AppendLine($"public partial class {cls.Name}{ifacesString}");
            GenerateFields(cls.Fields, sb);
        }
        File.WriteAllText($"{BasePath}Entities.g.cs", sb.ToString());
    }

    public override bool ShouldGenerateField(Field field, string action) => !field.IsNotInDb;
}
