import { getToken } from './storage'

let API_BASE_URL = 'https://619f1f43.r7.cpolar.cn'

export const request = (options) => {
  return new Promise((resolve, reject) => {
    const token = getToken()
    wx.request({
      url: `${API_BASE_URL}${options.url}`,
      method: options.method || 'GET',
      data: options.data || {},
      header: {
        'content-type': 'application/json',
        ...(token ? { 'authorization': `Bearer ${token}` } : {}),
        ...options.header
      },
      success: (res) => {
        if (res.statusCode === 200) {
          if (res.data.success === true) {
            resolve(res.data.data)
          } else {
            // 对于某些正常情况（如进度不存在），返回 null
            if (res.data.message?.includes('not found') || res.data.message?.includes('Not Found')) {
              console.log('API 返回空数据:', res.data.message)
              resolve(null)
            } else {
              reject(res)
              wx.showToast({
                title: res.data.message || '请求失败',
                icon: 'none'
              })
            }
          }
        } else if (res.statusCode === 404 && res.data && res.data.message?.includes('not found')) {
          // 处理 404 not found 的情况
          console.log('API 返回空数据 (404):', res.data.message)
          resolve(null)
        } else {
          reject(res)
          wx.showToast({
            title: res.data?.message || '请求失败',
            icon: 'none'
          })
        }
      },
      fail: (error) => {
        reject(error)
        wx.showToast({
          title: '网络错误',
          icon: 'none'
        })
      }
    })
  })
}

export const setBaseUrl = (url) => {
  API_BASE_URL = url
}

export const getBaseUrl = () => {
  return API_BASE_URL
}
