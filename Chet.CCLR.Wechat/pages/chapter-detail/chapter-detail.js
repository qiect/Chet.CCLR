import { apiGetChapterWithSentences } from '../../services/chapter'
import { apiGetUserProgress } from '../../services/progress'
import { getUser } from '../../utils/storage'

Page({
  data: {
    bookTitle: '',
    chapterTitle: '',
    sentences: [],
    userId: null,
    bookId: '',
    chapterId: ''
  },

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
    this.loadChapter(options.chapterId)
  },

  async loadChapter (chapterId) {
    try {
      const chapter = await apiGetChapterWithSentences(chapterId)
      this.setData({ 
        sentences: chapter.sentences || [],
        chapterTitle: chapter.title
      })
    } catch (error) {
      console.error('加载章节失败', error)
    }
  },

  startListening () {
    if (this.data.sentences.length > 0) {
      wx.navigateTo({
        url: `/pages/listen/listen?bookId=${this.data.bookId}&bookTitle=${encodeURIComponent(this.data.bookTitle)}&chapterId=${this.data.chapterId}&chapterTitle=${encodeURIComponent(this.data.chapterTitle)}`
      })
    } else {
      wx.showToast({
        title: '暂无句子',
        icon: 'none'
      })
    }
  }
})
