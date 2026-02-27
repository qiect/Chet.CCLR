import { request } from '../utils/request'

export const apiGetBooks = () => {
  return request({
    url: '/api/ClassicBooks',
    method: 'get'
  })
}

export const apiGetBookById = (id) => {
  return request({
    url: `/api/ClassicBooks/${id}`,
    method: 'get'
  })
}

export const apiGetBooksByCategory = (category) => {
  return request({
    url: `/api/ClassicBooks/category/${category}`,
    method: 'get'
  })
}

export const apiGetRecommendedBooks = (limit = 10) => {
  return request({
    url: `/api/ClassicBooks/recommended?limit=${limit}`,
    method: 'get'
  })
}

export const apiSearchBooks = (keyword = '') => {
  return request({
    url: `/api/ClassicBooks/search?keyword=${keyword}`,
    method: 'get'
  })
}
