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
            Assert.AreEqual(expected, method);
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
            Assert.AreEqual(expected, method);
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
                if (opening > 0 && !isIn) isIn = true;
            }
            return sb.ToString();
        }
    }
}