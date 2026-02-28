import { request } from '../utils/request'

export const apiGetChaptersByBookId = (bookId) => {
  return request({
    url: `/api/ClassicChapters/book/${bookId}`,
    method: 'get'
  })
}

export const apiGetChapterById = (id) => {
  return request({
    url: `/api/ClassicChapters/${id}`,
    method: 'get'
  })
}

export const apiGetChapterWithSentences = (id) => {
  return request({
    url: `/api/ClassicChapters/${id}/with-sentences`,
    method: 'get'
  })
}

export const apiGetSentencesByChapterId = (chapterId) => {
  return request({
    url: `/api/ClassicSentences/chapter/${chapterId}`,
    method: 'get'
  })
}
