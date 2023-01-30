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

    //TODO1: Update file InputInterfaces.cs
    //TODO2: Set those constants
    public static string BasePath = @"c:\projects\GeneratedProject\GeneratedProject\";
    public static string GeneratedProjectNamespace = "GeneratedProject";
    public static string DbSchema = "SecNG";
    //TODO3: Run and 🙏 

    public void Run()
    {
        var dataModel = CreateDataModel();

        new EntitiesGenerator().Generate(dataModel);
        new InterfacesGenerator().Generate(dataModel);
        new ModelsGenerator().Generate(dataModel);
        new CommandsGenerator().Generate(dataModel);
        new ControllersGenerator().Generate(dataModel);
        new ServicesGenerator().Generate(dataModel);
        new RepositoriesGenerator().Generate(dataModel);
        new DbContextGenerator().Generate(dataModel);
    }

    DataModel CreateDataModel()
    {
        var dataModel = new DataModel();
        var allTypes = Assembly.GetExecutingAssembly().GetTypes()
          .Where(t => String.Equals(t.Namespace, "ProjectGenerator.InputInterfaces", StringComparison.Ordinal))
          .ToArray();

        foreach (var type in allTypes)
        {
            if (type.CustomAttributes.SingleOrDefault(e => e.AttributeType == typeof(Annotations.DbEntityAttribute)) != null)
            {
                dataModel.AddClass(type);
            }
            else
            {
                dataModel.AddInterface(type);
            }
        }
        return dataModel;
    }
}