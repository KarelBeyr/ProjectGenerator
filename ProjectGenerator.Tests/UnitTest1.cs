using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Text;

namespace ProjectGenerator.Tests
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var prog = new ProjectGenerator.Program();
            var dir = @"c:\projects\GeneratedProject\GeneratedProject\";
            prog.Run(
                basePath: dir,
                outputNamespace: "ConfigProvider",
                dbSchema: "Conf",
                sourceNamespace: "ProjectGenerator.Tests");

            var method = GetTextChunk($"{dir}Controllers\\ConfigurationController.g.cs", "    /// Creates new Configuration", -1);
            var expected = Resources.ConfigurationControllerCreate;
            Assert.That(method, Is.EqualTo(expected));

            method = GetTextChunk($"{dir}Controllers\\ConfigurationController.g.cs", "    [HttpGet(\"{Key}\")]", -8);
            expected = Resources.ConfigurationControllerGet;
            Assert.That(method, Is.EqualTo(expected));

            method = GetTextChunk($"{dir}Controllers\\ConfigurationController.g.cs", "    /// Updates Configuration", -1);
            expected = Resources.ConfigurationControllerUpdate;
            Assert.That(method, Is.EqualTo(expected));

            method = GetTextChunk($"{dir}Controllers\\ConfigurationController.g.cs", "    /// Deletes Configuration", -1);
            expected = Resources.ConfigurationControllerDelete;
            Assert.That(method, Is.EqualTo(expected));

            method = GetTextChunk($"{dir}Services\\ConfigurationService.g.cs", @"    async Task<ConfigurationModel> IConfigurationService.Get(string key)", 0);
            expected = Resources.ConfigurationServiceGet;
            Assert.That(method, Is.EqualTo(expected));

            method = GetTextChunk($"{dir}Controllers\\Models.g.cs", @"public partial class UserSettingModel", 0);
            expected = Resources.Models_UserSettingModel;
            Assert.That(method, Is.EqualTo(expected));
        }

        [Test]
        public void Test2()
        {
            var prog = new ProjectGenerator.Program();
            var dir = @"c:\projects\GeneratedProject\GeneratedProject\";
            prog.Run(
                basePath: dir,
                outputNamespace: "ConfigProvider",
                dbSchema: "Conf",
                sourceNamespace: "ProjectGenerator.Tests");

            var method = GetTextChunk($"{dir}Controllers\\UserSettingController.g.cs", "    /// Creates new UserSetting", -1);
            var expected = Resources.UserSettingControllerCreate;
            Assert.That(method, Is.EqualTo(expected));

            method = GetTextChunk($"{dir}Controllers\\UserSettingController.g.cs", "    /// Gets UserSetting", -1);
            expected = Resources.UserSettingControllerGet;
            Assert.That(method, Is.EqualTo(expected));
        }

        [Test]
        public void Test3()
        {
            var prog = new ProjectGenerator.Program();
            var dir = @"c:\projects\GeneratedProject\GeneratedProject\";
            prog.Run(
                basePath: dir,
                outputNamespace: "SimpleThing",
                dbSchema: "Conf",
                sourceNamespace: "ProjectGenerator.Tests");

            var method = GetTextChunk($"{dir}Controllers\\SimpleThingController.g.cs", "    /// Creates new SimpleThing", -1);
            var expected = Resources.SimpleThingControllerCreate;
            Assert.That(method, Is.EqualTo(expected));

            method = GetTextChunk($"{dir}Controllers\\SimpleThingController.g.cs", "    /// Gets SimpleThing", -1);
            expected = Resources.SimpleThingControllerGet;
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