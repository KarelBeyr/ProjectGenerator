using System.Text;

namespace ProjectGenerator;

public class DbContextGenerator : GeneratorBase
{
    //generates {DbSchema}DbContext.cs

    public void Generate(DataModel dm)
    {
        var sb = new IndentingStringBuilder();
        sb.AppendLine($"using Microsoft.EntityFrameworkCore;");
        sb.AppendLine($"using {dm.OutputNamespace}.Entities;");
        sb.AppendLine();
        sb.AppendLine($"namespace {dm.OutputNamespace};");
        sb.AppendLine();
        sb.AppendLine($"public class {dm.DbSchema}DbContext : DbContext");
        sb.IncreaseIndent();
        sb.AppendLine($"public const string Schema = \"{dm.DbSchema}\";");
        sb.AppendLine($"public {dm.DbSchema}DbContext(DbContextOptions options) : base(options)");
        sb.IncreaseIndent();
        sb.DecreaseIndent();
        sb.AppendLine();
        sb.AppendLine($"protected override void OnModelCreating(ModelBuilder modelBuilder)");
        sb.IncreaseIndent();
        sb.AppendLine("modelBuilder.HasDefaultSchema(Schema);");
        sb.AppendLine();
        
        foreach (var cls in dm.Classes.Values.Where(e => e.IsDbEntity))
        {
            sb.AppendLine($"modelBuilder.Entity<{cls.Name}>()");
            if (cls.PrimaryKeyFields().Count() == 1)
            {
                sb.AppendLine($"    .HasKey(e => e.{cls.PrimaryKeyFields().First().Name});");
            } else
            {
                var keyString = string.Join(", ", cls.PrimaryKeyFields().Select(e => $"e.{e.Name}"));
                sb.AppendLine($"    .HasKey(e => new {{ {keyString} }} );");
            }
            sb.AppendLine();
        }
        sb.DecreaseIndent();
        sb.AppendLine();
        foreach (var cls in dm.Classes.Values.Where(e => e.IsDbEntity))
        {
            sb.AppendLine($"public DbSet<{cls.Name}> {cls.Name}s => Set<{cls.Name}>();");   //TODO pluralizer
        }
        sb.DecreaseIndent();

        File.WriteAllText($"{dm.BasePath}{dm.DbSchema}DbContext..cs", sb.ToString());
    }
}
