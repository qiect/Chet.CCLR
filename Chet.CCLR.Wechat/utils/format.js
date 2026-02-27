export const formatTime = (date) => {
  const year = date.getFullYear()
  const month = date.getMonth() + 1
  const day = date.getDate()
  const hour = date.getHours()
  const minute = date.getMinutes()
  const second = date.getSeconds()

  return `${[year, month, day].map(formatNumber).join('-')} ${[hour, minute, second].map(formatNumber).join(':')}`
}

export const formatNumber = (n) => {
  n = n.toString()
  return n[1] ? n : `0${n}`
}

export const formatDuration = (seconds) => {
  const mins = Math.floor(seconds / 60)
  const secs = Math.floor(seconds % 60)
  return `${formatNumber(mins)}:${formatNumber(secs)}`
}

export const formatDate = (date) => {
  const year = date.getFullYear()
  const month = date.getMonth() + 1
  const day = date.getDate()
  return `${year}-${formatNumber(month)}-${formatNumber(day)}`
}

export const getToday = () => {
  const date = new Date()
  return formatDate(date)
}

export const getYesterday = () => {
  const date = new Date()
  date.setDate(date.getDate() - 1)
  return formatDate(date)
}
