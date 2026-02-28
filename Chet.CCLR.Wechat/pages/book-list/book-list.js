import { apiGetBooks, apiSearchBooks } from '../../services/book'

Page({
  data: {
    books: [],
    keyword: ''
  },

  onLoad: function () {
    this.loadBooks()
  },

  onSearchInput (e) {
    const keyword = e.detail.value
    this.setData({ keyword })
    if (keyword.trim()) {
      this.searchBooks(keyword)
    } else {
      this.loadBooks()
    }
  },

  onSearchConfirm () {
    if (this.data.keyword.trim()) {
      this.searchBooks(this.data.keyword)
    }
  },

  async searchBooks (keyword) {
    try {
      const data = await apiSearchBooks(keyword)
      this.setData({ books: data })
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

  onBookTap (e) {
    const book = e.currentTarget.dataset.book
    wx.navigateTo({
      url: `/pages/book-detail/book-detail?id=${book.id}`
    })
  }
})
