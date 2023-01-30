using System.Text;

namespace ProjectGenerator;

public class InterfacesGenerator : GeneratorBase
{
    public void Generate(DataModel dataModel)
    {
        var sb = new IndentingStringBuilder();
        sb.AppendLine($"namespace {Program.GeneratedProjectNamespace}.Interfaces;");
        sb.AppendLine();
        foreach (var iface in dataModel.Interfaces.Values)
        {
            var ifacesString = GetInterfacesString(iface);
            sb.AppendLine($"public interface {iface.Name}{ifacesString}");
            GenerateFields(iface.Fields, sb);
        }
        File.WriteAllText($"{Program.BasePath}Interfaces.g.cs", sb.ToString());
    }
}
