import { getBaseUrl } from './request'

let API_BASE_URL = 'https://619f1f43.r7.cpolar.cn'

export const getFullAudioUrl = (audioUrl) => {
  if (!audioUrl) return ''
  
  if (audioUrl.startsWith('http://') || audioUrl.startsWith('https://')) {
    return audioUrl
  }
  
  return `${API_BASE_URL}/${audioUrl}`
}

export const playAudio = (src, startTime = 0) => {
  return new Promise((resolve, reject) => {
    const innerAudioContext = wx.createInnerAudioContext()
    
    console.log('准备播放音频:', src)
    innerAudioContext.src = src
    innerAudioContext.startTime = startTime
    innerAudioContext.autoplay = true
    
    innerAudioContext.onPlay(() => {
      console.log('音频开始播放')
      resolve(innerAudioContext)
    })
    
    innerAudioContext.onError((res) => {
      console.error('音频播放错误:', res)
      reject(res)
    })
    
    innerAudioContext.onEnded(() => {
      console.log('音频播放结束')
    })
  })
}

export const pauseAudio = (audioContext) => {
  if (audioContext) {
    audioContext.pause()
  }
}

export const stopAudio = (audioContext) => {
  if (audioContext) {
    audioContext.stop()
    audioContext.seek(0)
  }
}

export const getCurrentTime = (audioContext) => {
  if (audioContext) {
    return audioContext.currentTime
  }
  return 0
}

export const getDuration = (audioContext) => {
  if (audioContext) {
    return audioContext.duration
  }
  return 0
}

export const setVolume = (audioContext, volume) => {
  if (audioContext) {
    audioContext.volume = volume
  }
}

export const setPlaybackRate = (audioContext, rate) => {
  if (audioContext) {
    audioContext.playbackRate = rate
  }
}

export const seekAudio = (audioContext, time) => {
  if (audioContext) {
    audioContext.seek(time)
  }
}

export const onPlay = (audioContext, callback) => {
  if (audioContext) {
    audioContext.onPlay(callback)
  }
}

export const onTimeUpdate = (audioContext, callback) => {
  if (audioContext) {
    audioContext.onTimeUpdate(callback)
  }
}

export const onEnded = (audioContext, callback) => {
  if (audioContext) {
    audioContext.onEnded(callback)
  }
}

export const onError = (audioContext, callback) => {
  if (audioContext) {
    audioContext.onError(callback)
  }
}

export const offPlay = (audioContext, callback) => {
  if (audioContext) {
    audioContext.offPlay(callback)
  }
}

export const offTimeUpdate = (audioContext, callback) => {
  if (audioContext) {
    audioContext.offTimeUpdate(callback)
  }
}

export const offEnded = (audioContext, callback) => {
  if (audioContext) {
    audioContext.offEnded(callback)
  }
}

export const offError = (audioContext, callback) => {
  if (audioContext) {
    audioContext.offError(callback)
  }
}
