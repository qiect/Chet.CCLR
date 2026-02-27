import { request } from '../utils/request'

export const apiGetSentencesByChapterId = (chapterId) => {
  return request({
    url: `/api/ClassicSentences/chapter/${chapterId}`,
    method: 'get'
  })
}

export const apiGetSentenceById = (id) => {
  return request({
    url: `/api/ClassicSentences/${id}`,
    method: 'get'
  })
}

export const apiGetRandomSentences = (limit = 5) => {
  return request({
    url: `/api/ClassicSentences/random?limit=${limit}`,
    method: 'get'
  })
}

export const apiSearchSentences = (keyword = '') => {
  return request({
    url: `/api/ClassicSentences/search?keyword=${keyword}`,
    method: 'get'
  })
}
