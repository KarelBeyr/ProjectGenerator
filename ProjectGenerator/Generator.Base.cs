using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGenerator
{
    public class GeneratorBase
    {
        public string BasePath = @"c:\projects\GeneratedProject\GeneratedProject\";
        public string GeneratedProjectNamespace = "GeneratedProject";

        public void GenerateFields(IEnumerable<Field> fields, IndentingStringBuilder sb, string action = "")
        {
            sb.IncreaseIndent();
            foreach (var field in fields)
            {
                if (ShouldGenerateField(field, action))
                {
                    GenerateXmlCommentSummary(field, sb);
                    sb.AppendLine($"public {field.TypeName} {field.Name} {{ get; set; }}");
                }
            }
            sb.DecreaseIndent();
        }

        public string GetInterfacesString(IHasInterfaces ihasInterface)
        {
            var ifaces = ihasInterface.Interfaces;
            var ifacesString = $"";
            if (ifaces.Count() > 0)
            {
                ifacesString = " : " + string.Join(", ", ifaces.Select(e => e.Name));
            }
            return ifacesString;
        }

        public void GenerateXmlCommentSummary(IHasCommentSummary ihasCommentSummary, IndentingStringBuilder sb)
        {
            if (ihasCommentSummary.CommentSummary != null)
            {
                GenerateXmlComment("summary", ihasCommentSummary.CommentSummary, sb);
            }
        }

        public void GenerateXmlComment(string xmlElement, string comment, IndentingStringBuilder sb, bool inline = false)
        {
            var tagName = xmlElement;
            if (xmlElement.Contains(' '))
            {
                tagName = tagName.Substring(0, xmlElement.IndexOf(' '));
            }
            if (inline)
            {
                sb.AppendLine($"/// <{xmlElement}>{comment}</{tagName}>");
            }
            else
            {
                sb.AppendLine($"/// <{xmlElement}>");
                sb.AppendLine($"/// {comment}");
                sb.AppendLine($"/// </{tagName}>");
            }
        }

        public virtual bool ShouldGenerateField(Field field, string action = "") => true;
    }
}
