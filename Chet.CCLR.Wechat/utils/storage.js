const STORAGE_KEY_USER = 'chet_user'
const STORAGE_KEY_TOKEN = 'chet_token'

export const saveUser = (user) => {
  try {
    wx.setStorageSync(STORAGE_KEY_USER, user)
  } catch (e) {
  }
}

export const getUser = () => {
  try {
    return wx.getStorageSync(STORAGE_KEY_USER)
  } catch (e) {
    return null
  }
}

export const removeUser = () => {
  try {
    wx.removeStorageSync(STORAGE_KEY_USER)
  } catch (e) {
  }
}

export const saveToken = (token) => {
  try {
    wx.setStorageSync(STORAGE_KEY_TOKEN, token)
  } catch (e) {
  }
}

export const getToken = () => {
  try {
    return wx.getStorageSync(STORAGE_KEY_TOKEN)
  } catch (e) {
    return null
  }
}

export const removeToken = () => {
  try {
    wx.removeStorageSync(STORAGE_KEY_TOKEN)
  } catch (e) {
  }
}

export const clearStorage = () => {
  try {
    wx.clearStorageSync()
  } catch (e) {
  }
}
