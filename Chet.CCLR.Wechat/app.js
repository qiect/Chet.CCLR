import { handleAuthorization } from './utils/auth'

App({
  onLaunch: async function () {
    try {
      const user = await handleAuthorization()
      this.globalData.userInfo = user
    } catch (error) {
      // 即使登录失败，也继续运行，让用户可以浏览内容
    }
  },

  onShow: function () {
  },

  onHide: function () {
  },

  globalData: {
    userInfo: null
  }
})
