const STORAGE_KEY_USER = 'chet_user'
const STORAGE_KEY_TOKEN = 'chet_token'

export const saveUser = (user) => {
  try {
    wx.setStorageSync(STORAGE_KEY_USER, user)
  } catch (e) {
    console.error('保存用户信息失败', e)
  }
}

export const getUser = () => {
  try {
    return wx.getStorageSync(STORAGE_KEY_USER)
  } catch (e) {
    console.error('获取用户信息失败', e)
    return null
  }
}

export const removeUser = () => {
  try {
    wx.removeStorageSync(STORAGE_KEY_USER)
  } catch (e) {
    console.error('清除用户信息失败', e)
  }
}

export const saveToken = (token) => {
  try {
    wx.setStorageSync(STORAGE_KEY_TOKEN, token)
  } catch (e) {
    console.error('保存token失败', e)
  }
}

export const getToken = () => {
  try {
    return wx.getStorageSync(STORAGE_KEY_TOKEN)
  } catch (e) {
    console.error('获取token失败', e)
    return null
  }
}

export const removeToken = () => {
  try {
    wx.removeStorageSync(STORAGE_KEY_TOKEN)
  } catch (e) {
    console.error('清除token失败', e)
  }
}

export const clearStorage = () => {
  try {
    wx.clearStorageSync()
  } catch (e) {
    console.error('清除所有存储失败', e)
  }
}
