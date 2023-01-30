using System.Text;
using System.Xml.Linq;

namespace ProjectGenerator;

public class RepositoriesGenerator : GeneratorBase
{
    //generates repositories

    public void Generate(DataModel dataModel)
    {
        foreach (var cls in dataModel.Classes.Values.Where(e => e.IsModel))
        {
            var serviceName = $"{cls.Name}Service";
            var repositoryName = $"_{Utils.LowerCaseFirst(cls.Name)}Repository";
            var pkField = cls.PrimaryKeyField();
            var pkFieldVarName = Utils.LowerCaseFirst(pkField.Name);
            var sb = new IndentingStringBuilder();

            sb.AppendLine($"using Microsoft.EntityFrameworkCore;");
            sb.AppendLine($"using {Program.GeneratedProjectNamespace}.Entities;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Program.GeneratedProjectNamespace}.Repositories;");
            sb.AppendLine();

            sb.AppendLine($"public class {cls.Name}Repository: I{cls.Name}Repository");
            sb.IncreaseIndent();
            sb.AppendLine($"protected DbSet<{cls.Name}> DbSet {{ get; }}");
            sb.AppendLine();
            sb.AppendLine($"public {cls.Name}Repository(DbSet<{cls.Name}> dbSet)");
            sb.IncreaseIndent();
            sb.AppendLine($"DbSet = dbSet;");
            sb.DecreaseIndent();
            sb.AppendLine();

            ///////////////GET
            sb.AppendLine($"public async Task<{cls.Name}Model> Get({pkField.TypeName} {pkFieldVarName})");
            sb.IncreaseIndent();
            sb.AppendLine($"return await DbSet.SingleOrDefaultAsync(e => e.{pkField.Name} == {pkFieldVarName});");
            sb.DecreaseIndent(";");
            sb.AppendLine();

            ///////////////GET for tracking
            sb.AppendLine($"public async Task<{cls.Name}Model> GetForUpdate({pkField.TypeName} {pkFieldVarName})");
            sb.IncreaseIndent();
            sb.AppendLine($"return await DbSet.AsTracking().SingleOrDefaultAsync(e => e.{pkField.Name} == {pkFieldVarName});");
            sb.DecreaseIndent(";");
            sb.AppendLine();

            ////////////SAVE
            sb.AppendLine($"public void Save({cls.Name} entity)");
            sb.IncreaseIndent();
            sb.AppendLine($"DbSet.Add(entity);");
            sb.DecreaseIndent();
            sb.AppendLine();

            ///////////////DELETE
            sb.AppendLine($"public void Delete({cls.Name} entity)");
            sb.IncreaseIndent();
            sb.AppendLine("DbSet.Remove(entity);");
            sb.DecreaseIndent(";");
            sb.AppendLine();

            sb.DecreaseIndent();
            Directory.CreateDirectory($"{Program.BasePath}Repositories\\Interfaces");
            File.WriteAllText($"{Program.BasePath}Repositories\\{cls.Name}Repository.g.cs", sb.ToString());

            sb = new IndentingStringBuilder();
            sb.AppendLine($"using {Program.GeneratedProjectNamespace}.Entities;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Program.GeneratedProjectNamespace}.Repositories;");
            sb.AppendLine();
            sb.AppendLine($"public interface I{cls.Name}Repository");
            sb.IncreaseIndent();
            sb.AppendLine($"async Task<{cls.Name}Model> Get({pkField.TypeName} {pkFieldVarName})");
            sb.AppendLine($"async Task<{cls.Name}Model> GetForUpdate({pkField.TypeName} {pkFieldVarName})");
            sb.AppendLine($"void Save({cls.Name} entity)");
            sb.AppendLine($"void Delete({cls.Name} entity)");
            sb.DecreaseIndent();
            File.WriteAllText($"{Program.BasePath}Repositories\\Interfaces\\I{cls.Name}Repository.g.cs", sb.ToString());
        }
    }

    public override bool ShouldGenerateField(Field field, string action)
    {
        if (field.IsOnlyInDb) return false;
        if (field.IsOnlyCreate && action == "updateModel") return false;
        if (field.IsPrimaryKey && action == "createModel") return false;
        if (field.IsPrimaryKey && action == "updateModel") return false;
        return true;
    }
}
