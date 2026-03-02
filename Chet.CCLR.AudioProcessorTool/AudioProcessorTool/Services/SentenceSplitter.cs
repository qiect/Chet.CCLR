using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AudioProcessorTool.Services;

public class SentenceSplitter : ISentenceSplitter
{
    public List<string> SplitSentences(string text)
    {
        var sentences = new List<string>();
        
        var lines = text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
        
        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            
            if (Regex.IsMatch(trimmedLine, @"^第 [一二三四五六七八九十\d]+章$"))
            {
                if (!string.IsNullOrEmpty(trimmedLine))
                {
                    sentences.Add(trimmedLine);
                }
                continue;
            }
            
            if (!string.IsNullOrEmpty(trimmedLine) && !Regex.IsMatch(trimmedLine, @"^第 [一二三四五六七八九十\d]+章$"))
            {
                var lineSentences = Regex.Split(trimmedLine, @"(?<=[。！？；])\s*")
                                         .Where(s => !string.IsNullOrWhiteSpace(s))
                                         .ToList();
                
                foreach (var sentence in lineSentences)
                {
                    var cleanSentence = sentence.Trim();
                    if (!string.IsNullOrEmpty(cleanSentence) && cleanSentence.Length > 1)
                    {
                        sentences.Add(cleanSentence);
                    }
                }
            }
        }
        
        var filteredSentences = new List<string>();
        foreach (var s in sentences)
        {
            if (!Regex.IsMatch(s, @"^第 [一二三四五六七八九十\d]+章$") || 
                (Regex.IsMatch(s, @"^第 [一二三四五六七八九十\d]+章$") && 
                 !string.IsNullOrEmpty(Regex.Replace(s, @"^第 [一二三四五六七八九十\d]+章", "").Trim())))
            {
                if (Regex.IsMatch(s, @"^第 [一二三四五六七八九十\d]+章 (.+)$"))
                {
                    var match = Regex.Match(s, @"^第 [一二三四五六七八九十\d]+章 (.+)$");
                    if (match.Success && !string.IsNullOrEmpty(match.Groups[1].Value))
                    {
                        filteredSentences.Add(match.Groups[0].Value.Substring(0, match.Groups[1].Index));
                        var remaining = match.Groups[1].Value.Trim();
                        if (!string.IsNullOrEmpty(remaining))
                        {
                            var remainingSentences = Regex.Split(remaining, @"(?<=[。！？；])\s*")
                                                          .Where(rs => !string.IsNullOrWhiteSpace(rs))
                                                          .Select(rs => rs.Trim());
                            foreach (var rs in remainingSentences)
                            {
                                if (!string.IsNullOrEmpty(rs) && rs.Length > 1)
                                {
                                    filteredSentences.Add(rs);
                                }
                            }
                        }
                    }
                }
                else
                {
                    filteredSentences.Add(s);
                }
            }
        }
        
        return filteredSentences;
    }
    
    public List<string> SplitSentencesFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"找不到文本文件：{filePath}", filePath);
        }
        
        var text = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
        return SplitSentences(text);
    }
}
