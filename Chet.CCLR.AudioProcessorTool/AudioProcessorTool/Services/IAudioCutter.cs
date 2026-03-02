namespace AudioProcessorTool.Services;

public interface IAudioCutter
{
    bool IsFFmpegAvailable();
    bool CutAudio(string sourceFile, string outputFile, double start, double duration);
    List<string> CutAllAudios(string sourceAudioPath, string[] labelLines, string outputDir);
}
