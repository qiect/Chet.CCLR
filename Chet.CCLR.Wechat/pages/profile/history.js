import { apiGetUserAllProgress } from '../../services/progress'
import { formatDuration } from '../../utils/format'
import { getUser } from '../../utils/storage'

Page({
  data: {
    history: [],
    userId: null
  },

  onLoad: function () {
    const user = getUser()
    if (user) {
      this.setData({ userId: user.id })
      this.loadHistory(user.id)
    }
  },

  onShow: function () {
    const user = getUser()
    if (user && user.id !== this.data.userId) {
      this.setData({ userId: user.id })
      this.loadHistory(user.id)
    }
  },

  async loadHistory (userId) {
    try {
      const data = await apiGetUserAllProgress(userId)
      this.setData({ history: data })
    } catch (error) {
    }
  },

  onBookTap (e) {
    const book = e.currentTarget.dataset.book
    wx.navigateTo({
      url: `/pages/book-detail/book-detail?id=${book.bookId}`
    })
  },

  formatDuration
})
