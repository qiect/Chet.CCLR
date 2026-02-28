import { request } from '../utils/request'

export const apiGetUserFavorites = (userId) => {
  return request({
    url: `/api/UserFavorites/user/${userId}`,
    method: 'get'
  })
}

export const apiGetPopularFavorites = (limit = 10) => {
  return request({
    url: `/api/UserFavorites/popular?limit=${limit}`,
    method: 'get'
  })
}

export const apiGetFavoriteById = (id) => {
  return request({
    url: `/api/UserFavorites/${id}`,
    method: 'get'
  })
}

export const apiCreateFavorite = (data) => {
  return request({
    url: '/api/UserFavorites',
    method: 'post',
    data
  })
}

export const apiRemoveFavorite = (userId, sentenceId) => {
  return request({
    url: `/api/UserFavorites/user/${userId}/sentence/${sentenceId}`,
    method: 'delete'
  })
}

export const apiIsFavorite = (userId, sentenceId) => {
  return request({
    url: `/api/UserFavorites/user/${userId}/sentence/${sentenceId}/is-favorited`,
    method: 'get'
  })
}
