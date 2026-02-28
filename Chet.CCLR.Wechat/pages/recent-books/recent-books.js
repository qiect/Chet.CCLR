import { apiGetUserAllProgress } from '../../services/progress'
import { getUser } from '../../utils/storage'

Page({
  data: {
    recentBooks: []
  },

  onLoad: function () {
    const user = getUser()
    if (user) {
      this.loadRecentBooks(user.id)
    }
  },

  onShow: function () {
    const user = getUser()
    if (user) {
      this.loadRecentBooks(user.id)
    }
  },

  async loadRecentBooks (userId) {
    try {
      const data = await apiGetUserAllProgress(userId)
      this.setData({ recentBooks: data })
    } catch (error) {
    }
  },

  onBookTap (e) {
    const book = e.currentTarget.dataset.book
    wx.navigateTo({
      url: `/pages/chapter-detail/chapter-detail?bookId=${book.bookId}&bookTitle=${encodeURIComponent(book.title)}`
    })
  }
})
