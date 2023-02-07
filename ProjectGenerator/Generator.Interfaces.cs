using System.Text;

namespace ProjectGenerator;

public class InterfacesGenerator : GeneratorBase
{
    public void Generate(DataModel dm)
    {
        var sb = new IndentingStringBuilder();
        sb.AppendLine($"namespace {dm.OutputNamespace}.Interfaces;");
        sb.AppendLine();
        foreach (var iface in dm.Interfaces.Values)
        {
            var ifacesString = GetInterfacesString(iface);
            sb.AppendLine($"public interface {iface.Name}{ifacesString}");
            GenerateFields(iface.Fields, sb);
        }
        File.WriteAllText($"{dm.BasePath}Interfaces.cs", sb.ToString());
    }
}
