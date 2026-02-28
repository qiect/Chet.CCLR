import { request } from './request'
import { saveUser, getUser, removeUser, getToken, saveToken  } from './storage'

export const login = async (userInfo) => {
  return new Promise((resolve, reject) => {
    wx.login({
      success: (res) => {
        if (res.code) {
          const data = { code: res.code }
          
          if (userInfo) {
            data.nickname = userInfo.nickName
            data.avatarUrl = userInfo.avatarUrl
            data.gender = userInfo.gender || 0
            data.country = userInfo.country
            data.province = userInfo.province
            data.city = userInfo.city
          }
          
          request({
            url: '/api/Auth/wx-login',
            method: 'post',
            data: data
          })
            .then((response) => {
              const user = response.user
              const token = response.token?.accessToken || response.token
              saveUser(user)
              if (token) {
                saveToken(token)
              }
              resolve(user)
            })
            .catch((error) => {
              reject(error)
            })
        } else {
          reject(new Error('获取微信登录凭证失败'))
        }
      },
      fail: (error) => {
        reject(new Error('微信登录失败'))
      }
    })
  })
}

export const getUserInfoWithAuth = async () => {
  return new Promise((resolve, reject) => {
    wx.getSetting({
      success: (res) => {
        if (res.authSetting['scope.userInfo']) {
          wx.getUserInfo({
            success: (res) => {
              resolve(res.userInfo)
            },
            fail: (error) => {
              reject(error)
            }
          })
        } else {
          reject(new Error('未授权'))
        }
      },
      fail: (error) => {
        reject(error)
      }
    })
  })
}

export const ensureUserExists = async (userInfo) => {
  try {
    const user = getUser()
    if (!user) {
      return null
    }

    const existingUser = await request({
      url: `/api/Users/openid/${user.wxOpenid || user.openid}`,
      method: 'get'
    })

    if (existingUser) {
      const updatedUser = {
        ...existingUser,
        nickname: userInfo?.nickName || existingUser.nickname,
        avatarUrl: userInfo?.avatarUrl || existingUser.avatarUrl
      }
      const result = await request({
        url: `/api/Users/${existingUser.id}`,
        method: 'put',
        data: updatedUser
      })
      saveUser(updatedUser)
      return updatedUser
    } else {
      const newUser = {
        wxOpenid: user.wxOpenid || user.openid,
        nickname: userInfo?.nickName,
        avatarUrl: userInfo?.avatarUrl,
        unionid: userInfo?.unionId,
        gender: userInfo?.gender,
        country: userInfo?.country,
        province: userInfo?.province,
        city: userInfo?.city,
        language: userInfo?.language
      }
      const createdUser = await request({
        url: '/api/Users',
        method: 'post',
        data: newUser
      })
      saveUser(createdUser)
      return createdUser
    }
  } catch (error) {
    throw error
  }
}

export const handleAuthorization = async () => {
  try {
    const user = getUser()
    if (user) {
      const token = getToken()
      if (!token) {
        const createdUser = await login(null)
        return createdUser
      }
      try {
        const userInfo = await getUserInfoWithAuth()
        const updatedUser = await ensureUserExists(userInfo)
        return updatedUser
      } catch (authError) {
        return user
      }
    } else {
      const createdUser = await login(null)
      return createdUser
    }
  } catch (error) {
    const user = getUser()
    if (user) {
      return user
    }
    throw error
  }
}
