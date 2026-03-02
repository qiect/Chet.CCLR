using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace AudioProcessorTool.Services;

public class AudioCutter : IAudioCutter
{
    public bool IsFFmpegAvailable()
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = "-version",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            
            process.Start();
            process.WaitForExit(5000);
            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }
    
    public bool CutAudio(string sourceFile, string outputFile, double start, double duration)
    {
        try
        {
            if (!IsFFmpegAvailable())
            {
                return false;
            }
            
            var outputDir = Path.GetDirectoryName(outputFile);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
            
            string args;
            string extension = Path.GetExtension(outputFile).ToLower();
            
            if (extension == ".m4a" || extension == ".mp4")
            {
                args = $"-i \"{sourceFile}\" -ss {start:F3} -t {duration:F3} -c copy -avoid_negative_ts make_zero -y \"{outputFile}\"";
            }
            else
            {
                args = $"-i \"{sourceFile}\" -ss {start:F3} -t {duration:F3} -c:a libmp3lame -b:a 128k -y \"{outputFile}\"";
            }
            
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit(60000);
            
            if (process.ExitCode != 0)
            {
                return false;
            }
            
            if (!File.Exists(outputFile))
            {
                return false;
            }
            
            var fileInfo = new FileInfo(outputFile);
            if (fileInfo.Length < 1024)
            {
                return false;
            }
            
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public List<string> CutAllAudios(string sourceAudioPath, string[] labelLines, string outputDir)
    {
        var results = new List<string>();
        Directory.CreateDirectory(outputDir);
        
        foreach (var line in labelLines)
        {
            var parts = line.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3)
            {
                if (double.TryParse(parts[0], out double start) && 
                    double.TryParse(parts[1], out double end))
                {
                    string name = parts[2];
                    name = Regex.Replace(name, @"[<>:""/\\|?*]", "_");
                    
                    double duration = end - start;
                    string outputFile = Path.Combine(outputDir, $"{name}.m4a");
                    
                    bool success = CutAudio(sourceAudioPath, outputFile, start, duration);
                    
                    if (success)
                    {
                        results.Add($"成功: {outputFile}");
                    }
                    else
                    {
                        results.Add($"失败: {outputFile}");
                    }
                }
            }
        }
        
        return results;
    }
}
