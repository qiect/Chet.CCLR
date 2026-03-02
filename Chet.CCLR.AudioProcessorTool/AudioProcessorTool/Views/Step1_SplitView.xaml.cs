using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using AudioProcessorTool.Services;

namespace AudioProcessorTool.Views;

public partial class Step1_SplitView : UserControl
{
    private readonly ISentenceSplitter _sentenceSplitter;
    private ObservableCollection<string> _sentences;
    private string? _textFilePath;

    public ObservableCollection<string> Sentences => _sentences;

    public Step1_SplitView()
    {
        _sentenceSplitter = new SentenceSplitter();
        _sentences = new ObservableCollection<string>();
        
        InitializeComponent();
    }

    private void BtnSelectFile_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "文本文件|*.txt|所有文件|*.*",
            Title = "选择文本文件"
        };

        if (dialog.ShowDialog() == true)
        {
            _textFilePath = dialog.FileName;
            TxtFilePath.Text = _textFilePath;
            TxtStatus.Text = $"已选择文件：{_textFilePath}";
        }
    }

    private void BtnSplit_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_textFilePath))
        {
            MessageBox.Show("请先选择文本文件", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        try
        {
            var sentences = _sentenceSplitter.SplitSentencesFromFile(_textFilePath);
            
            _sentences.Clear();
            foreach (var sentence in sentences)
            {
                _sentences.Add(sentence);
            }
            
            TxtStatus.Text = $"成功分句：{_sentences.Count} 个句子";
            MessageBox.Show($"成功分句：{_sentences.Count} 个句子", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"分句失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            TxtStatus.Text = "分句失败";
        }
    }

    private void BtnExport_Click(object sender, RoutedEventArgs e)
    {
        if (_sentences.Count == 0)
        {
            MessageBox.Show("没有可导出的句子", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var dialog = new SaveFileDialog
        {
            Filter = "文本文件|*.txt|所有文件|*.*",
            FileName = "sentences.txt",
            Title = "导出句子文件"
        };

        if (dialog.ShowDialog() == true)
        {
            try
            {
                File.WriteAllLines(dialog.FileName, _sentences);
                TxtStatus.Text = $"已导出 {_sentences.Count} 个句子到：{dialog.FileName}";
                MessageBox.Show($"成功导出 {_sentences.Count} 个句子", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                TxtStatus.Text = "导出失败";
            }
        }
    }

    private void BtnPreview_Click(object sender, RoutedEventArgs e)
    {
        if (_sentences.Count == 0)
        {
            MessageBox.Show("没有可预览的句子", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var previewWindow = new Window
        {
            Title = "预览句子",
            Width = 600,
            Height = 400,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        var textBox = new TextBox
        {
            Text = string.Join("\n", _sentences),
            TextWrapping = TextWrapping.Wrap,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            Margin = new Thickness(10),
            FontSize = 14,
            FontFamily = new System.Windows.Media.FontFamily("Microsoft YaHei")
        };

        previewWindow.Content = textBox;
        previewWindow.Owner = Application.Current.MainWindow;
        previewWindow.ShowDialog();
    }
}
