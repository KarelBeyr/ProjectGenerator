using ProjectGenerator.InputInterfaces;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace ProjectGenerator;

public class Program
{
    public static void Main()
    {
        var prog = new Program();
        prog.Run();
    }

    public void Run()
    {
        var allTypes = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "ProjectGenerator.InputInterfaces");
        var dataModel = new DataModel();
        foreach (var type in allTypes)
        { 
            if (type.CustomAttributes.SingleOrDefault(e => e.AttributeType == typeof(Annotations.DbEntityAttribute)) != null)
            {
                dataModel.AddClass(type);
            } else
            {
                dataModel.AddInterface(type);
            }
        }
        new EntitiesGenerator().Generate(dataModel);
        new InterfacesGenerator().Generate(dataModel);
        new ModelsGenerator().Generate(dataModel);
        new CommandsGenerator().Generate(dataModel);
    }

    private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
    {
        return assembly.GetTypes()
                  .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                  .ToArray();
    }



}