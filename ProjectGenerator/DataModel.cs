using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGenerator
{
    public class DataModel
    {
        public Dictionary<string, Class> Classes { get; set; }
        public Dictionary<string, Iface> Interfaces { get; set; }

        public DataModel()
        {
            Classes = new Dictionary<string, Class>();
            Interfaces = new Dictionary<string, Iface>();
        }

        public Iface AddInterface(Type type)
        {
            var name = type.Name;
            if (Interfaces.ContainsKey(name)) return Interfaces[name];
            var members = type.GetMembers();
            var filteredMembers = members.Where(e => e.GetType().Name == "RuntimePropertyInfo").Select(e => (PropertyInfo)e);
            var fields = filteredMembers.Select(e =>
            {
                var notInDb = e.CustomAttributes.Any(e => e.AttributeType == typeof(Annotations.NotInDbAttribute));
                return new Field() { Name = e.Name, TypeName = Utils.GetTypeName(e.PropertyType), IsNotInDb = notInDb };
            }).ToList();

            var iface = new Iface()
            {
                Name = type.Name,
                Interfaces = type.GetInterfaces().Select(e => AddInterface(e)).ToList(),
                Fields = fields
            };
            
            Interfaces[name] = iface;
            return iface;
        }

        public Class AddClass(Type type)
        {
            var name = type.Name.Substring(1);

            if (Classes.ContainsKey(name)) return Classes[name];
            var inheritedMembers = type.GetInterfaces().SelectMany(x => x.GetMembers());
            var ownMembers = type.GetMembers();

            var allMembers = ownMembers.ToList();
            allMembers.AddRange(inheritedMembers);
            var filteredMembers = allMembers.Where(e => e.GetType().Name == "RuntimePropertyInfo").Select(e => (PropertyInfo)e);
            var fields = filteredMembers.Select(e =>
            {
                var notInDb = e.CustomAttributes.Any(e => e.AttributeType == typeof(Annotations.NotInDbAttribute));
                var onlyInDb = e.CustomAttributes.Any(e => e.AttributeType == typeof(Annotations.OnlyInDbAttribute));
                var onlyCreate = e.CustomAttributes.Any(e => e.AttributeType == typeof(Annotations.OnlyCreateAttribute));
                var isPrimaryKey = e.CustomAttributes.Any(e => e.AttributeType == typeof(Annotations.PrimaryKeyAttribute));
                return new Field() { Name = e.Name, TypeName = Utils.GetTypeName(e.PropertyType), IsNotInDb = notInDb, IsOnlyInDb = onlyInDb, IsOnlyCreate = onlyCreate, IsPrimaryKey = isPrimaryKey };
            }).ToList();

            var cls = new Class()
            {
                Name = type.Name.Substring(1),
                Interfaces = type.GetInterfaces().Select(e => AddInterface(e)).ToList(),
                Fields = fields,
                IsDbEntity = type.CustomAttributes.Any(e => e.AttributeType == typeof(Annotations.DbEntityAttribute)),
                IsModel = type.CustomAttributes.Any(e => e.AttributeType == typeof(Annotations.ModelAttribute))
            };

            Classes[name] = cls;
            return cls;
        }
    }

    [DebuggerDisplay("{TypeName} {Name}")]
    public class Field
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public bool IsNotInDb { get; set; }
        public bool IsOnlyInDb { get; set; }
        public bool IsOnlyCreate { get; set; }
        public bool IsPrimaryKey { get; set; }
    }

    [DebuggerDisplay("Interface {Name}")]
    public class Iface : IHasInterfaces
    {
        public List<Iface> Interfaces { get; set; }
        public string Name { get; set; }
        public List<Field> Fields { get; set; }
    }

    [DebuggerDisplay("Class {Name}")]
    public class Class : IHasInterfaces
    {
        public List<Iface> Interfaces { get; set; }
        public string Name { get; set; }
        public List<Field> Fields { get; set; }
        public bool IsDbEntity { get; set; }
        public bool IsModel { get; set; }
    }

    public interface IHasInterfaces
    {
        public List<Iface> Interfaces { get; set; }
    }
}
