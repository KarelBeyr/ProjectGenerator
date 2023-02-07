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
            basePath: @"c:\projects\ConfigProvider\ConfigProvider\", 
            outputNamespace: "ConfigProvider", 
            dbSchema: "CP", 
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
        var allTypes = AppDomain.CurrentDomain.GetAssemblies()
           .SelectMany(e => e.GetTypes())
           .Where(t => String.Equals(t.Namespace, sourceNamespace, StringComparison.Ordinal));

        foreach (var type in allTypes)
        {
            if (type.CustomAttributes.SingleOrDefault(e => e.AttributeType == typeof(Annotations.DbEntityAttribute)) != null)
            {
                dataModel.AddClass(type);
            }
            //TODO if we miss some interfaces, we can either annotate them, or change the namespaces and then add everything. But if we change namespace, tests don't see it for some reason. So perhaps keep it like that and hope they will get transitively loaded
            //else
            //{
            //    dataModel.AddInterface(type);
            //}
        }
        return dataModel;
    }
}