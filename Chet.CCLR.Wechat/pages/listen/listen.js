import { apiGetChapterWithSentences, apiGetChaptersByBookId, apiGetSentencesByChapterId } from '../../services/chapter'
import { apiGetUserProgress, apiUpdateUserProgress, apiGetCurrentSentence, apiGetUserAllProgress } from '../../services/progress'
import { apiCreateRecord } from '../../services/record'
import { playAudio, pauseAudio, getCurrentTime, getDuration, seekAudio, getFullAudioUrl } from '../../utils/audio'
import { formatDuration, getToday } from '../../utils/format'
import { getUser } from '../../utils/storage'

Page({
  data: {
    book: null,
    chapter: null,
    sentences: [],
    currentIndex: 0,
    isPlaying: false,
    currentTime: 0,
    duration: 0,
    showPinyin: true,
    showNote: false,
    currentSentenceNote: '',
    userId: null,
    bookId: '',
    bookTitle: ''
  },

  audioContext: null,

  onLoad: function (options) {
    const user = getUser()
    if (user) {
      this.setData({ userId: user.id })
    }
    this.setData({
      bookId: options.bookId,
      bookTitle: decodeURIComponent(options.bookTitle)
    })
    this.loadChapter(options.bookId)
  },

  onShow: function () {
    const user = getUser()
    if (user && user.id !== this.data.userId) {
      this.setData({ userId: user.id })
    }
  },

  async loadChapter (bookId) {
    try {
      console.log('开始加载章节列表, bookId:', bookId)
      const chapters = await apiGetChaptersByBookId(bookId)
      console.log('获取章节列表:', chapters)
      if (chapters.length > 0) {
        const chapter = chapters[0]
        console.log('选择第一个章节:', chapter)
        this.setData({ chapter, bookId })
        await this.loadSentences(chapter.id)
        await this.loadProgress(this.data.userId, bookId)
      } else {
        console.warn('没有获取到章节列表')
        wx.showToast({
          title: '暂无章节',
          icon: 'none'
        })
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

  async loadSentences (chapterId) {
    try {
      const data = await apiGetSentencesByChapterId(chapterId)
      console.log('获取句子列表:', data)
      this.setData({ sentences: data })
      if (data.length > 0) {
        this.setData({ currentIndex: 0 })
        console.log('句子列表加载成功，共', data.length, '个句子')
      } else {
        console.warn('没有获取到句子数据')
        wx.showToast({
          title: '暂无句子',
          icon: 'none'
        })
      }
    } catch (error) {
      console.error('加载句子列表失败:', error)
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
          currentIndex: index
        })
        this.scrollToCurrent(index)
      })

      this.audioContext.onTimeUpdate(() => {
        this.setData({
          currentTime: this.audioContext.currentTime,
          duration: this.audioContext.duration
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
    const { currentIndex, sentences, bookId, chapter } = this.data
    
    if (!userId || currentIndex < 0 || !sentences[currentIndex]) return

    const sentence = sentences[currentIndex]
    const progress = getCurrentTime(this.audioContext)

    apiUpdateUserProgress({
      userId,
      bookId,
      chapterId: chapter?.id,
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
  }
})
