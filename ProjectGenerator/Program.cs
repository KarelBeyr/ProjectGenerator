using ProjectGenerator.InputInterfaces;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Xml.Linq;

namespace ProjectGenerator;

public class Program
{
    public static void Main()
    {
        var prog = new Program();
        prog.Run(
        //TODO1: Update file InputInterfaces.cs
        //TODO2: Set those constants
            basePath: @"c:\projects\GeneratedProject\GeneratedProject\", 
            outputNamespace: "ConfigProvider", 
            dbSchema: "Conf", 
            sourceNamespace: "ProjectGenerator.InputInterfaces");
        //TODO3: Run and 🙏 
    }

    public void Run(string basePath, string outputNamespace, string dbSchema, string sourceNamespace)
    {
        var dataModel = CreateDataModel(basePath, outputNamespace, dbSchema, sourceNamespace);

        new EntitiesGenerator().Generate(dataModel);
        new InterfacesGenerator().Generate(dataModel);
        new ModelsGenerator().Generate(dataModel);
        new CommandsGenerator().Generate(dataModel);
        new ControllersGenerator().Generate(dataModel);
        new ServicesGenerator().Generate(dataModel);
        new RepositoriesGenerator().Generate(dataModel);
        new DbContextGenerator().Generate(dataModel);
    }

    DataModel CreateDataModel(string basePath, string outputNamespace, string dbSchema, string sourceNamespace)
    {
        var dataModel = new DataModel(basePath, outputNamespace, dbSchema);
        //var allTypes = Assembly.GetExecutingAssembly().GetTypes()
        //  .Where(t => String.Equals(t.Namespace, sourceNamespace, StringComparison.Ordinal))
        //  .ToArray();

        var allTypes = AppDomain.CurrentDomain.GetAssemblies().
           SingleOrDefault(assembly => assembly.GetName().Name == sourceNamespace).GetTypes()
          .Where(t => String.Equals(t.Namespace, sourceNamespace, StringComparison.Ordinal))
          .ToArray();

        foreach (var type in allTypes)
        {
            if (type.CustomAttributes.SingleOrDefault(e => e.AttributeType == typeof(Annotations.DbEntityAttribute)) != null)
            {
                dataModel.AddClass(type);
            }
            //else
            //{
            //    dataModel.AddInterface(type);
            //}
        }
        return dataModel;
    }
}