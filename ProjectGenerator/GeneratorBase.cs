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

        public void GenerateFields(IEnumerable<Field> fields, StringBuilder sb, string action = "")
        {
            sb.AppendLine("{");
            foreach (var field in fields)
            {
                if (ShouldGenerateField(field, action))
                {
                    GenerateCommentSummary(field, sb);
                    sb.AppendLine($"    public {field.TypeName} {field.Name} {{ get; set; }}");
                }
            }
            sb.AppendLine("}");
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

        public void GenerateCommentSummary(IHasCommentSummary ihasCommentSummary, StringBuilder sb)
        {
            if (ihasCommentSummary.CommentSummary != null)
            {
                var indent = "";
                if (ihasCommentSummary.GetType() == typeof(Field))
                {
                    indent = "    ";
                }
                sb.AppendLine($"{indent}/// <summary>");
                sb.AppendLine($"{indent}/// {ihasCommentSummary.CommentSummary}");
                sb.AppendLine($"{indent}/// </summary>");
            }
        }

        public virtual bool ShouldGenerateField(Field field, string action = "") => true;
    }
}
