import { apiGetUserFavorites } from '../../services/favorite'
import { apiGetUserLearningStats, apiGetUserProgress } from '../../services/progress'
import { formatDuration } from '../../utils/format'
import { getUser } from '../../utils/storage'

Page({
  data: {
    stats: {
      consecutiveDays: 0,
      totalDuration: 0,
      todayDuration: 0
    },
    favoritesCount: 0,
    popularFavorites: [],
    userId: null,
    userInfo: null
  },

  onLoad: function () {
    const user = getUser()
    if (user) {
      this.setData({ 
        userId: user.id,
        userInfo: {
          nickname: user.nickname,
          avatar: user.avatarUrl || user.avatar
        }
      })
      this.loadStats(user.id)
      this.loadFavoritesCount(user.id)
    }
  },

  onShow: function () {
    const user = getUser()
    if (user && user.id !== this.data.userId) {
      this.setData({ 
        userId: user.id,
        userInfo: {
          nickname: user.nickname,
          avatar: user.avatarUrl || user.avatar
        }
      })
      this.loadStats(user.id)
      this.loadFavoritesCount(user.id)
    }
  },

  async loadStats (userId) {
    try {
      const stats = await apiGetUserLearningStats(userId)
      this.setData({ stats })
    } catch (error) {
    }
  },

  async loadFavoritesCount (userId) {
    try {
      const data = await apiGetUserFavorites(userId)
      this.setData({ favoritesCount: data.length || 0 })
    } catch (error) {
    }
  },

  viewHistory () {
    wx.navigateTo({
      url: '/pages/profile/history'
    })
  },

  viewFavorites () {
    wx.switchTab({
      url: '/pages/favorites/favorites'
    })
  },

  viewSettings () {
    wx.navigateTo({
      url: '/pages/profile/settings'
    })
  },

  onSentenceTap (e) {
    const sentence = e.currentTarget.dataset.sentence
    wx.navigateTo({
      url: `/pages/listen/listen?bookId=${sentence.bookId}&chapterId=${sentence.chapterId}&sentenceId=${sentence.id}`
    })
  },

  formatDuration
})
