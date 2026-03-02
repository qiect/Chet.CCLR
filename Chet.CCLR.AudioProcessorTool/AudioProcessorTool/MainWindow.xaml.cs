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
using AudioProcessorTool.Services;
using AudioProcessorTool.Views;

namespace AudioProcessorTool;

public partial class MainWindow : Window
{
    private readonly ISentenceSplitter _sentenceSplitter;
    private readonly IAudioCutter _audioCutter;
    private readonly IOutputGenerator _outputGenerator;

    public MainWindow()
    {
        _sentenceSplitter = new SentenceSplitter();
        _audioCutter = new AudioCutter();
        _outputGenerator = new OutputGenerator();
        
        InitializeComponent();
        InitializeUI();
    }

    private void InitializeUI()
    {
        TxtStatus.Text = "就绪";
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Space)
        {
            if (TabMain.SelectedItem is TabItem tabItem && tabItem.Content is Step2_TagView step2View)
            {
                step2View.PlayPauseAudio();
            }
        }
    }
}
