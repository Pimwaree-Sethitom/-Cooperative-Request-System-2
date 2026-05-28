<template>
  <div class="p-8">
    <!-- Stats -->
    <div class="grid grid-cols-4 gap-4 mb-6">
      <div class="bg-white rounded-xl border border-gray-200 p-4">
        <div class="flex items-center gap-3">
          <div class="w-9 h-9 rounded-lg bg-gray-100 flex items-center justify-center">
            <FileText class="w-4 h-4 text-gray-500" />
          </div>
          <div>
            <p class="text-xs text-gray-400">Total Requests</p>
            <p class="text-xl font-bold text-gray-900">{{ stats.total }}</p>
          </div>
        </div>
      </div>

      <div class="bg-yellow-50 rounded-xl border border-yellow-200 p-4">
        <div class="flex items-center gap-3">
          <div class="w-9 h-9 rounded-lg bg-yellow-100 flex items-center justify-center">
            <Clock class="w-4 h-4 text-yellow-500" />
          </div>
          <div>
            <p class="text-xs text-yellow-600">Pending</p>
            <p class="text-xl font-bold text-gray-900">{{ stats.pending }}</p>
          </div>
        </div>
      </div>

      <div class="bg-green-50 rounded-xl border border-green-200 p-4">
        <div class="flex items-center gap-3">
          <div class="w-9 h-9 rounded-lg bg-green-100 flex items-center justify-center">
            <CheckCircle class="w-4 h-4 text-green-500" />
          </div>
          <div>
            <p class="text-xs text-green-600">Approved</p>
            <p class="text-xl font-bold text-gray-900">{{ stats.approved }}</p>
          </div>
        </div>
      </div>

      <div class="bg-red-50 rounded-xl border border-red-200 p-4">
        <div class="flex items-center gap-3">
          <div class="w-9 h-9 rounded-lg bg-red-100 flex items-center justify-center">
            <XCircle class="w-4 h-4 text-red-500" />
          </div>
          <div>
            <p class="text-xs text-red-600">Rejected</p>
            <p class="text-xl font-bold text-gray-900">{{ stats.rejected }}</p>
          </div>
        </div>
      </div>
    </div>

    <div class="grid grid-cols-3 gap-4 mb-6">
      <div class="col-span-2 bg-white rounded-xl border border-gray-200 p-8 flex flex-col items-center justify-center text-center">
        <div class="w-12 h-12 rounded-full bg-primary-50 flex items-center justify-center mb-4">
          <PlusCircle class="w-6 h-6 text-primary-600" />
        </div>
        <h2 class="text-base font-bold text-gray-900 mb-2">Register a New Cooperative</h2>
        <p class="text-sm text-gray-400 mb-5 max-w-sm">
          Start the registration process for your cooperative. Our staff will review your application within 5-7 business days.
        </p>
        <RouterLink
          to="/new-request"
          class="inline-flex items-center gap-2 px-4 py-2 bg-primary-600 text-white text-sm font-medium rounded-lg hover:bg-primary-700 transition-colors"
        >
          <PlusCircle class="w-4 h-4" />
          Create New Request
        </RouterLink>
      </div>

      <div class="bg-white rounded-xl border border-gray-200 p-5">
        <h2 class="text-sm font-bold text-gray-900 mb-4">Quick Links</h2>
        <div class="space-y-3">
          <RouterLink to="/my-requests" class="flex items-center gap-3 p-3 rounded-lg hover:bg-gray-50 transition-colors group">
            <div class="w-8 h-8 rounded-lg bg-blue-50 flex items-center justify-center">
              <FileText class="w-4 h-4 text-blue-500" />
            </div>
            <div>
              <p class="text-sm font-medium text-gray-700 group-hover:text-gray-900">My Requests</p>
              <p class="text-xs text-gray-400">View all your submissions</p>
            </div>
          </RouterLink>
          <RouterLink to="/new-request" class="flex items-center gap-3 p-3 rounded-lg hover:bg-gray-50 transition-colors group">
            <div class="w-8 h-8 rounded-lg bg-primary-50 flex items-center justify-center">
              <PlusCircle class="w-4 h-4 text-primary-600" />
            </div>
            <div>
              <p class="text-sm font-medium text-gray-700 group-hover:text-gray-900">New Request</p>
              <p class="text-xs text-gray-400">Register a cooperative</p>
            </div>
          </RouterLink>
        </div>
      </div>
    </div>

    <!-- Recent Requests -->
    <div class="bg-white rounded-xl border border-gray-200 p-5">
      <div class="flex items-center justify-between mb-4">
        <div>
          <h2 class="text-sm font-bold text-gray-900">Recent Requests</h2>
          <p class="text-xs text-gray-400">Your latest cooperative registration requests</p>
        </div>
        <RouterLink to="/my-requests" class="text-sm text-primary-600 hover:text-primary-700 font-medium">
          View All
        </RouterLink>
      </div>

      <div class="divide-y divide-gray-50">
        <div
          v-for="coop in mockCooperatives.slice(0, 5)"
          :key="coop.id"
          class="flex items-center justify-between py-3"
        >
          <div>
            <p class="text-sm font-medium text-gray-800">{{ coop.name }}</p>
            <p class="text-xs text-gray-400">{{ coop.members.length }} members</p>
          </div>
          <div class="flex items-center gap-3">
            <span :class="statusClass(coop.status)" class="text-xs font-medium px-2 py-0.5 rounded-full">
              {{ statusLabel(coop.status) }}
            </span>
            <span class="text-xs text-gray-400">{{ formatDate(coop.createdAt) }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'
import { FileText, Clock, CheckCircle, XCircle, PlusCircle } from 'lucide-vue-next'
import { useAuthStore } from '@/stores/auth'
import { mockCooperatives } from '@/mock/cooperatives'

const authStore = useAuthStore()

const stats = computed(() => ({
  total: mockCooperatives.length,
  pending: mockCooperatives.filter(c => c.status === 'pending').length,
  approved: mockCooperatives.filter(c => c.status === 'approved').length,
  rejected: mockCooperatives.filter(c => c.status === 'rejected').length,
}))

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
