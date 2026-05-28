import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

const MOCK_USERS = [
  { email: 'public@test.com', password: 'public123', fullName: 'John Citizen', role: 'public', token: 'mock-token-public' },
  { email: 'staff@test.com', password: 'staff123', fullName: 'Jane Staff', role: 'staff', token: 'mock-token-staff' },
]

export const useAuthStore = defineStore('auth', () => {
  const token = ref(localStorage.getItem('token'))
  const role = ref(localStorage.getItem('role'))
  const fullName = ref(localStorage.getItem('fullName'))

  const isLoggedIn = computed(() => !!token.value)
  const isStaff = computed(() => role.value === 'staff')

  async function login(email, password) {
    const user = MOCK_USERS.find(u => u.email === email && u.password === password)
    if (!user) return { success: false, error: 'Invalid email or password' }

    token.value = user.token
    role.value = user.role
    fullName.value = user.fullName
    localStorage.setItem('token', user.token)
    localStorage.setItem('role', user.role)
    localStorage.setItem('fullName', user.fullName)
    return { success: true }
  }

  async function register(_fullName, _email, _password) {
    await new Promise(r => setTimeout(r, 500))
    return { success: true }
  }

  function logout() {
    token.value = null
    role.value = null
    fullName.value = null
    localStorage.removeItem('token')
    localStorage.removeItem('role')
    localStorage.removeItem('fullName')
  }

  return { token, role, fullName, isLoggedIn, isStaff, login, register, logout }
})
