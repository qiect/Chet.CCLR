import { request } from '../utils/request'

export const apiGetUserById = (id) => {
  return request({
    url: `/api/Users/${id}`,
    method: 'get'
  })
}

export const apiGetUserByOpenid = (openid) => {
  return request({
    url: `/api/Users/openid/${openid}`,
    method: 'get'
  })
}

export const apiCreateUser = (data) => {
  return request({
    url: '/api/Users',
    method: 'post',
    data
  })
}

export const apiUpdateUser = (id, data) => {
  return request({
    url: `/api/Users/${id}`,
    method: 'put',
    data
  })
}

export const apiUpdateUserAvatar = (id, avatarUrl) => {
  return request({
    url: `/api/Users/${id}/avatar`,
    method: 'put',
    data: { avatarUrl }
  })
}

export const apiUpdateUserNickname = (id, nickname) => {
  return request({
    url: `/api/Users/${id}/nickname`,
    method: 'put',
    data: { nickname }
  })
}
