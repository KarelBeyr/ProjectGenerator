using System.Text;

namespace ProjectGenerator;

public class EntitiesGenerator : GeneratorBase
{
    public void Generate(DataModel dm)
    {
        var sb = new IndentingStringBuilder();
        sb.AppendLine("using Microsoft.EntityFrameworkCore;");
        sb.AppendLine("using Newtonsoft.Json;");
        sb.AppendLine($"using {dm.OutputNamespace}.Interfaces;");
        sb.AppendLine();
        sb.AppendLine($"namespace {dm.OutputNamespace}.Entities;");
        sb.AppendLine();
        foreach (var cls in dm.Classes.Values.Where(e => e.IsDbEntity))
        {
            var ifacesString = GetInterfacesString(cls);
            sb.AppendLine($"public partial class {cls.Name}{ifacesString}");
            GenerateFields(cls.Fields, sb);
        }
        File.WriteAllText($"{dm.BasePath}Entities.g.cs", sb.ToString());
    }

    public override bool ShouldGenerateField(Field field, string action) => !field.IsNotInDb;
}
