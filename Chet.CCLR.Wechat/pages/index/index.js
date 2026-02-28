import { apiGetBooks, apiGetRecommendedBooks } from '../../services/book'
import { apiGetUserAllProgress, apiGetUserLearningStats } from '../../services/progress'
import { formatDuration } from '../../utils/format'
import { getUser } from '../../utils/storage'

Page({
  data: {
    recommendations: [],
    recentBooks: [],
    books: [],
    stats: {
      consecutiveDays: 0,
      totalDuration: 0,
      todayDuration: 0
    },
    userId: null
  },

  onLoad: function () {
    const user = getUser()
    if (user) {
      this.setData({ userId: user.id })
      this.loadStats(user.id)
    }
    this.loadRecommendations()
    this.loadBooks()
    this.loadRecentBooks()
  },

  onShow: function () {
    const user = getUser()
    if (user && user.id !== this.data.userId) {
      this.setData({ userId: user.id })
      this.loadStats(user.id)
    }
  },

  async loadRecommendations () {
    try {
      const data = await apiGetRecommendedBooks(5)
      this.setData({ recommendations: data })
    } catch (error) {
    }
  },

  async loadBooks () {
    try {
      const data = await apiGetBooks()
      this.setData({ books: data })
    } catch (error) {
    }
  },

  async loadRecentBooks () {
    const userId = this.data.userId
    if (!userId) return
    
    try {
      const data = await apiGetUserAllProgress(userId)
      this.setData({ recentBooks: data })
    } catch (error) {
    }
  },

  async loadStats (userId) {
    try {
      const stats = await apiGetUserLearningStats(userId)
      this.setData({ stats })
    } catch (error) {
    }
  },

  formatDuration,
  
  onRecommendationTap (e) {
    const book = e.currentTarget.dataset.book
    wx.navigateTo({
      url: `/pages/book-detail/book-detail?id=${book.id}`
    })
  },

  onBookTap (e) {
    const book = e.currentTarget.dataset.book
    wx.navigateTo({
      url: `/pages/book-detail/book-detail?id=${book.id}`
    })
  },

  onRecentTap (e) {
    const book = e.currentTarget.dataset.book
    wx.navigateTo({
      url: `/pages/chapter-detail/chapter-detail?bookId=${book.bookId}&bookTitle=${encodeURIComponent(book.title)}`
    })
  },

  viewAllRecommendations () {
    wx.navigateTo({
      url: '/pages/index/index?tab=recommendations'
    })
  },

  viewAllRecent () {
    wx.navigateTo({
      url: '/pages/index/index?tab=recent'
    })
  },

  viewAllBooks () {
    wx.navigateTo({
      url: '/pages/index/index?tab=books'
    })
  }
})
