﻿using System;
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

        public virtual bool ShouldGenerateField(Field field, string action = "") => true;
    }
}