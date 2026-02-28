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
    this.loadChapter(options.chapterId).then(() => {
      console.log('章节加载完成')
    }).catch((error) => {
      console.error('章节加载失败:', error)
    })
  },

  async loadChapter (chapterId) {
    try {
      const response = await apiGetChapterWithSentences(chapterId)
      console.log('获取章节详情:', response)
      const chapter = response.Chapter || response
      const sentences = response.Sentences || response.sentences || []
      this.setData({ 
        sentences: sentences,
        chapterTitle: chapter.title || chapter.chapterTitle
      })
      console.log('章节句子数量:', sentences.length)
      if (sentences.length > 0) {
        console.log('第一个句子的AudioUrl:', sentences[0].audioUrl || sentences[0].AudioUrl)
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

  startListening () {
    console.log('startListening - 句子数量:', this.data.sentences.length)
    console.log('startListening - 句子数据:', this.data.sentences)
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
