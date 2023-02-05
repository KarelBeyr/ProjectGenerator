using ProjectGenerator.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
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
                var genericMemberType = memberType.GenericTypeArguments[0];
                var genericMemberTypeName = genericMemberType.Name.Substring(1);
                if (Aliases.ContainsKey(genericMemberType))
                {
                    genericMemberTypeName = Aliases[genericMemberType];
                }

                return memberTypeName + $"<{genericMemberTypeName}>";
            }
            return memberTypeName.Substring(1);     //my custom type, maybe not ideal test
        }

        private static readonly Dictionary<Type, string> Aliases =
            new Dictionary<Type, string>()
            {
                { typeof(Guid), "Guid" },   //HACK
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

        public static string LowerCaseFirst(string text)
        {
            return char.ToLower(text[0]) + text.Substring(1);
        }
    }

    public class IndentingStringBuilder
    {
        public StringBuilder Sb;
        public int indent = 0;

        public IndentingStringBuilder()
        {
            Sb = new StringBuilder();
        }

        public void AppendLine(string text)
        {
            Sb.Append(new String(' ', indent));
            Sb.AppendLine(text);
        }

        public void IncreaseIndent()
        {
            AppendLine("{");
            indent += 4;
        }

        public void DecreaseIndent(string additionalText = "")
        {
            indent -= 4;
            AppendLine($"}}{additionalText}");
        }
        public void AppendLine() => Sb.AppendLine();
        public string ToString() => Sb.ToString();
    }

    public static class MyEx
    {
        public static bool HasAttribute(this PropertyInfo pi, Type attrType)
        {
            return pi.CustomAttributes.Any(e => e.AttributeType == attrType);
        }

        public static bool HasAttribute(this Type type, Type attrType)
        {
            return type.CustomAttributes.Any(e => e.AttributeType == attrType);
        }

        public static string ControllerFromHeaderName(this PropertyInfo pi)
        {
            var attrs = (ControllerFromHeaderAttribute[])pi.GetCustomAttributes(typeof(ControllerFromHeaderAttribute), false);
            if (attrs.Count() > 0)
            {
                return attrs[0].HeaderName;
            }
            return null;
        }

        public static PrimaryKey PrimaryKey(this PropertyInfo pi)
        {
            var attrs = (PrimaryKeyAttribute[])pi.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
            if (attrs.Count() > 0)
            {
                return new PrimaryKey(attrs[0].IsAutogonerated, attrs[0].IsOptional);
            }
            return null;
        }

        public static string CommentSummary(this PropertyInfo pi)
        {
            var attrs = (CommentSummaryAttribute[])pi.GetCustomAttributes(typeof(CommentSummaryAttribute), false);
            if (attrs.Count() > 0)
            {
                return attrs[0].Text;
            }
            return null;
        }

        public static string CommentSummary(this Type type)
        {
            var attrs = (CommentSummaryAttribute[])type.GetCustomAttributes(typeof(CommentSummaryAttribute), false);
            if (attrs.Count() > 0)
            {
                return attrs[0].Text;
            }
            return null;
        }
    }
}
