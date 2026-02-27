import { request } from '../utils/request'

export const apiGetUserProgress = (userId, bookId) => {
  return request({
    url: `/api/UserListenProgress/user/${userId}/book/${bookId}`,
    method: 'get'
  })
}

export const apiGetUserAllProgress = (userId) => {
  return request({
    url: `/api/UserListenProgress/user/${userId}/all`,
    method: 'get'
  })
}

export const apiUpdateUserProgress = (data) => {
  return request({
    url: '/api/UserListenProgress',
    method: 'post',
    data
  })
}

export const apiResetUserProgress = (userId, bookId) => {
  return request({
    url: `/api/UserListenProgress/reset/user/${userId}/book/${bookId}`,
    method: 'post'
  })
}

export const apiGetCurrentSentence = (userId, bookId) => {
  return request({
    url: `/api/UserListenProgress/user/${userId}/book/${bookId}/current-sentence`,
    method: 'get'
  })
}

export const apiGetUserLearningStats = (userId) => {
  return request({
    url: `/api/UserListenProgress/user/${userId}/stats`,
    method: 'get'
  })
}
