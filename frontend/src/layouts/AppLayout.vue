<template>
  <div class="flex min-h-screen bg-white">
    <AppSidebar />
    <div class="flex-1 flex flex-col overflow-hidden">
      <AppNavbar :title="pageTitle" :subtitle="pageSubtitle" />
      <main class="flex-1 overflow-auto">
        <RouterView />
      </main>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import AppSidebar from '@/components/shared/AppSidebar.vue'
import AppNavbar  from '@/components/shared/AppNavbar.vue'

const route = useRoute()
const auth  = useAuthStore()

const pageTitle = computed(() => {
  switch (route.name) {
    case 'dashboard':       return 'Dashboard'
    case 'my-requests':     return 'My Requests'
    case 'new-request':     return 'New Request'
    case 'staff-dashboard': return 'Staff Dashboard'
    case 'staff-requests':  return 'Review Requests'
    default:                return ''
  }
})

const pageSubtitle = computed(() => {
  switch (route.name) {
    case 'dashboard':       return `Welcome back, ${auth.fullName}`
    case 'my-requests':     return 'All your cooperative registration submissions'
    case 'new-request':     return 'Fill in the details below. At least 10 founding members are required.'
    case 'staff-dashboard': return `Welcome, ${auth.fullName}`
    case 'staff-requests':  return 'Manage cooperative registration applications'
    default:                return ''
  }
})
</script>
