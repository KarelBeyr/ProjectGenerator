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
        var entityTypes = allTypes.Where(e => e.GetInterfaces().Contains(typeof(IEntity)));
        var interfaceTypes = allTypes.Where(e => !e.GetInterfaces().Contains(typeof(IEntity)));

        var entityDict = new Dictionary<Type, IEnumerable<PropertyInfo>>();
        var ifaceDict = new Dictionary<Type, IEnumerable<PropertyInfo>>();
        foreach (var type in allTypes)
        { 
            Console.WriteLine(type.Name);

            var inheritedMembers = type.GetInterfaces().SelectMany(x => x.GetMembers());
            var ownMembers = type.GetMembers();

            var allMembers = ownMembers.ToList();
            IEnumerable<PropertyInfo> filteredMembers;

            if (type.GetInterfaces().Contains(typeof(IEntity)))
            {
                allMembers.AddRange(inheritedMembers);
                filteredMembers = allMembers.Where(e => e.GetType().Name == "RuntimePropertyInfo").Select(e => (PropertyInfo)e);
                entityDict[type] = filteredMembers;
            } else
            {
                filteredMembers = allMembers.Where(e => e.GetType().Name == "RuntimePropertyInfo").Select(e => (PropertyInfo)e);
                ifaceDict[type] = filteredMembers;
            }

            foreach (var member in filteredMembers)
            {
                Console.WriteLine($" {member.PropertyType.Name} {member.Name}");
                foreach (var ca in member.CustomAttributes)
                {
                    Console.WriteLine($"  {ca}");
                }
            }
            Console.WriteLine();
        }
        new EntitiesGenerator().Generate(entityDict);
        new InterfacesGenerator().Generate(entityDict, ifaceDict);
        
    }

    private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
    {
        return assembly.GetTypes()
                  .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                  .ToArray();
    }


}