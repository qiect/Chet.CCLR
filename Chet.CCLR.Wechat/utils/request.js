let API_BASE_URL = 'https://619f1f43.r7.cpolar.cn'

export const request = (options) => {
  return new Promise((resolve, reject) => {
    wx.request({
      url: `${API_BASE_URL}${options.url}`,
      method: options.method || 'GET',
      data: options.data || {},
      header: {
        'content-type': 'application/json',
        ...options.header
      },
      success: (res) => {
        if (res.statusCode === 200 && res.data.success === true) {
          resolve(res.data.data)
        } else {
          reject(res)
          wx.showToast({
            title: res.data.message || '请求失败',
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
