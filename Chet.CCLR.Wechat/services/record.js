import { request } from '../utils/request'

export const apiGetRecordByUserAndDate = (userId, date) => {
  return request({
    url: `/api/UserListenRecords/user/${userId}/date/${date}`,
    method: 'get'
  })
}

export const apiGetRecordsByUserAndDateRange = (userId, startDate, endDate) => {
  return request({
    url: `/api/UserListenRecords/user/${userId}/range?startDate=${startDate}&endDate=${endDate}`,
    method: 'get'
  })
}

export const apiGetRecentRecords = (userId, days = 7) => {
  return request({
    url: `/api/UserListenRecords/user/${userId}/recent?days=${days}`,
    method: 'get'
  })
}

export const apiCreateRecord = (data) => {
  return request({
    url: '/api/UserListenRecords',
    method: 'post',
    data
  })
}

export const apiUpdateRecord = (id, data) => {
  return request({
    url: `/api/UserListenRecords/${id}`,
    method: 'put',
    data
  })
}

export const apiGetConsecutiveListenDays = (userId) => {
  return request({
    url: `/api/UserListenRecords/user/${userId}/consecutive-days`,
    method: 'get'
  })
}

export const apiGetTotalListenDays = (userId) => {
  return request({
    url: `/api/UserListenRecords/user/${userId}/total-days`,
    method: 'get'
  })
}
