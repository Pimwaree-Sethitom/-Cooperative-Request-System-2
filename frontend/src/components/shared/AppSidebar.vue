<template>
  <aside class="w-56 min-h-screen bg-white border-r border-gray-100 flex flex-col shrink-0">

    <!-- Logo -->
    <div class="h-14 flex items-center gap-3 px-5 border-b border-gray-100">
      <div class="w-8 h-8 rounded-xl bg-primary-600 flex items-center justify-center shrink-0">
        <Building2 class="w-4 h-4 text-white" />
      </div>
      <div>
        <p class="text-sm font-bold text-gray-900 tracking-tight leading-none">CoopMS</p>
        <p class="text-[10px] text-gray-400 uppercase tracking-widest mt-0.5">Management System</p>
      </div>
    </div>

    <!-- Navigation -->
    <nav class="flex-1 px-3 py-4 space-y-0.5">
      <RouterLink
        v-for="item in navItems"
        :key="item.to"
        :to="item.to"
        class="flex items-center gap-3 px-3 py-2.5 rounded-xl text-sm font-medium transition-all"
        :class="isActive(item.to)
          ? 'bg-primary-50 text-primary-700'
          : 'text-gray-500 hover:bg-gray-50 hover:text-gray-800'"
      >
        <component
          :is="item.icon"
          class="w-4 h-4 shrink-0"
          :class="isActive(item.to) ? 'text-primary-600' : ''"
        />
        {{ item.label }}
      </RouterLink>
    </nav>

    <!-- User Info -->
    <div class="px-4 py-4 border-t border-gray-100 flex items-center gap-3">
      <div class="w-8 h-8 rounded-full bg-primary-600 flex items-center justify-center text-white text-xs font-bold shrink-0">
        {{ initials }}
      </div>
      <div class="flex-1 min-w-0">
        <p class="text-sm font-semibold text-gray-900 truncate leading-none">{{ authStore.fullName }}</p>
        <p class="text-[10px] text-gray-400 mt-0.5 capitalize">
          {{ authStore.role === 'staff' ? 'Staff User' : 'Public User' }}
        </p>
      </div>
      <button
        @click="handleLogout"
        class="text-gray-300 hover:text-gray-600 transition-colors"
        title="Sign out"
      >
        <LogOut class="w-4 h-4" />
      </button>
    </div>

  </aside>
</template>

<script setup>
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { Building2, LayoutDashboard, FileText, PlusCircle, LogOut } from 'lucide-vue-next'
import { useAuthStore } from '@/stores/auth'

const route     = useRoute()
const router    = useRouter()
const authStore = useAuthStore()

const publicNavItems = [
  { to: '/dashboard',   label: 'Dashboard',   icon: LayoutDashboard },
  { to: '/my-requests', label: 'My Requests',  icon: FileText        },
  { to: '/new-request', label: 'New Request',  icon: PlusCircle      },
]

const staffNavItems = [
  { to: '/staff/dashboard', label: 'Dashboard',        icon: LayoutDashboard },
  { to: '/staff/requests',  label: 'Review Requests',  icon: FileText        },
]

const navItems = computed(() => authStore.isStaff ? staffNavItems : publicNavItems)

const isActive = (path) => route.path === path

const initials = computed(() =>
  (authStore.fullName || '')
    .split(' ')
    .map(n => n[0])
    .join('')
    .toUpperCase()
    .slice(0, 2)
)

function handleLogout() {
  authStore.logout()
  router.push('/login')
}
</script>
