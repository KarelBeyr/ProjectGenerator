using System.Text;

namespace ProjectGenerator;

public class EntitiesGenerator : GeneratorBase
{
    public void Generate(DataModel dataModel)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using Microsoft.EntityFrameworkCore;");
        sb.AppendLine("using Newtonsoft.Json;");
        sb.AppendLine("using ResourceInventory.NG.Models;");
        sb.AppendLine();
        sb.AppendLine("namespace GeneratedProject;");   //TODO project name prefix
        sb.AppendLine();
        foreach (var cls in dataModel.Classes.Values)
        {
            var ifacesString = GetInterfacesString(cls);
            sb.AppendLine($"public class {cls.Name}{ifacesString}");
            GenerateFields(cls.Fields, sb);
        }
        File.WriteAllText($"{BasePath}Entities.cs", sb.ToString());
    }
}
