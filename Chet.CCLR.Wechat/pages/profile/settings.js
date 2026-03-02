import { getUser } from '../../utils/storage'

Page({
  data: {
    theme: 'light',
    soundMode: 'normal',
    autoPlay: true,
    showPinyin: true,
    userId: null
  },

  onLoad: function () {
    const user = getUser()
    if (user) {
      this.setData({ userId: user.id })
    }
    
    const settings = wx.getStorageSync('settings') || {}
    this.setData(settings)
  },

  onThemeChange (e) {
    const theme = e.detail.value
    this.setData({ theme })
    wx.setStorageSync('settings', { ...this.data, theme })
    
    if (theme === 'dark') {
      wx.setNavigationBarColor({ backgroundColor: '#000', frontColor: '#fff' })
    } else {
      wx.setNavigationBarColor({ backgroundColor: '#fff', frontColor: '#000' })
    }
  },

  onSoundModeChange (e) {
    const soundMode = e.detail.value
    this.setData({ soundMode })
    wx.setStorageSync('settings', { ...this.data, soundMode })
  },

  onAutoPlayChange (e) {
    const autoPlay = e.detail.value
    this.setData({ autoPlay })
    wx.setStorageSync('settings', { ...this.data, autoPlay })
  },

  onShowPinyinChange (e) {
    const showPinyin = e.detail.value
    this.setData({ showPinyin })
    wx.setStorageSync('settings', { ...this.data, showPinyin })
  },

  clearCache () {
    wx.showModal({
      title: '确认清空',
      content: '确定要清空缓存吗？',
      success: () => {
        wx.clearStorageSync()
        wx.showToast({
          title: '已清空',
          icon: 'success'
        })
      }
    })
  },

  about () {
    wx.showModal({
      title: '关于我们',
      content: '国学经典听读 v1.0.0',
      showCancel: false
    })
  },

  disclaimer () {
    wx.showModal({
      title: '免责声明',
      content: '本应用提供的内容仅供学习参考，不构成任何法律建议。如有侵权，请联系我们删除相关内容。',
      showCancel: false
    })
  }
})
