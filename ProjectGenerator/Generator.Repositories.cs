using System.Text;
using System.Xml.Linq;

namespace ProjectGenerator;

public class RepositoriesGenerator : GeneratorBase
{
    //generates repositories

    public void Generate(DataModel dm)
    {
        foreach (var cls in dm.Classes.Values.Where(e => e.IsModel))
        {
            var serviceName = $"{cls.Name}Service";
            var repositoryName = $"_{Utils.LowerCaseFirst(cls.Name)}Repository";
            var requiredPrimaryKeys = cls.PrimaryKeyFields().Where(e => e.PrimaryKey.IsOptional == false);

            var pkField = cls.PrimaryKeyFields().First();
            var pkFieldVarName = Utils.LowerCaseFirst(pkField.Name);
            var sb = new IndentingStringBuilder();

            sb.AppendLine($"using Microsoft.EntityFrameworkCore;");
            sb.AppendLine($"using {dm.OutputNamespace}.Entities;");
            sb.AppendLine();
            sb.AppendLine($"namespace {dm.OutputNamespace}.Repositories;");
            sb.AppendLine();

            sb.AppendLine($"public class {cls.Name}Repository: I{cls.Name}Repository");
            sb.IncreaseIndent();
            sb.AppendLine($"private DbSet<{cls.Name}> DbSet {{ get; }}");
            sb.AppendLine();
            sb.AppendLine($"public {cls.Name}Repository(DbSet<{cls.Name}> dbSet)");
            sb.IncreaseIndent();
            sb.AppendLine($"DbSet = dbSet;");
            sb.DecreaseIndent();
            sb.AppendLine();

            ///////////////GET
            var paramNames = new List<string>();
            paramNames.AddRange(cls.PrimaryKeyFields().Where(e => e.ControllerFromHeader == null).Select(e => $"{e.TypeName} {Utils.LowerCaseFirst(e.Name)}"));
            paramNames.AddRange(cls.PrimaryKeyFields().Where(e => e.ControllerFromHeader != null).Select(e => $"{e.TypeName} {Utils.LowerCaseFirst(e.Name)}"));
            var inputParameters = string.Join(", ", paramNames);

            sb.AppendLine($"public async Task<{cls.Name}> Get({inputParameters})");
            sb.IncreaseIndent();

            paramNames = new List<string>();
            paramNames.AddRange(cls.PrimaryKeyFields().Where(e => e.ControllerFromHeader == null).Select(e => $"e.{e.Name} == {Utils.LowerCaseFirst(e.Name)}"));
            paramNames.AddRange(cls.PrimaryKeyFields().Where(e => e.ControllerFromHeader != null).Select(e => $"e.{e.Name} == {Utils.LowerCaseFirst(e.Name)}"));
            var comparisonParameters = string.Join(" && ", paramNames);


            sb.AppendLine($"return await DbSet.SingleOrDefaultAsync(e => {comparisonParameters});");
            sb.DecreaseIndent();
            sb.AppendLine();

            ///////////////GET for tracking
            sb.AppendLine($"public async Task<{cls.Name}> GetForUpdate({inputParameters})");
            sb.IncreaseIndent();
            sb.AppendLine($"return await DbSet.AsTracking().SingleOrDefaultAsync(e => {comparisonParameters});");
            sb.DecreaseIndent();
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
            sb.DecreaseIndent();
            sb.AppendLine();

            sb.DecreaseIndent();
            Directory.CreateDirectory($"{dm.BasePath}Repositories\\Interfaces");
            File.WriteAllText($"{dm.BasePath}Repositories\\{cls.Name}Repository..cs", sb.ToString());

            sb = new IndentingStringBuilder();
            sb.AppendLine($"using {dm.OutputNamespace}.Entities;");
            sb.AppendLine();
            sb.AppendLine($"namespace {dm.OutputNamespace}.Repositories;");
            sb.AppendLine();
            sb.AppendLine($"public interface I{cls.Name}Repository");
            sb.IncreaseIndent();
            sb.AppendLine($"Task<{cls.Name}> Get({inputParameters});");
            sb.AppendLine($"Task<{cls.Name}> GetForUpdate({inputParameters});");
            sb.AppendLine($"void Save({cls.Name} entity);");
            sb.AppendLine($"void Delete({cls.Name} entity);");
            sb.DecreaseIndent();
            File.WriteAllText($"{dm.BasePath}Repositories\\Interfaces\\I{cls.Name}Repository.cs", sb.ToString());
        }
    }

    public override bool ShouldGenerateField(Field field, string action)
    {
        if (field.IsOnlyInDb) return false;
        if (field.IsReadOnly && action == "updateModel") return false;
        if (field.IsPrimaryKey() && action == "createModel") return false;
        if (field.IsPrimaryKey() && action == "updateModel") return false;
        return true;
    }
}
