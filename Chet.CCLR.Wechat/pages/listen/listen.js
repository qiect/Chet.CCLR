import { apiGetChapterWithSentences, apiGetChaptersByBookId } from '../../services/chapter'
import { apiGetUserProgress, apiUpdateUserProgress, apiGetCurrentSentence, apiGetUserAllProgress } from '../../services/progress'
import { apiCreateRecord } from '../../services/record'
import { playAudio, pauseAudio, getCurrentTime, getDuration, seekAudio } from '../../utils/audio'
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
      const chapters = await apiGetChaptersByBookId(bookId)
      if (chapters.length > 0) {
        const chapter = chapters[0]
        this.setData({ chapter, bookId })
        await this.loadSentences(chapter.id)
        await this.loadProgress(bookId, chapter.id)
      }
    } catch (error) {
      console.error('加载章节失败', error)
    }
  },

  async loadSentences (chapterId) {
    try {
      const data = await apiGetSentencesByChapterId(chapterId)
      this.setData({ sentences: data })
      if (data.length > 0) {
        this.setData({ currentIndex: 0 })
      }
    } catch (error) {
      console.error('加载句子失败', error)
    }
  },

  async loadProgress (bookId, chapterId) {
    const userId = this.data.userId
    if (!userId) return

    try {
      const progress = await apiGetUserProgress(userId, bookId)
      if (progress && progress.sentenceId) {
        const sentences = this.data.sentences
        const index = sentences.findIndex(s => s.id === progress.sentenceId)
        if (index !== -1) {
          this.setData({ currentIndex: index })
          if (progress.progressSec > 0) {
            this.seekAudio(progress.progressSec)
          }
        }
      }
    } catch (error) {
      console.error('加载进度失败', error)
    }
  },

  async playSentence (index) {
    const sentence = this.data.sentences[index]
    if (!sentence || !sentence.audioUrl) {
      wx.showToast({
        title: '音频不存在',
        icon: 'none'
      })
      return
    }

    if (this.audioContext) {
      this.audioContext.stop()
    }

    try {
      this.audioContext = await playAudio(sentence.audioUrl)
      
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
          currentTime: this.audioContext.currentTime
        })
      })

      this.audioContext.onEnded(() => {
        this.playNext()
      })

      this.audioContext.onError((res) => {
        console.error('播放错误', res)
        wx.showToast({
          title: '播放失败',
          icon: 'none'
        })
        this.setData({ isPlaying: false })
      })
    } catch (error) {
      console.error('播放失败', error)
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
    if (!this.audioContext) return

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
    const { currentIndex, sentences, bookId } = this.data
    
    if (!userId || currentIndex < 0 || !sentences[currentIndex]) return

    const sentence = sentences[currentIndex]
    const progress = getCurrentTime(this.audioContext)

    apiUpdateUserProgress({
      userId,
      bookId,
      sentenceId: sentence.id,
      progressSec: Math.floor(progress)
    }).catch(error => {
      console.error('保存进度失败', error)
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
    if (this.data.currentIndex >= 0) {
      this.playSentence(this.data.currentIndex)
    }
  }
})
