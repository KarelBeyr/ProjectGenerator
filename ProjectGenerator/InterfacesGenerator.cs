using System.Text;

namespace ProjectGenerator;

public class InterfacesGenerator : GeneratorBase
{
    public void Generate(DataModel dataModel)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"namespace {GeneratedProjectNamespace}.Interfaces;");   //TODO project name prefix
        sb.AppendLine();
        foreach (var iface in dataModel.Interfaces.Values)
        {
            var ifacesString = GetInterfacesString(iface);
            sb.AppendLine($"public interface {iface.Name}{ifacesString}");
            GenerateFields(iface.Fields, sb);
        }
        File.WriteAllText($"{BasePath}Interfaces.cs", sb.ToString());
    }
}
