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

namespace AudioProcessorTool.Views;

public class TagItem
{
    public int Index { get; set; }
    public double StartTime { get; set; }
    public double EndTime { get; set; }
    public double Duration => EndTime - StartTime;
    public string Content { get; set; } = string.Empty;
}

public partial class Step2_TagView : UserControl
{
    private readonly ObservableCollection<TagItem> _tags = new();
    private string? _audioFilePath;
    private WaveOutEvent? _waveOut;
    private AudioFileReader? _audioFile;
    private System.Windows.Threading.DispatcherTimer? _timer;
    private double _currentPosition = 0;
    private double _totalDuration = 0;

    public ObservableCollection<TagItem> Tags => _tags;

    public Step2_TagView()
    {
        try
        {
            InitializeComponent();
            DataContext = this;
            InitializeUI();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"初始化失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void InitializeUI()
    {
        LvTags.ItemsSource = _tags;
        TxtStatus.Text = "就绪 - 请选择音频文件";
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
            TxtStatus.Text = $"已选择音频：{_audioFilePath}";
        }
    }

    private void BtnLoad_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_audioFilePath))
        {
            MessageBox.Show("请先选择音频文件", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        try
        {
            _waveOut?.Stop();
            _waveOut?.Dispose();
            _audioFile?.Dispose();
            
            _audioFile = new AudioFileReader(_audioFilePath);
            _totalDuration = _audioFile.TotalTime.TotalSeconds;
            
            _waveOut = new WaveOutEvent();
            _waveOut.Init(_audioFile);
            
            TxtTotalTime.Text = $"总时长：{_totalDuration:F3} 秒";
            
            TxtStatus.Text = "音频已加载完成";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"加载失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            TxtStatus.Text = "加载失败";
        }
    }

    private void PlayPauseAudioInternal()
    {
        if (_waveOut == null)
        {
            MessageBox.Show("请先加载音频文件", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        if (_waveOut.PlaybackState == PlaybackState.Playing)
        {
            _waveOut.Pause();
            ((Button)FindName("BtnPlayPause")).Content = "播放";
            TxtStatus.Text = "已暂停";
        }
        else
        {
            _waveOut.Play();
            ((Button)FindName("BtnPlayPause")).Content = "暂停";
            TxtStatus.Text = "正在播放...";
            
            StartTimer();
        }
    }

    public void PlayPauseAudio()
    {
        PlayPauseAudioInternal();
    }

    private void StartTimer()
    {
        _timer?.Stop();
        _timer = new System.Windows.Threading.DispatcherTimer();
        _timer.Interval = System.TimeSpan.FromMilliseconds(100);
        _timer.Tick += (s, ev) =>
        {
            if (_audioFile != null)
            {
                _currentPosition = _audioFile.CurrentTime.TotalSeconds;
            }
            
            if (_currentPosition > _totalDuration)
            {
                _currentPosition = 0;
                _timer?.Stop();
                _waveOut?.Stop();
                ((Button)FindName("BtnPlayPause")).Content = "播放";
                TxtStatus.Text = "播放完成";
            }
            
            TxtCurrentTime.Text = $"当前时间：{_currentPosition:F3} 秒";
            
            if (_totalDuration > 0)
            {
                SliTimeline.Value = (_currentPosition / _totalDuration) * 100;
            }
        };
        _timer.Start();
    }

    private void StartTimerForTag(TagItem tag)
    {
        _timer?.Stop();
        _timer = new System.Windows.Threading.DispatcherTimer();
        _timer.Interval = System.TimeSpan.FromMilliseconds(100);
        _timer.Tick += (s, ev) =>
        {
            if (_audioFile != null)
            {
                _currentPosition = _audioFile.CurrentTime.TotalSeconds;
            }
            
            if (_currentPosition >= tag.EndTime)
            {
                _currentPosition = tag.EndTime;
                _timer?.Stop();
                _waveOut?.Pause();
                ((Button)FindName("BtnPlayPause")).Content = "播放";
                TxtStatus.Text = $"已播放到标签 {tag.Index} 结束时间";
            }
            
            TxtCurrentTime.Text = $"当前时间：{_currentPosition:F3} 秒";
            
            if (_totalDuration > 0)
            {
                SliTimeline.Value = (_currentPosition / _totalDuration) * 100;
            }
        };
        _timer.Start();
    }

    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Space)
        {
            PlayPauseAudioInternal();
        }
        else if (e.Key == System.Windows.Input.Key.Add || e.Key == System.Windows.Input.Key.OemPlus)
        {
            if (_waveOut?.PlaybackState == PlaybackState.Playing)
            {
                BtnAddTag_Click(null!, null!);
            }
        }
        else if (e.Key == System.Windows.Input.Key.Delete)
        {
            BtnDeleteTag_Click(null!, null!);
        }
    }

    private void LvTags_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        var selectedTag = LvTags.SelectedItem as TagItem;
        if (selectedTag != null && _totalDuration > 0)
        {
            SliTimeline.Value = (selectedTag.StartTime / _totalDuration) * 100;
            TxtStatus.Text = $"选中标签 {selectedTag.Index}";
        }
    }

    private void SliTimeline_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
    {
        if (_totalDuration > 0 && _audioFile != null)
        {
            _currentPosition = (SliTimeline.Value / 100) * _totalDuration;
            TxtCurrentTime.Text = $"当前时间：{_currentPosition:F3} 秒";
            
            if (SliTimeline.IsMouseOver)
            {
                _audioFile.Position = (long)(_currentPosition * _audioFile.WaveFormat.AverageBytesPerSecond);
            }
        }
    }

    private void SliTimeline_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (_waveOut != null && _audioFile != null && _totalDuration > 0)
        {
            var targetPosition = (SliTimeline.Value / 100) * _totalDuration;
            _audioFile.Position = (long)(targetPosition * _audioFile.WaveFormat.AverageBytesPerSecond);
            _currentPosition = targetPosition;
            TxtCurrentTime.Text = $"当前时间：{_currentPosition:F3} 秒";
            TxtStatus.Text = $"已跳转到：{targetPosition:F3} 秒";
        }
    }

    private void BtnAddTag_Click(object sender, RoutedEventArgs e)
    {
        var currentTime = _currentPosition;
        
        double startTime = _tags.Count > 0 ? _tags.Last().EndTime : 0;
        
        var newTag = new TagItem
        {
            Index = _tags.Count + 1,
            StartTime = startTime,
            EndTime = currentTime,
            Content = ""
        };

        _tags.Add(newTag);
        
        LvTags.ScrollIntoView(newTag);
        TxtStatus.Text = $"已添加标签 {_tags.Count}";
    }

    private void BtnDeleteTag_Click(object sender, RoutedEventArgs e)
    {
        var selectedTag = LvTags.SelectedItem as TagItem;
        if (selectedTag == null)
        {
            MessageBox.Show("请先选择一个标签", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show($"确定要删除标签 {selectedTag.Index} 吗？", "确认删除", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        
        if (result == MessageBoxResult.Yes)
        {
            _tags.Remove(selectedTag);
            
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
                
                TxtStatus.Text = $"已导出 {_tags.Count} 个标签到：{dialog.FileName}";
                MessageBox.Show($"成功导出 {_tags.Count} 个标签", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                TxtStatus.Text = "导出失败";
            }
        }
    }

    private void BtnPlayTag_Click(object sender, RoutedEventArgs e)
    {
        if (_waveOut == null || _audioFile == null)
        {
            MessageBox.Show("请先加载音频文件", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var button = (Button)sender;
        var tag = (TagItem)button.CommandParameter;

        if (tag == null)
        {
            return;
        }

        try
        {
            _waveOut.Stop();
            
            _audioFile.Position = (long)(tag.StartTime * _audioFile.WaveFormat.AverageBytesPerSecond);
            _currentPosition = tag.StartTime;
            
            TxtCurrentTime.Text = $"当前时间：{_currentPosition:F3} 秒";
            SliTimeline.Value = (tag.StartTime / _totalDuration) * 100;
            
            _waveOut.Play();
            ((Button)FindName("BtnPlayPause")).Content = "暂停";
            TxtStatus.Text = $"正在播放标签 {tag.Index}: {tag.StartTime:F3} - {tag.EndTime:F3} 秒";
            
            StartTimerForTag(tag);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"播放失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            TxtStatus.Text = "播放失败";
        }
    }

    private void BtnPlayPauseInternal_Click(object sender, RoutedEventArgs e)
    {
        PlayPauseAudioInternal();
    }

    private void BtnStopInternal_Click(object sender, RoutedEventArgs e)
    {
        if (_waveOut != null)
        {
            _waveOut.Stop();
            _timer?.Stop();
            _currentPosition = 0;
            TxtCurrentTime.Text = "当前时间：0.000 秒";
            SliTimeline.Value = 0;
            TxtStatus.Text = "已停止";
        }
    }
}
