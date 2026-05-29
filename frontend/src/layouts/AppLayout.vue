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
    case 'dashboard':       return 'แดชบอร์ด'
    case 'my-requests':     return 'คำขอของฉัน'
    case 'new-request':     return 'ยื่นคำขอใหม่'
    case 'staff-dashboard': return 'แดชบอร์ดเจ้าหน้าที่'
    case 'staff-requests':  return 'ตรวจสอบคำขอ'
    default:                return ''
  }
})

const pageSubtitle = computed(() => {
  switch (route.name) {
    case 'dashboard':       return `ยินดีต้อนรับ, ${auth.fullName}`
    case 'my-requests':     return 'คำขอจดทะเบียนสหกรณ์ทั้งหมดของคุณ'
    case 'new-request':     return 'กรอกรายละเอียดด้านล่าง โดยต้องมีสมาชิกผู้ก่อตั้งอย่างน้อย 10 คน'
    case 'staff-dashboard': return `ยินดีต้อนรับ, ${auth.fullName}`
    case 'staff-requests':  return 'จัดการและตรวจสอบคำขอจดทะเบียนสหกรณ์'
    default:                return ''
  }
})
</script>
