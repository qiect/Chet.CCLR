import { apiGetChapterWithSentences, apiGetChaptersByBookId, apiGetSentencesByChapterId } from '../../services/chapter'
import { apiGetUserProgress, apiUpdateUserProgress, apiGetCurrentSentence, apiGetUserAllProgress } from '../../services/progress'
import { apiCreateRecord } from '../../services/record'
import { playAudio, pauseAudio, getCurrentTime, getDuration, seekAudio, getFullAudioUrl } from '../../utils/audio'
import { formatDuration, getToday } from '../../utils/format'
import { getUser } from '../../utils/storage'

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
    chapterId: ''
  },

  audioContext: null,

  onLoad: function (options) {
    const user = getUser()
    if (user) {
      this.setData({ userId: user.id })
    }
    this.setData({
      bookTitle: decodeURIComponent(options.bookTitle),
      chapterTitle: decodeURIComponent(options.chapterTitle),
      bookId: options.bookId,
      chapterId: options.chapterId
    })
    this.loadChapter(options.chapterId).then(() => {
      console.log('章节加载完成')
    }).catch((error) => {
      console.error('章节加载失败:', error)
    })
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
        console.log('第一个句子的AudioUrl:', sentences[0].audioUrl || sentences[0].AudioUrl)
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
          if (progress.progressSec > 0 && this.audioContext) {
            seekAudio(this.audioContext, progress.progressSec)
          }
        } else {
          console.warn('进度中的句子不在当前列表中')
        }
      } else {
        console.log('没有找到用户进度，从头开始')
      }
    } catch (error) {
      console.error('加载用户进度失败:', error)
      // 用户进度不存在是正常情况，不需要提示错误
    }
  },

  async playSentence (index) {
    const sentence = this.data.sentences[index]
    if (!sentence) {
      wx.showToast({
        title: '句子不存在',
        icon: 'none'
      })
      return
    }
    
    const audioUrl = sentence.audioUrl || sentence.AudioUrl
    if (!audioUrl) {
      console.error('句子没有音频URL:', sentence)
      wx.showToast({
        title: '音频不存在',
        icon: 'none'
      })
      return
    }

    console.log('开始播放句子:', index, 'AudioUrl:', audioUrl)

    if (this.audioContext) {
      this.audioContext.stop()
    }

    try {
      const fullAudioUrl = getFullAudioUrl(audioUrl)
      console.log('完整音频URL:', fullAudioUrl)
      this.audioContext = await playAudio(fullAudioUrl)
      
      this.audioContext.onPlay(() => {
        this.setData({
          isPlaying: true,
          currentTime: 0,
          duration: this.audioContext.duration || 0,
          currentIndex: index
        })
        this.scrollToCurrent(index)
      })

      this.audioContext.onTimeUpdate(() => {
        const currentTime = this.audioContext.currentTime || 0
        const duration = this.audioContext.duration || 0
        console.log('onTimeUpdate - currentTime:', currentTime, 'duration:', duration)
        this.setData({
          currentTime: currentTime,
          duration: duration
        })
      })

      this.audioContext.onEnded(() => {
        this.playNext()
      })

      this.audioContext.onError((res) => {
        console.error('音频播放错误:', res)
        wx.showToast({
          title: '播放失败: ' + res.errMsg,
          icon: 'none',
          duration: 3000
        })
        this.setData({ isPlaying: false })
      })
    } catch (error) {
      console.error('播放音频失败:', error)
      wx.showToast({
        title: '播放失败',
        icon: 'none',
        duration: 3000
      })
    }
  },

  playNext () {
    const { currentIndex, sentences } = this.data
    if (currentIndex < sentences.length - 1) {
      this.playSentence(currentIndex + 1)
    }
  },

  playPrev () {
    const { currentIndex } = this.data
    if (currentIndex > 0) {
      this.playSentence(currentIndex - 1)
    }
  },

  togglePlay () {
    if (!this.audioContext) {
      // 如果没有 audioContext，播放当前句子
      if (this.data.sentences.length > 0 && this.data.currentIndex >= 0) {
        this.playSentence(this.data.currentIndex)
      } else {
        wx.showToast({
          title: '没有可播放的音频',
          icon: 'none'
        })
      }
      return
    }

    if (this.data.isPlaying) {
      pauseAudio(this.audioContext)
      this.setData({ isPlaying: false })
      this.saveProgress()
    } else {
      this.audioContext.play()
      this.setData({ isPlaying: true })
    }
  },

  scrollToCurrent (index) {
    const query = wx.createSelectorQuery()
    query.select(`#sentence-${index}`).boundingClientRect()
    query.selectViewport().scrollOffset()
    query.exec((res) => {
      if (res[0] && res[1]) {
        const sentenceTop = res[0].top
        const viewportTop = res[1].scrollTop
        const scrollY = sentenceTop + viewportTop - wx.getSystemInfoSync().windowHeight / 2
        
        this.setData({ scrollY })
        setTimeout(() => {
          wx.pageScrollTo({
            scrollTop: scrollY,
            duration: 300
          })
        }, 100)
      }
    })
  },

  saveProgress () {
    const userId = this.data.userId
    const { currentIndex, sentences, bookId} = this.data
    
    if (!userId || currentIndex < 0 || !sentences[currentIndex]) return

    const sentence = sentences[currentIndex]
    const progress = getCurrentTime(this.audioContext)

    apiUpdateUserProgress({
      userId,
      bookId,
      chapterId,
      sentenceId: sentence.id,
      progressSec: Math.floor(progress)
    }).catch(() => {
    })
  },

  togglePinyin () {
    this.setData({ showPinyin: !this.data.showPinyin })
  },

  toggleNote (index) {
    const { showNote, currentSentenceNote } = this.data
    if (showNote && currentSentenceNote) {
      this.setData({ showNote: false, currentSentenceNote: '' })
    } else {
      const sentence = this.data.sentences[index]
      this.setData({ 
        showNote: true, 
        currentSentenceNote: sentence.note || '' 
      })
    }
  },

  onUnload: function () {
    if (this.audioContext) {
      this.audioContext.stop()
      this.saveProgress()
    }
  },

  formatDuration,
  
  onReady: function () {
    console.log('onReady - 句子数量:', this.data.sentences.length)
    console.log('onReady - currentIndex:', this.data.currentIndex)
    if (this.data.sentences.length > 0 && this.data.currentIndex >= 0) {
      console.log('自动播放第一个句子')
      this.playSentence(this.data.currentIndex)
    } else {
      console.log('句子数据未加载完成，等待加载')
    }
  },

  onSentenceTap (e) {
    const index = e.currentTarget.dataset.index
    this.playSentence(index)
  }
})
