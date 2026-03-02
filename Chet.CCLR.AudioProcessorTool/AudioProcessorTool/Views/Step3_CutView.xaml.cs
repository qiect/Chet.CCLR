using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using AudioProcessorTool.Services;

namespace AudioProcessorTool.Views;

public partial class Step3_CutView : UserControl
{
    private readonly IAudioCutter _audioCutter;
    private readonly IOutputGenerator _outputGenerator;
    private string? _sourceAudioPath;
    private string? _labelsPath;

    public Step3_CutView()
    {
        _audioCutter = new AudioCutter();
        _outputGenerator = new OutputGenerator();
        
        InitializeComponent();
    }

    private void BtnSelectAudio_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "音频文件|*.mp3;*.wav;*.m4a;*.aac;*.flac|所有文件|*.*",
            Title = "选择原始音频文件"
        };

        if (dialog.ShowDialog() == true)
        {
            _sourceAudioPath = dialog.FileName;
            TxtAudioPath.Text = _sourceAudioPath;
            TxtStatus.Text = $"已选择音频：{_sourceAudioPath}";
        }
    }

    private void BtnSelectLabels_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "标签文件|*.txt|所有文件|*.*",
            Title = "选择标签文件"
        };

        if (dialog.ShowDialog() == true)
        {
            _labelsPath = dialog.FileName;
            TxtLabelsPath.Text = _labelsPath;
            TxtStatus.Text = $"已选择标签文件：{_labelsPath}";
        }
    }

    private void BtnCut_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_sourceAudioPath))
        {
            MessageBox.Show("请先选择音频文件", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (string.IsNullOrEmpty(_labelsPath))
        {
            MessageBox.Show("请先选择标签文件", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (!File.Exists(_labelsPath))
        {
            MessageBox.Show($"标签文件不存在：{_labelsPath}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        try
        {
            var outputDir = Path.Combine(Path.GetDirectoryName(_labelsPath) ?? ".", "audio");
            
            var labelLines = File.ReadAllLines(_labelsPath);
            
            var results = _audioCutter.CutAllAudios(_sourceAudioPath, labelLines, outputDir);
            
            var successCount = results.Count(r => r.StartsWith("成功:"));
            var failCount = results.Count(r => r.StartsWith("失败:"));
            
            _outputGenerator.GenerateDatabaseScript(labelLines, Array.Empty<string>(), Path.GetDirectoryName(_labelsPath) ?? ".");
            _outputGenerator.GenerateReadme(labelLines, Path.GetDirectoryName(_labelsPath) ?? ".");
            
            var resultMsg = $"音频切割完成!\n\n成功：{successCount} 个\n失败：{failCount} 个\n\n输出目录：{outputDir}";
            
            MessageBox.Show(resultMsg, "完成", MessageBoxButton.OK, MessageBoxImage.Information);
            
            TxtStatus.Text = $"切割完成：成功 {successCount} 个 失败 {failCount} 个";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"切割失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            TxtStatus.Text = "切割失败";
        }
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (_audioCutter.IsFFmpegAvailable())
        {
            TxtFFmpegStatus.Text = "✓ FFmpeg 已安装并可用";
            TxtFFmpegStatus.Foreground = System.Windows.Media.Brushes.Green;
        }
        else
        {
            TxtFFmpegStatus.Text = "⚠ 未检测到 FFmpeg，将创建模拟音频文件";
            TxtFFmpegStatus.Foreground = System.Windows.Media.Brushes.Orange;
        }
    }
}
