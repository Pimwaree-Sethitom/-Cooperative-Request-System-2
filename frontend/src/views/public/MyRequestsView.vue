<template>
  <div class="p-8">
    <!-- Filter tabs -->
    <div class="flex gap-1 mb-5 bg-gray-100 p-1 rounded-lg w-fit">
      <button
        v-for="tab in tabs"
        :key="tab.value"
        @click="activeTab = tab.value"
        class="px-4 py-1.5 text-sm font-medium rounded-md transition-colors"
        :class="activeTab === tab.value
          ? 'bg-white text-gray-900 shadow-sm'
          : 'text-gray-500 hover:text-gray-700'"
      >
        {{ tab.label }}
        <span class="ml-1.5 text-xs">({{ tabCount(tab.value) }})</span>
      </button>
    </div>

    <div class="bg-white rounded-xl border border-gray-200">
      <div v-if="filtered.length === 0" class="py-16 text-center">
        <FileText class="w-8 h-8 text-gray-300 mx-auto mb-3" />
        <p class="text-sm text-gray-400">No requests found</p>
        <RouterLink
          to="/new-request"
          class="mt-4 inline-flex items-center gap-2 px-4 py-2 bg-primary-600 text-white text-sm font-medium rounded-lg hover:bg-primary-700 transition-colors"
        >
          <PlusCircle class="w-4 h-4" />
          Create New Request
        </RouterLink>
      </div>

      <div v-else>
        <div class="grid grid-cols-12 gap-4 px-5 py-3 border-b border-gray-100 text-xs font-medium text-gray-400 uppercase tracking-wide">
          <div class="col-span-4">Cooperative Name</div>
          <div class="col-span-2 text-center">Members</div>
          <div class="col-span-2 text-center">Status</div>
          <div class="col-span-2">Submitted</div>
          <div class="col-span-2">Reviewed</div>
        </div>

        <div
          v-for="coop in filtered"
          :key="coop.id"
          class="grid grid-cols-12 gap-4 px-5 py-4 border-b border-gray-50 last:border-0 items-center hover:bg-gray-50 transition-colors cursor-pointer"
          @click="selected = coop"
        >
          <div class="col-span-4">
            <p class="text-sm font-medium text-gray-800">{{ coop.name }}</p>
            <p class="text-xs text-gray-400 mt-0.5 line-clamp-1">{{ coop.description || '—' }}</p>
          </div>
          <div class="col-span-2 text-center">
            <span class="text-sm text-gray-600">{{ coop.members.length }}</span>
          </div>
          <div class="col-span-2 flex justify-center">
            <span :class="statusClass(coop.status)" class="text-xs font-medium px-2.5 py-0.5 rounded-full">
              {{ statusLabel(coop.status) }}
            </span>
          </div>
          <div class="col-span-2">
            <span class="text-xs text-gray-500">{{ formatDate(coop.createdAt) }}</span>
          </div>
          <div class="col-span-2">
            <span class="text-xs text-gray-500">{{ coop.reviewedAt ? formatDate(coop.reviewedAt) : '—' }}</span>
          </div>
        </div>
      </div>
    </div>

    <!-- Detail modal -->
    <div v-if="selected" class="fixed inset-0 bg-black/40 flex items-center justify-center z-50 p-4" @click.self="selected = null">
      <div class="bg-white rounded-2xl shadow-xl w-full max-w-lg max-h-[80vh] flex flex-col">
        <div class="flex items-center justify-between p-6 border-b border-gray-100">
          <div>
            <h2 class="text-base font-bold text-gray-900">{{ selected.name }}</h2>
            <span :class="statusClass(selected.status)" class="text-xs font-medium px-2 py-0.5 rounded-full mt-1 inline-block">
              {{ statusLabel(selected.status) }}
            </span>
          </div>
          <button @click="selected = null" class="text-gray-400 hover:text-gray-600 transition-colors">
            <X class="w-5 h-5" />
          </button>
        </div>

        <div class="overflow-y-auto flex-1 p-6 space-y-4">
          <div v-if="selected.description">
            <p class="text-xs font-medium text-gray-400 mb-1">Description</p>
            <p class="text-sm text-gray-700">{{ selected.description }}</p>
          </div>

          <div v-if="selected.staffNote" class="bg-yellow-50 border border-yellow-200 rounded-lg p-3">
            <p class="text-xs font-medium text-yellow-700 mb-1">Staff Note</p>
            <p class="text-sm text-yellow-800">{{ selected.staffNote }}</p>
          </div>

          <div>
            <p class="text-xs font-medium text-gray-400 mb-2">Members ({{ selected.members.length }})</p>
            <div class="space-y-1.5">
              <div
                v-for="(m, i) in selected.members"
                :key="m.id"
                class="flex items-center gap-3 p-2.5 rounded-lg bg-gray-50"
              >
                <span class="w-6 h-6 rounded-full bg-primary-100 text-primary-700 text-xs font-bold flex items-center justify-center shrink-0">
                  {{ i + 1 }}
                </span>
                <div class="flex-1 min-w-0">
                  <p class="text-sm font-medium text-gray-800">{{ m.fullName }}</p>
                  <p class="text-xs text-gray-400">{{ [m.nationalId, m.phone].filter(Boolean).join(' · ') || '—' }}</p>
                </div>
              </div>
            </div>
          </div>

          <div class="text-xs text-gray-400 space-y-1 pt-2 border-t border-gray-100">
            <p>Submitted: {{ formatDate(selected.createdAt) }}</p>
            <p v-if="selected.reviewedAt">Reviewed: {{ formatDate(selected.reviewedAt) }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { FileText, PlusCircle, X } from 'lucide-vue-next'
import { mockCooperatives } from '@/mock/cooperatives'

const activeTab = ref('all')
const selected = ref(null)

const tabs = [
  { label: 'All', value: 'all' },
  { label: 'Pending', value: 'pending' },
  { label: 'Approved', value: 'approved' },
  { label: 'Rejected', value: 'rejected' },
]

const filtered = computed(() => {
  if (activeTab.value === 'all') return mockCooperatives
  return mockCooperatives.filter(c => c.status === activeTab.value)
})

function tabCount(value) {
  if (value === 'all') return mockCooperatives.length
  return mockCooperatives.filter(c => c.status === value).length
}

function statusClass(status) {
  if (status === 'approved') return 'bg-green-100 text-green-700'
  if (status === 'rejected') return 'bg-red-100 text-red-700'
  return 'bg-yellow-100 text-yellow-700'
}

function statusLabel(status) {
  if (status === 'approved') return 'Approved'
  if (status === 'rejected') return 'Rejected'
  return 'Pending'
}

function formatDate(dateStr) {
  if (!dateStr) return ''
  return new Date(dateStr).toLocaleDateString('en-GB', { day: 'numeric', month: 'short', year: 'numeric' })
}
</script>
