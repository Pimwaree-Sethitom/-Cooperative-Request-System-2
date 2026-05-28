<template>
  <div class="min-h-screen bg-gradient-to-br from-primary-50 to-primary-100 flex items-center justify-center p-4">
    <div class="w-full max-w-md">
      <!-- Logo -->
      <div class="text-center mb-8">
        <div class="inline-flex items-center justify-center w-16 h-16 rounded-2xl bg-primary-600 mb-4">
          <Building2 class="w-8 h-8 text-white" />
        </div>
        <h1 class="text-2xl font-bold text-gray-900">Cooperative Management</h1>
        <p class="text-gray-500 text-sm mt-1">Sign in to your account</p>
      </div>

      <!-- Login Form -->
      <AppCard>
        <form class="space-y-5" @submit.prevent="handleSubmit">
          <AppInput
            id="email"
            v-model="form.email"
            type="email"
            label="Email Address"
            placeholder="you@example.com"
            :error="errors.email"
            required
          />

          <AppInput
            id="password"
            v-model="form.password"
            type="password"
            label="Password"
            placeholder="Enter your password"
            :error="errors.password"
            required
          />

          <div v-if="errors.general" class="p-3 rounded-lg bg-red-50 border border-red-200">
            <p class="text-sm text-red-700">{{ errors.general }}</p>
          </div>

          <AppButton type="submit" class="w-full" :loading="isLoading">
            Sign In
          </AppButton>
        </form>

        <!-- Demo credentials -->
        <div class="mt-6 pt-6 border-t border-gray-100">
          <p class="text-sm text-gray-500 text-center mb-3">Demo Accounts</p>
          <div class="space-y-2 text-xs">
            <button
              type="button"
              class="w-full p-2 bg-gray-50 rounded-lg text-left hover:bg-gray-100 transition-colors"
              @click="fillDemo('public')"
            >
              <span class="font-medium text-gray-700">Public User:</span>
              <span class="text-gray-500 ml-1">public@test.com</span>
            </button>
            <button
              type="button"
              class="w-full p-2 bg-gray-50 rounded-lg text-left hover:bg-gray-100 transition-colors"
              @click="fillDemo('staff')"
            >
              <span class="font-medium text-gray-700">Staff Officer:</span>
              <span class="text-gray-500 ml-1">staff@test.com</span>
            </button>
          </div>
        </div>

        <template #footer>
          <p class="text-sm text-center text-gray-600">
            Don't have an account?
            <router-link to="/register" class="text-primary-600 hover:text-primary-700 font-medium">
              Register here
            </router-link>
          </p>
        </template>
      </AppCard>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { Building2 } from 'lucide-vue-next'
import AppCard from '@/components/ui/AppCard.vue'
import AppInput from '@/components/ui/AppInput.vue'
import AppButton from '@/components/ui/AppButton.vue'
import { useAuthStore } from '@/stores/auth'
import { useToastStore } from '@/stores/toast'

const router = useRouter()
const auth = useAuthStore()
const toast = useToastStore()

const form = reactive({ email: '', password: '' })
const errors = reactive({ email: '', password: '', general: '' })
const isLoading = ref(false)

function fillDemo(type) {
  form.email = type === 'public' ? 'public@test.com' : 'staff@test.com'
  form.password = type === 'public' ? 'public123' : 'staff123'
}

function validate() {
  errors.email = ''
  errors.password = ''
  errors.general = ''
  let valid = true
  if (!form.email) {
    errors.email = 'Email is required'
    valid = false
  } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email)) {
    errors.email = 'Invalid email format'
    valid = false
  }
  if (!form.password) {
    errors.password = 'Password is required'
    valid = false
  }
  return valid
}

async function handleSubmit() {
  if (!validate()) return
  isLoading.value = true
  try {
    const result = await auth.login(form.email, form.password)
    if (result.success) {
      toast.success('Welcome back!')
      router.push(auth.isStaff ? '/staff/dashboard' : '/dashboard')
    } else {
      errors.general = result.error
    }
  } finally {
    isLoading.value = false
  }
}
</script>
