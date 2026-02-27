using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using NAudio.Wave;

namespace AudioTagger;

public class TagItem
{
    public int Index { get; set; }
    public double StartTime { get; set; }
    public double EndTime { get; set; }
    public double Duration => EndTime - StartTime;
    public string Content { get; set; } = string.Empty;
}

public partial class MainWindow : Window
{
    private readonly ObservableCollection<TagItem> _tags = new();
    private readonly List<string> _sentences = new();
    private string? _audioFilePath;
    private string? _sentenceFilePath;
    private int _currentSentenceIndex = 0;
    private WaveOutEvent? _waveOut;
    private AudioFileReader? _audioFile;
    private System.Windows.Threading.DispatcherTimer? _timer;
    private double _currentPosition = 0;
    private double _totalDuration = 0;

    public ObservableCollection<TagItem> Tags => _tags;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        InitializeUI();
    }

    private void InitializeUI()
    {
        LvTags.ItemsSource = _tags;
        TxtStatus.Text = "就绪 - 请选择音频和句子文件";
    }

    private void BtnSelectAudio_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "音频文件|*.mp3;*.wav;*.m4a;*.aac;*.flac|所有文件|*.*",
            Title = "选择音频文件"
        };

        if (dialog.ShowDialog() == true)
        {
            _audioFilePath = dialog.FileName;
            TxtAudioPath.Text = _audioFilePath;
            TxtStatus.Text = $"已选择音频: {_audioFilePath}";
        }
    }

    private void BtnSelectSentences_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "文本文件|*.txt|所有文件|*.*",
            Title = "选择句子文件"
        };

        if (dialog.ShowDialog() == true)
        {
            _sentenceFilePath = dialog.FileName;
            TxtSentencePath.Text = _sentenceFilePath;
            
            // 读取句子文件
            _sentences.Clear();
            var lines = File.ReadAllLines(_sentenceFilePath, Encoding.UTF8);
            _sentences.AddRange(lines);
            
            TxtStatus.Text = $"已加载 {_sentences.Count} 个句子";
        }
    }

    private void BtnLoad_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_audioFilePath))
        {
            MessageBox.Show("请先选择音频文件", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (string.IsNullOrEmpty(_sentenceFilePath))
        {
            MessageBox.Show("请先选择句子文件", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        try
        {
            // 释放之前的音频资源
            _waveOut?.Stop();
            _waveOut?.Dispose();
            _audioFile?.Dispose();
            
            // 初始化音频播放器
            _audioFile = new AudioFileReader(_audioFilePath);
            _totalDuration = _audioFile.TotalTime.TotalSeconds;
            
            _waveOut = new WaveOutEvent();
            _waveOut.Init(_audioFile);
            
            // 初始化当前标签索引
            _currentSentenceIndex = 0;
            
            // 更新UI
            TxtTotalTime.Text = $"总时长: {_totalDuration:F3} 秒";
            
            TxtStatus.Text = "音频和句子已加载完成";
            
            // 显示第一个句子
            if (_sentences.Count > 0)
            {
                DisplayCurrentSentence();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"加载失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            TxtStatus.Text = "加载失败";
        }
    }

    private void BtnPlayPause_Click(object sender, RoutedEventArgs e)
    {
        if (_waveOut == null)
        {
            MessageBox.Show("请先加载音频文件", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        if (_waveOut.PlaybackState == PlaybackState.Playing)
        {
            _waveOut.Pause();
            ((Button)sender).Content = "播放";
            TxtStatus.Text = "已暂停";
        }
        else
        {
            _waveOut.Play();
            ((Button)sender).Content = "暂停";
            TxtStatus.Text = "正在播放...";
            
            // 启动定时器更新进度
            StartTimer();
        }
    }

    private void BtnStop_Click(object sender, RoutedEventArgs e)
    {
        if (_waveOut != null)
        {
            _waveOut.Stop();
            _timer?.Stop();
            _currentPosition = 0;
            TxtCurrentTime.Text = "当前时间: 0.000 秒";
            SliTimeline.Value = 0;
            TxtStatus.Text = "已停止";
        }
    }

    private void BtnAddTag_Click(object sender, RoutedEventArgs e)
    {
        if (_currentSentenceIndex >= _sentences.Count)
        {
            MessageBox.Show("已到达句子列表末尾", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var currentTime = _currentPosition;
        
        // 如果是第一个标签，从0开始
        double startTime = _tags.Count > 0 ? _tags.Last().EndTime : 0;
        
        // 创建新标签
        var newTag = new TagItem
        {
            Index = _tags.Count + 1,
            StartTime = startTime,
            EndTime = currentTime,
            Content = _sentences[_currentSentenceIndex]
        };

        _tags.Add(newTag);
        
        // 更新UI
        LvTags.ScrollIntoView(newTag);
        TxtStatus.Text = $"已添加标签 {_tags.Count}: {_sentences[_currentSentenceIndex]}";
        
        // 移动到下一个句子
        _currentSentenceIndex++;
        DisplayCurrentSentence();
    }

    private void BtnEditTag_Click(object sender, RoutedEventArgs e)
    {
        var selectedTag = LvTags.SelectedItem as TagItem;
        if (selectedTag == null)
        {
            MessageBox.Show("请先选择一个标签", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        // 打开编辑窗口（简化处理，直接在主界面编辑）
        TxtEditSentence.Text = selectedTag.Content;
        TxtStatus.Text = "请输入新的句子内容，然后点击'更新句子内容'";
    }

    private void BtnDeleteTag_Click(object sender, RoutedEventArgs e)
    {
        var selectedTag = LvTags.SelectedItem as TagItem;
        if (selectedTag == null)
        {
            MessageBox.Show("请先选择一个标签", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show($"确定要删除标签 '{selectedTag.Content}' 吗？", "确认删除", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        
        if (result == MessageBoxResult.Yes)
        {
            _tags.Remove(selectedTag);
            
            // 重新编号
            for (int i = 0; i < _tags.Count; i++)
            {
                _tags[i].Index = i + 1;
            }
            
            TxtStatus.Text = "标签已删除";
        }
    }

    private void BtnExport_Click(object sender, RoutedEventArgs e)
    {
        if (_tags.Count == 0)
        {
            MessageBox.Show("没有可导出的标签", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var dialog = new SaveFileDialog
        {
            Filter = "标签文件|*.txt|所有文件|*.*",
            FileName = "labels.txt",
            Title = "导出标签文件"
        };

        if (dialog.ShowDialog() == true)
        {
            try
            {
                var lines = _tags.Select(t => $"{t.StartTime:F6}\t{t.EndTime:F6}\t{t.Index:D3}").ToArray();
                File.WriteAllLines(dialog.FileName, lines, Encoding.UTF8);
                
                TxtStatus.Text = $"已导出 {_tags.Count} 个标签到: {dialog.FileName}";
                MessageBox.Show($"成功导出 {_tags.Count} 个标签", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                TxtStatus.Text = "导出失败";
            }
        }
    }

    private void BtnSaveSentences_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_sentenceFilePath))
        {
            MessageBox.Show("请先加载句子文件", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        try
        {
            File.WriteAllLines(_sentenceFilePath, _sentences, Encoding.UTF8);
            TxtStatus.Text = $"句子文件已保存: {_sentenceFilePath}";
            MessageBox.Show("句子文件已保存", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"保存失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            TxtStatus.Text = "保存失败";
        }
    }

    private void BtnUpdateSentence_Click(object sender, RoutedEventArgs e)
    {
        var selectedTag = LvTags.SelectedItem as TagItem;
        if (selectedTag == null)
        {
            MessageBox.Show("请先选择一个标签", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        if (!string.IsNullOrEmpty(TxtEditSentence.Text))
        {
            selectedTag.Content = TxtEditSentence.Text;
            
            // 同时更新句子列表
            if (_currentSentenceIndex > 0 && _currentSentenceIndex <= _sentences.Count)
            {
                _sentences[_currentSentenceIndex - 1] = TxtEditSentence.Text;
            }
            
            TxtSelectedSentence.Text = TxtEditSentence.Text;
            TxtStatus.Text = "句子内容已更新";
            
            // 清空编辑框
            TxtEditSentence.Text = string.Empty;
        }
    }

    private void LvTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedTag = LvTags.SelectedItem as TagItem;
        if (selectedTag != null)
        {
            TxtSelectedSentence.Text = selectedTag.Content;
            TxtEditSentence.Text = selectedTag.Content;
            
            // 跳转到标签位置
            if (_totalDuration > 0)
            {
                SliTimeline.Value = (selectedTag.StartTime / _totalDuration) * 100;
            }
            
            // 播放该标签对应的音频片段
            if (_waveOut != null)
            {
                // 简化处理，实际可能需要更精确的跳转
                TxtStatus.Text = $"跳转到标签 {selectedTag.Index}: {selectedTag.StartTime:F3} 秒";
            }
        }
    }

    private void SliTimeline_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_totalDuration > 0)
        {
            _currentPosition = (SliTimeline.Value / 100) * _totalDuration;
            TxtCurrentTime.Text = $"当前时间: {_currentPosition:F3} 秒";
        }
    }

    private void SliTimeline_PreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (_waveOut != null && _totalDuration > 0)
        {
            var targetPosition = (SliTimeline.Value / 100) * _totalDuration;
            // 简化处理，实际可能需要调用音频库的跳转功能
            TxtStatus.Text = $"跳转到: {targetPosition:F3} 秒";
        }
    }

    private void DisplayCurrentSentence()
    {
        if (_currentSentenceIndex < _sentences.Count)
        {
            TxtSelectedSentence.Text = _sentences[_currentSentenceIndex];
        }
    }

    private void StartTimer()
    {
        _timer?.Stop();
        _timer = new System.Windows.Threading.DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(100);
        _timer.Tick += (s, e) =>
        {
            // 更新当前时间显示
            if (_audioFile != null)
            {
                _currentPosition = _audioFile.CurrentTime.TotalSeconds;
            }
            
            if (_currentPosition > _totalDuration)
            {
                _currentPosition = 0;
                _timer?.Stop();
                BtnPlayPause.Content = "播放";
                TxtStatus.Text = "播放完成";
            }
            
            TxtCurrentTime.Text = $"当前时间: {_currentPosition:F3} 秒";
            
            // 更新进度条
            if (_totalDuration > 0)
            {
                SliTimeline.Value = (_currentPosition / _totalDuration) * 100;
            }
        };
        _timer.Start();
    }

    protected override void OnClosed(EventArgs e)
    {
        _timer?.Stop();
        _waveOut?.Stop();
        _waveOut?.Dispose();
        _audioFile?.Dispose();
        base.OnClosed(e);
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Space && _waveOut != null)
        {
            // 空格键切换播放/暂停
            if (_waveOut.PlaybackState == PlaybackState.Playing)
            {
                _waveOut.Pause();
                BtnPlayPause.Content = "播放";
                TxtStatus.Text = "已暂停";
            }
            else
            {
                _waveOut.Play();
                BtnPlayPause.Content = "暂停";
                TxtStatus.Text = "正在播放...";
                StartTimer();
            }
        }
        else if (e.Key == Key.Add || e.Key == Key.OemPlus)
        {
            // +键添加标签
            if (_waveOut?.PlaybackState == PlaybackState.Playing)
            {
                BtnAddTag_Click(null, null);
            }
        }
    }
}