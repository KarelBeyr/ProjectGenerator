using ProjectGenerator.InputInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGenerator
{
    public class InterfacesGenerator
    {
        public void Generate(Dictionary<Type, IEnumerable<PropertyInfo>> entityDict, Dictionary<Type, IEnumerable<PropertyInfo>> ifaceDict)
        {
            var sb = new StringBuilder();
            sb.AppendLine("namespace GeneratedProject;");   //TODO project name prefix
            sb.AppendLine();
            foreach (var kv in ifaceDict)
            {
                var type = kv.Key;
                var pis = kv.Value;
                var ifaces = type.GetInterfaces();
                var ifacesString = $"";
                if (ifaces.Count() > 0)
                {
                    ifacesString = " : " + string.Join(", ", ifaces.Select(e => e.Name));
                }
                sb.AppendLine($"public interface {type.Name}{ifacesString}");
                sb.AppendLine("{");
                foreach (var member in pis)
                {
                    if (member.CustomAttributes.SingleOrDefault(e => e.AttributeType == typeof(ProjectGenerator.Annotations.NotInDbAttribute)) == null)
                    {
                        var memberType = member.PropertyType;
                        var memberTypeName = memberType.Name;
                        if (Aliases.ContainsKey(memberType))
                        {
                            memberTypeName = Aliases[memberType];
                        }
                        if (memberType.GenericTypeArguments.Count() > 0)        //kolekce, mozna nedokonaly test
                        {
                            memberTypeName = memberTypeName.Substring(0, memberTypeName.IndexOf('`'));
                            memberTypeName = memberTypeName + $"<{memberType.GenericTypeArguments[0].Name.Substring(1)}>";
                        }
                        if (entityDict.ContainsKey(memberType))
                        {
                            memberTypeName = memberTypeName.Substring(1);
                        }
                        sb.AppendLine($"    public {memberTypeName} {member.Name} {{ get; set; }}");
                    }
                }
                sb.AppendLine("}");

            }
            Console.WriteLine();
            File.WriteAllText(@"c:\projects\GeneratedProject\GeneratedProject\Interfaces.cs", sb.ToString());
        }

        private static readonly Dictionary<Type, string> Aliases =
            new Dictionary<Type, string>()
            {
                { typeof(byte), "byte" },
                { typeof(sbyte), "sbyte" },
                { typeof(short), "short" },
                { typeof(ushort), "ushort" },
                { typeof(int), "int" },
                { typeof(uint), "uint" },
                { typeof(long), "long" },
                { typeof(ulong), "ulong" },
                { typeof(float), "float" },
                { typeof(double), "double" },
                { typeof(decimal), "decimal" },
                { typeof(object), "object" },
                { typeof(bool), "bool" },
                { typeof(char), "char" },
                { typeof(string), "string" },
                { typeof(void), "void" }
            };
    }
}
