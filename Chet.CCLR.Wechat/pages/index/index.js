import { apiGetRecommendedBooks } from '../../services/book'
import { apiGetUserAllProgress } from '../../services/progress'
import { apiGetRandomSentences } from '../../services/sentence'
import { getUser } from '../../utils/storage'

Page({
  data: {
    sentence: null,
    recentBooks: [],
    recommendedBooks: [],
    userId: null
  },

  onLoad: function () {
    const user = getUser()
    if (user) {
      this.setData({ userId: user.id })
    }
    this.loadSentence()
    this.loadRecentBooks()
    this.loadRecommendedBooks()
  },

  onShow: function () {
    const user = getUser()
    if (user && user.id !== this.data.userId) {
      this.setData({ userId: user.id })
    }
  },

  async loadSentence () {
    try {
      const data = await apiGetRandomSentences(1)
      this.setData({ sentence: data[0] })
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

  async loadRecommendedBooks () {
    try {
      const data = await apiGetRecommendedBooks(6)
      this.setData({ recommendedBooks: data })
    } catch (error) {
    }
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

  viewAllRecent () {
    wx.navigateTo({
      url: '/pages/recent-books/recent-books'
    })
  },

  viewAllBooks () {
    wx.navigateTo({
      url: '/pages/book-list/book-list'
    })
  }
})
