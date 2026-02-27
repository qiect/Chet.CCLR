using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace AudioProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("道德经音频拆解工具");
            
            // 获取源文件路径 - 使用绝对路径以确保能找到文件
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string sourceTextPath = @"E:\Project\Chet.CCLR\Chet.CCLR.Source\道德经\道德经_王弼本.txt";
            string sourceAudioPath = @"E:\Project\Chet.CCLR\Chet.CCLR.Source\道德经\道德经_全文朗读_王弼本.m4a";
            
            if (!File.Exists(sourceTextPath))
            {
                Console.WriteLine($"找不到文本文件: {sourceTextPath}");
                return;
            }
            
            if (!File.Exists(sourceAudioPath))
            {
                Console.WriteLine($"找不到音频文件: {sourceAudioPath}");
                return;
            }
            
            // 1. 自动分句
            Console.WriteLine("正在进行文本自动分句...");
            string textContent = File.ReadAllText(sourceTextPath, System.Text.Encoding.UTF8);
            var sentences = SplitSentences(textContent);
            
            Console.WriteLine($"共识别到 {sentences.Count} 个句子");
            
            // 创建输出目录
            string outputDir = "output";
            Directory.CreateDirectory(outputDir);
            
            // 保存分句结果
            string sentenceFilePath = Path.Combine(outputDir, "sentences.txt");
            File.WriteAllLines(sentenceFilePath, sentences, System.Text.Encoding.UTF8);
            Console.WriteLine($"分句结果已保存至: {sentenceFilePath}");
            
            // 提示用户进行标签制作
            Console.WriteLine("\n请按以下步骤操作:");
            Console.WriteLine($"1. 将音频文件 ({sourceAudioPath}) 导入 Audacity");
            Console.WriteLine("2. 为每个句子添加标签 (Ctrl+B)");
            Console.WriteLine("3. 导出标签文件为 labels.txt");
            Console.WriteLine("4. 将 labels.txt 放置在 output 目录下");
            Console.WriteLine("5. 运行此程序并选择 '2' 来切割音频");
            
            // 交互式菜单
            ShowMenu(sourceAudioPath, sentenceFilePath);
        }
        
        static void ShowMenu(string audioPath, string sentencePath)
        {
            while (true)
            {
                Console.WriteLine("\n请选择操作:");
                Console.WriteLine("1. 查看分句结果");
                Console.WriteLine("2. 执行音频切割 (需要 labels.txt 文件)");
                Console.WriteLine("3. 退出");
                
                var input = Console.ReadLine();
                
                switch (input)
                {
                    case "1":
                        ViewSentences(sentencePath);
                        break;
                    case "2":
                        ProcessAudioCutting(audioPath);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("无效选项，请重新选择");
                        break;
                }
            }
        }
        
        static void ViewSentences(string sentencePath)
        {
            if (!File.Exists(sentencePath))
            {
                Console.WriteLine("句子文件不存在");
                return;
            }
            
            var sentences = File.ReadAllLines(sentencePath);
            Console.WriteLine($"\n句子列表 (共 {sentences.Length} 个):");
            for (int i = 0; i < Math.Min(sentences.Length, 20); i++) // 只显示前20个
            {
                Console.WriteLine($"{i + 1:D3}. {sentences[i]}");
            }
            
            if (sentences.Length > 20)
            {
                Console.WriteLine($"... 还有 {sentences.Length - 20} 个句子");
            }
        }
        
        static void ProcessAudioCutting(string sourceAudioPath)
        {
            string labelPath = Path.Combine("output", "labels.txt");
            if (!File.Exists(labelPath))
            {
                Console.WriteLine($"找不到标签文件: {labelPath}");
                Console.WriteLine("请先按说明在 Audacity 中制作标签文件");
                return;
            }
            
            string audioOutputDir = Path.Combine("output", "audio");
            Directory.CreateDirectory(audioOutputDir);
            
            Console.WriteLine("开始切割音频...");
            
            var lines = File.ReadAllLines(labelPath);
            int processedCount = 0;
            
            // 检查FFmpeg是否可用
            bool ffmpegAvailable = IsFFmpegAvailable();
            if (!ffmpegAvailable)
            {
                Console.WriteLine("警告: 未检测到FFmpeg，将创建模拟音频文件用于演示。");
                Console.WriteLine("要进行实际音频切割，请安装FFmpeg并将其添加到系统PATH中。");
            }
            
            foreach (var line in lines)
            {
                var parts = line.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 3)
                {
                    if (double.TryParse(parts[0], out double start) && 
                        double.TryParse(parts[1], out double end))
                    {
                        string name = parts[2];
                        
                        // 确保名称符合文件命名规则
                        name = Regex.Replace(name, @"[<>:""/\\|?*]", "_");
                        
                        double duration = end - start;
                        
                        string outputFile = Path.Combine(audioOutputDir, $"{name}.m4a");
                        
                        bool success;
                        if (ffmpegAvailable)
                        {
                            success = CutAudioWithFFmpeg(sourceAudioPath, outputFile, start, duration);
                        }
                        else
                        {
                            // 创建模拟音频文件用于演示
                            success = CreateDemoAudioFile(outputFile, duration);
                        }
                        
                        if (success)
                        {
                            processedCount++;
                            Console.WriteLine($"已生成: {outputFile}");
                        }
                        else
                        {
                            Console.WriteLine($"失败: {outputFile}");
                        }
                    }
                }
            }
            
            Console.WriteLine($"音频切割完成! 成功处理 {processedCount} 个音频片段");
            
            // 生成数据库插入脚本
            GenerateDatabaseScript(lines);
        }
        
        static void GenerateDatabaseScript(string[] labelLines)
        {
            string scriptPath = Path.Combine("output", "insert_script.sql");
            
            var sqlLines = new List<string>();
            sqlLines.Add("-- 道德经句子数据插入脚本");
            sqlLines.Add("-- 生成时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sqlLines.Add("");
            
            // 添加UUID生成函数（如果数据库支持）
            sqlLines.Add("-- 注意: 请确保数据库中存在生成UUID的函数，或替换为具体的ID值");
            sqlLines.Add("");
            
            // 读取句子内容
            string[] sentences = Array.Empty<string>();
            string sentenceFilePath = Path.Combine("output", "sentences.txt");
            if (File.Exists(sentenceFilePath))
            {
                sentences = File.ReadAllLines(sentenceFilePath);
            }
            
            int index = 1;
            for (int i = 0; i < labelLines.Length; i++)
            {
                var line = labelLines[i];
                var parts = line.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 3)
                {
                    string name = parts[2];
                    // 确保名称符合文件命名规则
                    name = Regex.Replace(name, @"[<>:""/\\|?*]", "_");
                    
                    // 获取对应的句子内容（如果有）
                    string sentenceContent = "";
                    if (sentences != null && i < sentences.Length)
                    {
                        sentenceContent = sentences[i].Replace("'", "''"); // 转义单引号
                    }
                    
                    // 根据实际数据库表结构生成SQL
                    // classic_sentence 表结构：
                    // id, chapter_id, content, audio_url, order_index, is_published
                    string sql = $"INSERT INTO classic_sentence (id, chapter_id, content, audio_url, order_index, is_published) " +
                                $"VALUES (CONCAT('SENT', LPAD({i+1}, 8, '0')), 'CHAPTER001', '{sentenceContent}', '/audio/道德经/{name}.mp3', {i+1}, 1);";
                    sqlLines.Add(sql);
                    index++;
                }
            }
            
            File.WriteAllLines(scriptPath, sqlLines);
            Console.WriteLine($"数据库插入脚本已生成: {scriptPath}");
            
            // 同时生成一个说明文件
            string readmePath = Path.Combine("output", "README.md");
            var readmeContent = new List<string>();
            readmeContent.Add("# 道德经音频拆分结果");
            readmeContent.Add("");
            readmeContent.Add("- 音频总数：" + labelLines.Length);
            readmeContent.Add("- 生成时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            readmeContent.Add("- 音频存储路径：`/audio/道德经/`");
            readmeContent.Add("- 需要手动指定 `chapter_id` 以匹配实际章节");
            readmeContent.Add("");
            readmeContent.Add("## 使用说明");
            readmeContent.Add("1. 修改 `insert_script.sql` 中的 `chapter_id` 以匹配实际章节");
            readmeContent.Add("2. 执行SQL脚本前请备份数据库");
            readmeContent.Add("3. 确认音频文件已放置在正确路径");
            
            File.WriteAllLines(readmePath, readmeContent);
            Console.WriteLine($"说明文档已生成: {readmePath}");
        }
        
        public static List<string> SplitSentences(string text)
        {
            var sentences = new List<string>();
            
            // 按行分割文本
            var lines = text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
            
            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                
                // 检查是否是章节标题（如"第一章"）
                if (Regex.IsMatch(trimmedLine, @"^第[一二三四五六七八九十\d]+章.*$"))
                {
                    if (!string.IsNullOrEmpty(trimmedLine))
                    {
                        sentences.Add(trimmedLine);
                    }
                    continue;
                }
                
                // 对正文内容进行分句
                if (!string.IsNullOrEmpty(trimmedLine) && !Regex.IsMatch(trimmedLine, @"^第[一二三四五六七八九十\d]+章.*$"))
                {
                    // 按照中文标点符号分句，但保留标点符号
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
            
            // 过滤掉仅包含章节标题的项，只保留真正的句子
            var filteredSentences = new List<string>();
            foreach (var s in sentences)
            {
                if (!Regex.IsMatch(s, @"^第[一二三四五六七八九十\d]+章.*$") || 
                    (Regex.IsMatch(s, @"^第[一二三四五六七八九十\d]+章.*$") && 
                     !string.IsNullOrEmpty(Regex.Replace(s, @"^第[一二三四五六七八九十\d]+章", "").Trim())))
                {
                    // 如果是章节标题但后面跟着内容，将它们分开
                    if (Regex.IsMatch(s, @"^第[一二三四五六七八九十\d]+章(.+)$"))
                    {
                        var match = Regex.Match(s, @"^第[一二三四五六七八九十\d]+章(.+)$");
                        if (match.Success && !string.IsNullOrEmpty(match.Groups[1].Value))
                        {
                            filteredSentences.Add(match.Groups[0].Value.Substring(0, match.Groups[1].Index)); // 章节标题
                            // 再次对剩余部分进行分句
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
        
        public static bool CutAudioWithFFmpeg(string sourceFile, string outputFile, double start, double duration)
        {
            try
            {
                // 检查FFmpeg是否可用
                if (!IsFFmpegAvailable())
                {
                    Console.WriteLine("错误: 系统中未找到FFmpeg。请安装FFmpeg并将其添加到系统PATH环境变量中。");
                    return false;
                }
                
                // 确保输出目录存在
                var outputDir = Path.GetDirectoryName(outputFile);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
                
                // 构建FFmpeg参数 - 根据输入文件格式决定输出格式
                string args;
                string extension = Path.GetExtension(outputFile).ToLower();
                
                if (extension == ".m4a" || extension == ".mp4")
                {
                    // 对于M4A/M4P文件，保持原格式
                    args = $"-i \"{sourceFile}\" -ss {start:F3} -t {duration:F3} -c copy -avoid_negative_ts make_zero -y \"{outputFile}\"";
                }
                else
                {
                    // 默认转换为MP3格式
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
                process.WaitForExit(60000); // 60秒超时
                
                if (process.ExitCode != 0)
                {
                    Console.WriteLine($"FFmpeg执行失败: {error}");
                    return false;
                }
                
                // 检查输出文件是否成功创建
                if (!File.Exists(outputFile))
                {
                    Console.WriteLine($"输出文件未创建: {outputFile}");
                    return false;
                }
                
                // 检查输出文件大小是否合理（大于1KB）
                var fileInfo = new FileInfo(outputFile);
                if (fileInfo.Length < 1024)
                {
                    Console.WriteLine($"输出文件大小异常: {outputFile} (大小: {fileInfo.Length} bytes)");
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"切割音频时发生错误: {ex.Message}");
                return false;
            }
        }
        
        public static bool IsFFmpegAvailable()
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
                 process.WaitForExit(5000); // 5秒超时
                 return process.ExitCode == 0;
             }
             catch
             {
                 return false;
             }
         }
         
         public static bool CreateDemoAudioFile(string outputFile, double duration)
         {
             try
             {
                 // 创建一个简单的文本文件作为模拟音频文件
                 // 在实际场景中，这里会是真实的音频文件
                 var demoContent = $"// 模拟音频文件\n// 时长: {duration:F2} 秒\n// 生成时间: {DateTime.Now}\n// 这是一个演示文件，实际使用时会被FFmpeg生成的真实音频文件替换";
                 File.WriteAllText(outputFile, demoContent);
                 return true;
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"创建演示音频文件时发生错误: {ex.Message}");
                 return false;
             }
         }
    }
}
