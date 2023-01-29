using ProjectGenerator.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static string CommentSummary(this PropertyInfo pi)
        {
            string commentSummary = null;
            var attrs = (CommentSummaryAttribute[])pi.GetCustomAttributes(typeof(CommentSummaryAttribute), false);
            if (attrs.Count() > 0)
            {
                commentSummary = attrs[0].Text;
            }
            return commentSummary;
        }

        public static string CommentSummary(this Type type)
        {
            string commentSummary = null;
            var attrs = (CommentSummaryAttribute[])type.GetCustomAttributes(typeof(CommentSummaryAttribute), false);
            if (attrs.Count() > 0)
            {
                commentSummary = attrs[0].Text;
            }
            return commentSummary;
        }
    }
}
