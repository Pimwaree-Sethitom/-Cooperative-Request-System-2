<template>
  <div class="min-h-screen bg-gradient-to-br from-primary-50 to-primary-100 flex items-center justify-center p-4">
    <div class="w-full max-w-md">
      <!-- Logo -->
      <div class="text-center mb-8">
        <div class="inline-flex items-center justify-center w-16 h-16 rounded-2xl bg-primary-600 mb-4">
          <Building2 class="w-8 h-8 text-white" />
        </div>
        <h1 class="text-2xl font-bold text-gray-900">Create Account</h1>
        <p class="text-gray-500 text-sm mt-1">Register as a public user</p>
      </div>

      <!-- Register Form -->
      <AppCard>
        <form class="space-y-5" @submit.prevent="handleSubmit">
          <AppInput
            id="fullName"
            v-model="form.fullName"
            type="text"
            label="Full Name"
            placeholder="John Doe"
            :error="errors.fullName"
            required
          />

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
            placeholder="At least 6 characters"
            :error="errors.password"
            required
          />

          <AppInput
            id="confirmPassword"
            v-model="form.confirmPassword"
            type="password"
            label="Confirm Password"
            placeholder="Repeat your password"
            :error="errors.confirmPassword"
            required
          />

          <div v-if="errors.general" class="p-3 rounded-lg bg-red-50 border border-red-200">
            <p class="text-sm text-red-700">{{ errors.general }}</p>
          </div>

          <AppButton type="submit" class="w-full" :loading="isLoading">
            Create Account
          </AppButton>
        </form>

        <template #footer>
          <p class="text-sm text-center text-gray-600">
            Already have an account?
            <router-link to="/login" class="text-primary-600 hover:text-primary-700 font-medium">
              Sign in here
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

const router = useRouter()
const auth = useAuthStore()

const form = reactive({ fullName: '', email: '', password: '', confirmPassword: '' })
const errors = reactive({ fullName: '', email: '', password: '', confirmPassword: '', general: '' })
const isLoading = ref(false)

function validate() {
  Object.keys(errors).forEach(k => errors[k] = '')
  let valid = true

  if (!form.fullName) {
    errors.fullName = 'Full name is required'
    valid = false
  }
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
  } else if (form.password.length < 6) {
    errors.password = 'Password must be at least 6 characters'
    valid = false
  }
  if (!form.confirmPassword) {
    errors.confirmPassword = 'Please confirm your password'
    valid = false
  } else if (form.password !== form.confirmPassword) {
    errors.confirmPassword = 'Passwords do not match'
    valid = false
  }

  return valid
}

async function handleSubmit() {
  if (!validate()) return
  isLoading.value = true
  try {
    const result = await auth.register(form.fullName, form.email, form.password)
    if (result.success) {
      router.push('/login')
    } else {
      errors.general = result.error
    }
  } finally {
    isLoading.value = false
  }
}
</script>
