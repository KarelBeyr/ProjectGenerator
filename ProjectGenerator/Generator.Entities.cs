using System.Text;

namespace ProjectGenerator;

public class EntitiesGenerator : GeneratorBase
{
    public void Generate(DataModel dm)
    {
        foreach (var cls in dm.Classes.Values.Where(e => e.IsDbEntity))
        {
            var sb = new IndentingStringBuilder();
//            sb.AppendLine("using Microsoft.EntityFrameworkCore;");
//            sb.AppendLine("using Newtonsoft.Json;");
//            sb.AppendLine($"using {dm.OutputNamespace}.Interfaces;");
//            sb.AppendLine();
            sb.AppendLine($"namespace {dm.OutputNamespace}.Entities;");
            sb.AppendLine();
            var ifacesString = GetInterfacesString(cls);
            sb.AppendLine($"public class {cls.Name}{ifacesString}");
            GenerateFields(cls.Fields, sb);
            Directory.CreateDirectory($"{dm.BasePath}Entities");
            File.WriteAllText($"{dm.BasePath}Entities\\{cls.Name}.cs", sb.ToString());
        }
    }

    public override bool ShouldGenerateField(Field field, string action) => !field.IsNotInDb;
}
