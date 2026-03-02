# 音频处理工具

基于 WPF 的音频处理工具，整合了自动分句、音频打标签和音频拆解功能。

## 功能特性

### 1. 自动分句 (步骤1)
- 读取文本文件（.txt）
- 智能中文分句算法（支持句号、感叹号、问号、分号）
- 自动识别章节标题
- 预览和导出分句结果

### 2. 音频打标签 (步骤2)
- 加载和播放音频文件
- 添加、编辑、删除标签
- 实时时间轴显示
- 导出 labels.txt 标签文件

### 3. 音频拆解 (步骤3)
- 按标签时间轴切割音频
- 自动检测 FFmpeg
- 生成音频片段
- 自动生成数据库插入脚本和说明文档

## 使用流程

### 第一步：自动分句
1. 点击"步骤1: 自动分句"按钮
2. 选择文本文件（.txt）
3. 点击"自动分句"按钮
4. 预览和导出分句结果（sentences.txt）

### 第二步：音频打标签
1. 点击"步骤2: 音频打标签"按钮
2. 选择并加载音频文件
3. 播放音频，按空格键添加标签
4. 编辑和调整标签
5. 导出标签文件（labels.txt）

### 第三步：音频拆解
1. 点击"步骤3: 音频拆解"按钮
2. 选择原始音频文件
3. 选择 labels.txt 标签文件
4. 点击"开始切割"按钮
5. 查看输出结果（audio 文件夹、insert_script.sql、README.md）

## 技术架构

### 核心组件
- **NAudio**：音频播放和处理
- **WPF**：UI 界面
- **.NET 10**：运行时环境

### 服务层
- `ISentenceSplitter` / `SentenceSplitter`：分句服务
- `IAudioCutter` / `AudioCutter`：音频切割服务
- `IOutputGenerator` / `OutputGenerator`：输出文件生成服务

## 依赖项

- **NAudio 2.2.1**：音频处理库
- **.NET 10.0 Windows**：运行时环境

## 输出文件

```
output/
├── sentences.txt       # 分句结果
├── labels.txt          # 时间轴标签
├── audio/              # 音频片段
│   ├── 001.m4a
│   ├── 002.m4a
│   └── ...
├── insert_script.sql   # 数据库插入脚本
└── README.md          # 使用说明
```

## FFmpeg 依赖

要进行实际的音频切割，需要安装 FFmpeg 并将其添加到系统 PATH：

1. 下载 FFmpeg：https://ffmpeg.org/download.html
2. 解压并配置环境变量
3. 验证安装：`ffmpeg -version`

如果没有安装 FFmpeg，程序会创建模拟音频文件用于演示。

## 快捷键

| 快捷键 | 功能 |
|--------|------|
| 空格 | 播放/暂停（步骤2） |
| + | 添加标签（步骤2，播放状态下） |
| Delete | 删除选中的标签（步骤2） |

## 项目结构

```
AudioTagger/
├── AudioTagger/
│   ├── Services/              # 服务层
│   │   ├── ISentenceSplitter.cs
│   │   ├── SentenceSplitter.cs
│   │   ├── IAudioCutter.cs
│   │   ├── AudioCutter.cs
│   │   ├── IOutputGenerator.cs
│   │   └── OutputGenerator.cs
│   ├── Views/                 # 视图层
│   │   ├── Step1_SplitView.xaml
│   │   ├── Step1_SplitView.xaml.cs
│   │   ├── Step3_CutView.xaml
│   │   └── Step3_CutView.xaml.cs
│   ├── MainWindow.xaml
│   ├── MainWindow.xaml.cs
│   ├── App.xaml
│   ├── App.xaml.cs
│   ├── AssemblyInfo.cs
│   └── AudioTagger.csproj
└── README.md
```

## 开发说明

### 编译项目
```bash
dotnet build
```

### 运行项目
```bash
dotnet run --project AudioTagger
```

### 从 AudioProcessor 迁移
原 AudioProcessor 的功能已完全整合到 AudioTagger 中：
- 自动分句算法 → 步骤1
- 音频切割功能 → 步骤3
- 输出文件生成 → 步骤3

## 注意事项

1. **音频格式支持**：推荐使用 MP3 或 WAV 格式，确保兼容性
2. **时间精度**：标签时间精度为毫秒级（0.001 秒）
3. **大文件处理**：建议处理前备份原始音频文件
4. **输出路径**：所有输出文件保存在与 labels.txt 同目录的 audio 文件夹中

## 未来改进

- [ ] 支持批量处理多个音频文件
- [ ] 支持音频波形显示
- [ ] 支持快捷键自定义
- [ ] 支持撤销/重做操作
- [ ] 支持数据库直接导入（EF Core）
- [ ] 支持云存储集成

## 技术支持

如有问题或建议，请联系开发团队。
