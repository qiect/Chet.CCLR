import { apiGetUserFavorites, apiRemoveFavorite } from '../../services/favorite'
import { apiGetSentenceById } from '../../services/sentence'
import { formatDuration } from '../../utils/format'
import { getUser } from '../../utils/storage'

Page({
  data: {
    favorites: [],
    popularFavorites: [],
    userId: null,
    showPopular: false
  },

  onLoad: function () {
    const user = getUser()
    if (user) {
      this.setData({ userId: user.id })
      this.loadFavorites(user.id)
    }
    this.loadPopularFavorites()
  },

  onShow: function () {
    const user = getUser()
    if (user && user.id !== this.data.userId) {
      this.setData({ userId: user.id })
      this.loadFavorites(user.id)
    }
  },

  async loadFavorites (userId) {
    try {
      const data = await apiGetUserFavorites(userId)
      this.setData({ favorites: data })
    } catch (error) {
      console.error('加载收藏失败', error)
    }
  },

  async loadPopularFavorites () {
    try {
      const data = await apiGetPopularFavorites(10)
      this.setData({ popularFavorites: data })
    } catch (error) {
      console.error('加载热门收藏失败', error)
    }
  },

  async removeFavorite (e) {
    const sentenceId = e.currentTarget.dataset.sentenceid
    const index = e.currentTarget.dataset.index

    wx.showModal({
      title: '确认删除',
      content: '确定要取消收藏吗？',
      success: async (res) => {
        if (res.confirm) {
          const userId = this.data.userId
          try {
            await apiRemoveFavorite(userId, sentenceId)
            const favorites = this.data.favorites
            favorites.splice(index, 1)
            this.setData({ favorites })
            wx.showToast({
              title: '已取消收藏',
              icon: 'success'
            })
          } catch (error) {
            console.error('取消收藏失败', error)
            wx.showToast({
              title: '操作失败',
              icon: 'none'
            })
          }
        }
      }
    })
  },

  onSentenceTap (e) {
    const sentence = e.currentTarget.dataset.sentence
    wx.navigateTo({
      url: `/pages/listen/listen?bookId=${sentence.bookId}&chapterId=${sentence.chapterId}&sentenceId=${sentence.id}`
    })
  },

  toggleShowPopular () {
    this.setData({ showPopular: !this.data.showPopular })
  },

  formatDuration
})
