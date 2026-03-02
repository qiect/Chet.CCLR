namespace AudioProcessorTool.Services;

public interface ISentenceSplitter
{
    List<string> SplitSentences(string text);
    List<string> SplitSentencesFromFile(string filePath);
}
