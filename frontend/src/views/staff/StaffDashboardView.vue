<template>
  <div class="p-8 space-y-6">

    <!-- Stats row -->
    <div class="grid grid-cols-4 gap-3">
      <div class="bg-white rounded-2xl border border-gray-200 p-5">
        <div class="flex items-center gap-3">
          <div class="w-9 h-9 rounded-xl bg-gray-50 flex items-center justify-center">
            <FileText class="w-4 h-4 text-gray-400" />
          </div>
          <div>
            <p class="text-[11px] font-semibold text-gray-400 uppercase tracking-widest">คำขอทั้งหมด</p>
            <p class="text-2xl font-bold text-gray-900 tracking-tight">{{ stats.total }}</p>
          </div>
        </div>
      </div>

      <div class="bg-amber-50 rounded-2xl border border-amber-100 p-5">
        <div class="flex items-center gap-3">
          <div class="w-9 h-9 rounded-xl bg-amber-100 flex items-center justify-center">
            <Clock class="w-4 h-4 text-amber-500" />
          </div>
          <div>
            <p class="text-[11px] font-semibold text-amber-600 uppercase tracking-widest">รอตรวจสอบ</p>
            <p class="text-2xl font-bold text-gray-900 tracking-tight">{{ stats.pending }}</p>
          </div>
        </div>
      </div>

      <div class="bg-block-mint rounded-2xl border border-primary-100 p-5">
        <div class="flex items-center gap-3">
          <div class="w-9 h-9 rounded-xl bg-primary-100 flex items-center justify-center">
            <CheckCircle class="w-4 h-4 text-primary-600" />
          </div>
          <div>
            <p class="text-[11px] font-semibold text-primary-700 uppercase tracking-widest">อนุมัติแล้ว</p>
            <p class="text-2xl font-bold text-gray-900 tracking-tight">{{ stats.approved }}</p>
          </div>
        </div>
      </div>

      <div class="bg-red-50 rounded-2xl border border-red-100 p-5">
        <div class="flex items-center gap-3">
          <div class="w-9 h-9 rounded-xl bg-red-100 flex items-center justify-center">
            <XCircle class="w-4 h-4 text-red-500" />
          </div>
          <div>
            <p class="text-[11px] font-semibold text-red-600 uppercase tracking-widest">ถูกปฏิเสธ</p>
            <p class="text-2xl font-bold text-gray-900 tracking-tight">{{ stats.rejected }}</p>
          </div>
        </div>
      </div>
    </div>

    <!-- Main content: 2-col layout -->
    <div class="grid grid-cols-5 gap-5">

      <!-- Left: Donut chart -->
      <div class="col-span-2 bg-white rounded-2xl border border-gray-200 p-6">
        <p class="text-base font-bold text-gray-900 tracking-tight mb-6">การกระจายคำขอ</p>

        <div class="flex flex-col items-center gap-6">
          <div class="relative w-44 h-44">
            <svg viewBox="0 0 36 36" class="w-44 h-44">
              <circle cx="18" cy="18" r="15.9155" fill="none" stroke="#f3f4f6" stroke-width="3.8" />
              <circle
                v-for="seg in donutSegments"
                :key="seg.label"
                cx="18" cy="18" r="15.9155"
                fill="none"
                :stroke="seg.color"
                stroke-width="3.8"
                stroke-linecap="butt"
                :stroke-dasharray="`${seg.pct} 100`"
                :stroke-dashoffset="seg.offset"
              />
            </svg>
            <div class="absolute inset-0 flex items-center justify-center">
              <div class="text-center">
                <p class="text-2xl font-bold text-gray-900 leading-none">{{ stats.total }}</p>
                <p class="text-xs text-gray-400 mt-1">ทั้งหมด</p>
              </div>
            </div>
          </div>

          <!-- Legend -->
          <div class="flex items-center gap-5">
            <div class="flex items-center gap-1.5">
              <span class="w-2.5 h-2.5 rounded-full bg-amber-400 shrink-0"></span>
              <span class="text-sm text-gray-600">รอดำเนินการ</span>
            </div>
            <div class="flex items-center gap-1.5">
              <span class="w-2.5 h-2.5 rounded-full bg-primary-500 shrink-0"></span>
              <span class="text-sm text-gray-600">อนุมัติแล้ว</span>
            </div>
            <div class="flex items-center gap-1.5">
              <span class="w-2.5 h-2.5 rounded-full bg-red-500 shrink-0"></span>
              <span class="text-sm text-gray-600">ถูกปฏิเสธ</span>
            </div>
          </div>
        </div>
      </div>

      <!-- Right: Pending requests list -->
      <div class="col-span-3 bg-white rounded-2xl border border-gray-200 p-6">
        <div class="flex items-start justify-between mb-1">
          <div>
            <p class="text-base font-bold text-gray-900 tracking-tight">คำขอที่รอตรวจสอบ</p>
            <p class="text-sm text-gray-400 mt-0.5">รอการตรวจสอบจากคุณ</p>
          </div>
          <RouterLink
            to="/staff/requests"
            class="text-sm font-semibold text-primary-600 hover:text-primary-800 transition-colors mt-0.5"
          >
            ดูทั้งหมด
          </RouterLink>
        </div>

        <div class="mt-5 divide-y divide-gray-200">
          <div
            v-for="coop in pendingRequests"
            :key="coop.id"
            class="flex items-center justify-between py-4"
          >
            <div class="flex-1 min-w-0 mr-4">
              <p class="text-sm font-bold text-gray-900">{{ coop.name }}</p>
              <p class="text-xs text-gray-400 mt-0.5">
                {{ coop.applicant }} · {{ coop.city }} · {{ coop.members.length }} สมาชิก
              </p>
            </div>
            <div class="flex items-center gap-4 shrink-0">
              <span class="text-xs text-gray-400">{{ formatDate(coop.createdAt) }}</span>
              <button
                @click="$router.push('/staff/requests')"
                class="text-sm font-semibold text-primary-600 hover:text-primary-800 transition-colors"
              >
                ตรวจสอบ
              </button>
            </div>
          </div>

          <div v-if="pendingRequests.length === 0" class="py-10 text-center">
            <p class="text-sm text-gray-400">ไม่มีคำขอที่รอตรวจสอบ</p>
          </div>
        </div>
      </div>

    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'
import { FileText, Clock, CheckCircle, XCircle } from 'lucide-vue-next'
import { mockAllCooperatives } from '@/mock/cooperatives'

const list = mockAllCooperatives

const stats = computed(() => ({
  total:    list.length,
  pending:  list.filter(c => c.status === 'pending').length,
  approved: list.filter(c => c.status === 'approved').length,
  rejected: list.filter(c => c.status === 'rejected').length,
}))

const pendingRequests = computed(() => list.filter(c => c.status === 'pending'))

const SEGMENT_COLORS = {
  pending:  '#f59e0b',
  approved: '#10b981',
  rejected: '#ef4444',
}

const donutSegments = computed(() => {
  const total = stats.value.total
  if (total === 0) return []
  const CIRC = 100
  let accumulated = 0
  return ['pending', 'approved', 'rejected']
    .map(key => {
      const pct    = (stats.value[key] / total) * CIRC
      const offset = CIRC * 0.25 - accumulated
      accumulated += pct
      return { label: key, pct, offset, color: SEGMENT_COLORS[key] }
    })
    .filter(s => s.pct > 0)
})

function formatDate(dateStr) {
  if (!dateStr) return ''
  return new Date(dateStr).toLocaleDateString('th-TH', { day: 'numeric', month: 'short', year: 'numeric' })
}
</script>
