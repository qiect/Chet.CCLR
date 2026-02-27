import { apiGetBookById, apiGetChaptersByBookId } from '../../services/book'
import { apiGetChapterWithSentences } from '../../services/chapter'
import { getUser } from '../../utils/storage'

Page({
  data: {
    book: null,
    chapters: [],
    userId: null
  },

  onLoad: function (options) {
    const user = getUser()
    if (user) {
      this.setData({ userId: user.id })
    }
    this.loadBook(options.id)
  },

  async loadBook (id) {
    try {
      const book = await apiGetBookById(id)
      this.setData({ book })
      
      const chapters = await apiGetChaptersByBookId(id)
      this.setData({ chapters })
    } catch (error) {
      console.error('加载书籍详情失败', error)
    }
  },

  onChapterTap (e) {
    const chapter = e.currentTarget.dataset.chapter
    wx.navigateTo({
      url: `/pages/chapter-detail/chapter-detail?bookId=${this.data.book.id}&bookTitle=${encodeURIComponent(this.data.book.title)}&chapterId=${chapter.id}&chapterTitle=${encodeURIComponent(chapter.title)}`
    })
  },

  startListening () {
    if (this.data.chapters.length > 0) {
      wx.navigateTo({
        url: `/pages/chapter-detail/chapter-detail?bookId=${this.data.book.id}&bookTitle=${encodeURIComponent(this.data.book.title)}&chapterId=${this.data.chapters[0].id}&chapterTitle=${encodeURIComponent(this.data.chapters[0].title)}`
      })
    } else {
      wx.showToast({
        title: '暂无章节',
        icon: 'none'
      })
    }
  }
})
