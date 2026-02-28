import { apiGetChapterWithSentences, apiGetChaptersByBookId, apiGetSentencesByChapterId } from '../../services/chapter'
import { apiGetUserProgress, apiUpdateUserProgress, apiGetCurrentSentence, apiGetUserAllProgress } from '../../services/progress'
import { apiCreateRecord } from '../../services/record'
import { playAudio, pauseAudio, getCurrentTime, getDuration, seekAudio, getFullAudioUrl } from '../../utils/audio'
import { formatDuration, getToday } from '../../utils/format'
import { getUser } from '../../utils/storage'

// 使用全局背景音频管理器以获得更好的听读体验
const bgAudio = wx.getBackgroundAudioManager();

Page({
  data: {
    bookTitle: '',
    chapterTitle: '',
    chapter: null,
    sentences: [],
    currentIndex: 0,
    isPlaying: false,
    currentTime: 0,
    duration: 0,
    showPinyin: true,
    showNote: false,
    currentSentenceNote: '',
    scrollY: '',
    userId: null,
    bookId: '',
    chapterId: '',
    showTimeDisplay: false,
    seekTime: 0,
    showProgressThumb: false,
    isDragging: false // 增加拖拽状态锁
  },

  audioContext: null,
  lastUpdateTime: 0,
  progressRect: null,

  onLoad: function (options) {
    const user = getUser()
    if (user) {
      this.setData({ userId: user.id })
    }
    this.setData({
      bookTitle: decodeURIComponent(options.bookTitle),
      bookId: options.bookId,
      chapterId: options.chapterId
    })
    
    // 初始化音频回调（衔接原有的播放逻辑）
    this.initAudioCallback();

    if (this.data.chapterId) {
      this.loadChapter(this.data.chapterId)
    } else {
      this.loadFirstChapter()
    }
  },

  // 新增：初始化音频监听，确保UI与后台播放同步
  initAudioCallback() {
    bgAudio.onPlay(() => this.setData({ isPlaying: true }));
    bgAudio.onPause(() => this.setData({ isPlaying: false }));
    bgAudio.onStop(() => this.setData({ isPlaying: false }));
    bgAudio.onEnded(() => this.playNext());
    bgAudio.onTimeUpdate(() => {
      if (!this.data.isDragging) {
        this.setData({
          currentTime: bgAudio.currentTime || 0,
          duration: bgAudio.duration || 0
        });
      }
    });
  },

  async loadFirstChapter () {
    try {
      const chapters = await apiGetChaptersByBookId(this.data.bookId)
      if (chapters && chapters.length > 0) {
        const firstChapter = chapters[0]
        this.setData({
          chapterId: firstChapter.id,
          chapterTitle: firstChapter.title
        })
        await this.loadChapter(firstChapter.id)
      } else {
        wx.showToast({
          title: '暂无章节数据',
          icon: 'none'
        })
      }
    } catch (error) {
      console.error('加载章节列表失败:', error)
      wx.showToast({
        title: '加载失败',
        icon: 'none'
      })
    }
  },

  onShow: function () {
    const user = getUser()
    if (user && user.id !== this.data.userId) {
      this.setData({ userId: user.id })
    }
  },

  async loadChapter (chapterId) {
    try {
      console.log('开始加载章节详情, chapterId:', chapterId)
      const response = await apiGetChapterWithSentences(chapterId)
      console.log('获取章节详情:', response)
      const chapter = response.Chapter || response
      const sentences = response.Sentences || response.sentences || []
      
      this.setData({ 
        chapter: chapter,
        chapterTitle: chapter.title || chapter.chapterTitle,
        sentences: sentences,
        bookId: chapter.bookId || this.data.bookId
      })
      
      console.log('章节句子数量:', sentences.length)
      if (sentences.length > 0) {
        await this.loadProgress(this.data.userId, chapter.bookId || this.data.bookId)
      }
    } catch (error) {
      console.error('加载章节失败:', error)
      wx.showToast({
        title: '加载失败: ' + (error.message || '未知错误'),
        icon: 'none',
        duration: 3000
      })
    }
  },

  async loadProgress (userId, bookId) {
    if (!userId) {
      console.log('没有用户ID，跳过进度加载')
      return
    }

    try {
      console.log('开始加载用户进度, userId:', userId, 'bookId:', bookId)
      const progress = await apiGetUserProgress(userId, bookId)
      console.log('获取用户进度:', progress)
      if (progress && progress.sentenceId) {
        const sentences = this.data.sentences
        const index = sentences.findIndex(s => s.id === progress.sentenceId)
        if (index !== -1) {
          this.setData({ currentIndex: index })
          console.log('恢复到之前的进度，句子索引:', index)
          // 如果有进度秒数，则在播放时seek
          if (progress.progressSec > 0) {
            this.playSentence(index, progress.progressSec);
          }
        }
      }
    } catch (error) {
      console.error('加载用户进度失败:', error)
    }
  },

  async playSentence (index, seekTime = 0) {
    const sentence = this.data.sentences[index]
    if (!sentence) return;
    
    const audioUrl = sentence.audioUrl || sentence.AudioUrl
    if (!audioUrl) {
      wx.showToast({ title: '音频不存在', icon: 'none' })
      return
    }

    // 切换到 BackgroundAudioManager 逻辑
    bgAudio.title = sentence.content;
    bgAudio.epname = this.data.bookTitle;
    bgAudio.singer = this.data.chapterTitle;
    bgAudio.src = getFullAudioUrl(audioUrl);

    if (seekTime > 0) {
      bgAudio.seek(seekTime);
    }

    this.setData({ 
      currentIndex: index,
      currentSentenceNote: sentence.note || '' 
    });
    
    this.scrollToCurrent(index);
  },

  playNext () {
    const { currentIndex, sentences } = this.data
    if (currentIndex < sentences.length - 1) {
      this.playSentence(currentIndex + 1)
    } else {
      wx.showToast({ title: '已是最后一章', icon: 'none' })
    }
  },

  playPrev () {
    const { currentIndex } = this.data
    if (currentIndex > 0) {
      this.playSentence(currentIndex - 1)
    }
  },

  togglePlay () {
    // 逻辑保留：如果没有正在播放的音频，则从当前句子开始
    if (!bgAudio.src) {
      if (this.data.sentences.length > 0) {
        this.playSentence(this.data.currentIndex)
      }
      return
    }

    if (this.data.isPlaying) {
      bgAudio.pause();
      this.saveProgress();
    } else {
      bgAudio.play();
    }
  },

  // 优化后的滚动逻辑，对应 WXML 中的 id="sent-{{index}}"
  scrollToCurrent (index) {
    this.setData({
      scrollY: `sent-${index}`
    });
  },

  async saveProgress () {
    const userId = this.data.userId
    const { currentIndex, sentences, bookId, chapterId } = this.data
    
    if (!userId || currentIndex < 0 || !sentences[currentIndex]) return

    const sentence = sentences[currentIndex]
    const progress = bgAudio.currentTime || 0;

    try {
      await apiUpdateUserProgress({
        userId,
        bookId,
        chapterId,
        sentenceId: sentence.id,
        progressSec: Math.floor(progress)
      })
    } catch (e) {
      console.error('保存进度失败', e);
    }
  },

  togglePinyin () {
    this.setData({ showPinyin: !this.data.showPinyin })
  },

  toggleNote () {
    // 逻辑保留并针对新UI做了简化
    this.setData({ showNote: !this.data.showNote });
  },

  // 进度条交互逻辑（对应 slider 组件）
  onSliderChanging() {
    this.setData({ isDragging: true });
  },

  onSliderChange(e) {
    const time = e.detail.value;
    bgAudio.seek(time);
    this.setData({
      currentTime: time,
      isDragging: false
    });
  },

  onUnload: function () {
    this.saveProgress();
    // 根据需要决定是否在退出页面时停止背景音频
    // bgAudio.stop(); 
  },

  formatDuration,
  
  onReady: function () {
    if (this.data.sentences.length > 0) {
      this.playSentence(this.data.currentIndex)
    }
  },

  onSentenceTap (e) {
    const index = e.currentTarget.dataset.index
    this.playSentence(index)
  }
})