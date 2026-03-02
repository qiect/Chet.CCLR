using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AudioProcessorTool.Services;

public class OutputGenerator : IOutputGenerator
{
    public void GenerateDatabaseScript(string[] labelLines, string[] sentences, string outputDir)
    {
        string scriptPath = Path.Combine(outputDir, "insert_script.sql");
        
        var sqlLines = new List<string>();
        sqlLines.Add("-- 道德经句子数据插入脚本");
        sqlLines.Add("-- 生成时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        sqlLines.Add("");
        sqlLines.Add("-- 注意：请确保数据库中存在生成 UUID 的函数，或替换为具体的 ID");
        sqlLines.Add("");
        
        int index = 1;
        for (int i = 0; i < labelLines.Length; i++)
        {
            var line = labelLines[i];
            var parts = line.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3)
            {
                string name = parts[2];
                name = Regex.Replace(name, @"[<>:""/\\|?*]", "_");
                
                string sentenceContent = "";
                if (sentences != null && i < sentences.Length)
                {
                    sentenceContent = sentences[i].Replace("'", "''");
                }
                
                string sql = $"INSERT INTO classic_sentence (id, chapter_id, content, audio_url, order_index, is_published) " +
                            $"VALUES (CONCAT('SENT', LPAD({i+1}, 8, '0')), 'CHAPTER001', '{sentenceContent}', '/audio/道德经/{name}.mp3', {i+1}, 1);";
                sqlLines.Add(sql);
                index++;
            }
        }
        
        File.WriteAllLines(scriptPath, sqlLines);
    }
    
    public void GenerateReadme(string[] labelLines, string outputDir)
    {
        string readmePath = Path.Combine(outputDir, "README.md");
        var readmeContent = new List<string>();
        readmeContent.Add("# 道德经音频拆分结果");
        readmeContent.Add("");
        readmeContent.Add($"- 音频总数：{labelLines.Length}");
        readmeContent.Add($"- 生成时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        readmeContent.Add("- 音频存储路径：`/audio/道德经`");
        readmeContent.Add("- 需要手动指定 `chapter_id` 以匹配实际章节");
        readmeContent.Add("");
        readmeContent.Add("## 使用说明");
        readmeContent.Add("1. 修改 `insert_script.sql` 中的 `chapter_id` 以匹配实际章节");
        readmeContent.Add("2. 执行 SQL 脚本前请备份数据");
        readmeContent.Add("3. 确认音频文件已放置在正确路径");
        
        File.WriteAllLines(readmePath, readmeContent);
    }
}
