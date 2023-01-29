using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGenerator
{
    public static class Utils
    {
        public static string GetTypeName(Type memberType)
        {
            var memberTypeName = memberType.Name;
            if (Aliases.ContainsKey(memberType))
            {
                return Aliases[memberType];
            }
            if (memberType.GenericTypeArguments.Count() > 0)        //collection, maybe not ideal test
            {
                memberTypeName = memberTypeName.Substring(0, memberTypeName.IndexOf('`'));
                return memberTypeName + $"<{memberType.GenericTypeArguments[0].Name.Substring(1)}>";
            }
            return memberTypeName.Substring(1);     //my custom type, maybe not ideal test
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
