using System.Text;

namespace ProjectGenerator;

public class DbContextGenerator : GeneratorBase
{
    //generates {DbSchema}DbContext.cs

    public void Generate(DataModel dataModel)
    {
        var sb = new IndentingStringBuilder();
        sb.AppendLine($"using Microsoft.EntityFrameworkCore;");
        sb.AppendLine($"using {Program.GeneratedProjectNamespace}.Entities;");
        sb.AppendLine();
        sb.AppendLine($"namespace {Program.GeneratedProjectNamespace}s;");
        sb.AppendLine();
        sb.AppendLine($"public class {Program.DbSchema}DbContext : DbContext");
        sb.IncreaseIndent();
        sb.AppendLine($"public const string Schema = \"{Program.DbSchema}\";");
        sb.AppendLine();
        sb.AppendLine($"protected override void OnModelCreating(ModelBuilder modelBuilder)");
        sb.IncreaseIndent();
        sb.AppendLine("modelBuilder.HasDefaultSchema(Schema);");
        sb.AppendLine();
        
        foreach (var cls in dataModel.Classes.Values.Where(e => e.IsDbEntity))
        {
            sb.AppendLine($"modelBuilder.Entity<{cls.Name}>()");
            if (cls.PrimaryKeyFields().Count() == 1)
            {
                sb.AppendLine($"    .HasKey(e => e.{cls.PrimaryKeyField().Name});");
            } else
            {
                var keyString = string.Join(", ", cls.PrimaryKeyFields().Select(e => $"e.{e.Name}"));
                sb.AppendLine($"    .HasKey(e => new {{ {keyString} }} );");
            }
            sb.AppendLine();
        }
        sb.DecreaseIndent();
        sb.AppendLine();
        foreach (var cls in dataModel.Classes.Values.Where(e => e.IsDbEntity))
        {
            sb.AppendLine($"public DbSet<{cls.Name}> {cls.Name}s => Set<{cls.Name}>();");   //TODO pluralizer
        }
        sb.DecreaseIndent();

        File.WriteAllText($"{Program.BasePath}{Program.DbSchema}DbContext.g.cs", sb.ToString());
    }
}
