using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Text;

namespace ProjectGenerator.Tests
{
    public class Tests
    {
        [Test]
        [TestCase("Controllers\\ConfigurationController.g.cs", "    /// Creates new Configuration", -1, Resources.ConfigurationControllerCreate)]
        [TestCase("Controllers\\ConfigurationController.g.cs", "    [HttpGet(\"{key}\")]", -8, Resources.ConfigurationControllerGet)]
        [TestCase("Controllers\\ConfigurationController.g.cs", "    /// Updates Configuration", -1, Resources.ConfigurationControllerUpdate)]
        [TestCase("Controllers\\ConfigurationController.g.cs", "    /// Deletes Configuration", -1, Resources.ConfigurationControllerDelete)]
        [TestCase("Services\\ConfigurationService.g.cs", "    async Task<ConfigurationModel> IConfigurationService.Get(string key, string serviceName)", 0, Resources.ConfigurationServiceGet)]
        [TestCase("Controllers\\Models.g.cs", "public partial class UserSettingModel", 0, Resources.Models_UserSettingModel)]
        public void Test_ConfigurationProvider(string path, string search, int previousLines, string expected)
        {
            var prog = new ProjectGenerator.Program();
            var dir = @"c:\projects\GeneratedProject\GeneratedProject\";
            prog.Run(
                basePath: dir,
                outputNamespace: "ConfigProvider",
                dbSchema: "Conf",
                sourceNamespace: "ProjectGenerator.Tests");

            var method = GetTextChunk($"{dir}{path}", search, previousLines);
            Assert.That(method, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("Controllers\\UserSettingController.g.cs", "    /// Creates new UserSetting", -1, Resources.UserSettingControllerCreate)]
        [TestCase("Controllers\\UserSettingController.g.cs", "    /// Gets UserSetting", -1, Resources.UserSettingControllerGet)]
        public void Test_UserSettings(string path, string search, int previousLines, string expected)
        {
            var prog = new ProjectGenerator.Program();
            var dir = @"c:\projects\GeneratedProject\GeneratedProject\";
            prog.Run(
                basePath: dir,
                outputNamespace: "ConfigProvider",
                dbSchema: "Conf",
                sourceNamespace: "ProjectGenerator.Tests");

            var method = GetTextChunk($"{dir}{path}", search, previousLines);
            Assert.That(method, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("Controllers\\SimpleThingController.g.cs", "    /// Creates new SimpleThing", -1, Resources.SimpleThingControllerCreate)]
        [TestCase("Controllers\\SimpleThingController.g.cs", "    /// Gets SimpleThing", -1, Resources.SimpleThingControllerGet)]
        public void Test_SimpleThing(string path, string search, int previousLines, string expected)
        {
            var prog = new ProjectGenerator.Program();
            var dir = @"c:\projects\GeneratedProject\GeneratedProject\";
            prog.Run(
                basePath: dir,
                outputNamespace: "SimpleThing",
                dbSchema: "Conf",
                sourceNamespace: "ProjectGenerator.Tests");

            var method = GetTextChunk($"{dir}{path}", search, previousLines);
            Assert.That(method, Is.EqualTo(expected));
        }

        string GetTextChunk(string filename, string startLine, int addToStartIndex)
        {
            var lines = File.ReadAllLines(filename).ToList();
            var idx = lines.IndexOf(startLine) + addToStartIndex;
            var sb = new StringBuilder();
            var isIn = false;
            var depth = 0;
            while (!isIn || depth != 0)
            {
                var line = lines[idx++];
                sb.AppendLine(line);
                var opening = line.Count(e => e == '{');
                var closing = line.Count(e => e == '}');
                depth = depth + opening - closing;
                if (depth > 0 && !isIn) isIn = true;
            }
            var res = sb.ToString();
            return res.Substring(0, res.Length - 2);
        }
    }
}