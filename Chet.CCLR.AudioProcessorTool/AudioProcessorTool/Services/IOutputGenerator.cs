namespace AudioProcessorTool.Services;

public interface IOutputGenerator
{
    void GenerateDatabaseScript(string[] labelLines, string[] sentences, string outputDir);
    void GenerateReadme(string[] labelLines, string outputDir);
}
