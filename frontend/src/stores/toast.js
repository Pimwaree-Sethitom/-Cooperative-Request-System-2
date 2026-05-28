import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useToastStore = defineStore('toast', () => {
  const message = ref('')
  const type = ref('')
  let timer = null

  function show(msg, t) {
    message.value = msg
    type.value = t
    clearTimeout(timer)
    timer = setTimeout(() => { message.value = '' }, 3000)
  }

  const success = (msg) => show(msg, 'success')
  const error = (msg) => show(msg, 'error')

  return { message, type, success, error }
})
