<template>
  <div class="p-8 space-y-5">

    <!-- Search + filter row -->
    <div class="flex gap-3">
      <div class="relative flex-1">
        <Search class="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400 pointer-events-none" />
        <input
          v-model="search"
          type="text"
          placeholder="ค้นหาด้วยชื่อสหกรณ์หรือผู้ยื่นคำขอ..."
          class="w-full pl-11 pr-4 py-3 rounded-xl border border-gray-200 bg-white text-sm text-gray-900 placeholder-gray-300 focus:outline-none focus:ring-2 focus:ring-primary-300 focus:border-primary-300 transition"
        />
      </div>
      <select
        v-model="statusFilter"
        class="px-4 py-3 rounded-xl border border-gray-200 bg-white text-sm text-gray-700 focus:outline-none focus:ring-2 focus:ring-primary-300 focus:border-primary-300 transition appearance-none pr-9 cursor-pointer min-w-[160px]"
      >
        <option value="all">สถานะทั้งหมด</option>
        <option value="pending">รอดำเนินการ</option>
        <option value="approved">อนุมัติแล้ว</option>
        <option value="rejected">ถูกปฏิเสธ</option>
      </select>
    </div>

    <!-- Table -->
    <div class="bg-white rounded-2xl border border-gray-200 overflow-hidden">

      <div v-if="filteredList.length === 0" class="py-20 text-center">
        <div class="w-12 h-12 rounded-2xl bg-primary-50 flex items-center justify-center mx-auto mb-4">
          <FileText class="w-5 h-5 text-primary-400" />
        </div>
        <p class="text-sm font-semibold text-gray-500">ไม่พบคำขอ</p>
      </div>

      <div v-else>
        <!-- Header -->
        <div class="grid grid-cols-12 gap-4 px-5 py-3 border-b border-gray-200 text-[11px] font-semibold text-gray-400 uppercase tracking-widest">
          <div class="col-span-4">สหกรณ์</div>
          <div class="col-span-2">ผู้ยื่นคำขอ</div>
          <div class="col-span-1 text-center">สมาชิก</div>
          <div class="col-span-2 text-center">สถานะ</div>
          <div class="col-span-2">วันที่</div>
          <div class="col-span-1 text-right">การดำเนินการ</div>
        </div>

        <!-- Data rows -->
        <div
          v-for="coop in pagedList"
          :key="coop.id"
          class="grid grid-cols-12 gap-4 px-5 py-4 border-b border-gray-200 last:border-0 items-center hover:bg-primary-50/30 transition-colors"
        >
          <div class="col-span-4 cursor-pointer" @click="selected = coop">
            <p class="text-sm font-bold text-gray-900">{{ coop.name }}</p>
            <p class="text-xs text-gray-400 mt-0.5">{{ coop.city }}</p>
          </div>
          <div class="col-span-2">
            <p class="text-sm text-gray-600">{{ coop.applicant }}</p>
          </div>
          <div class="col-span-1 text-center">
            <span class="text-sm text-gray-600">{{ coop.members.length }}</span>
          </div>
          <div class="col-span-2 flex justify-center">
            <span :class="statusClass(coop.status)" class="text-xs font-semibold px-2.5 py-1 rounded-full">
              {{ statusLabel(coop.status) }}
            </span>
          </div>
          <div class="col-span-2">
            <span class="text-xs text-gray-500">{{ formatDate(coop.createdAt) }}</span>
          </div>
          <div class="col-span-1 flex justify-end gap-2">
            <button @click="selected = coop" class="text-xs font-semibold text-gray-400 hover:text-gray-700 transition-colors">
              ดู
            </button>
            <button
              v-if="coop.status === 'pending'"
              @click="openReview(coop)"
              class="text-xs font-semibold text-primary-600 hover:text-primary-800 transition-colors"
            >
              ตรวจสอบ
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Pagination -->
    <div v-if="filteredList.length > 0" class="flex items-center justify-between">
      <p class="text-sm text-gray-500">
        แสดง {{ rangeStart }}–{{ rangeEnd }} จาก {{ filteredList.length }} รายการ
      </p>
      <div class="flex items-center gap-1">
        <button
          @click="currentPage--"
          :disabled="currentPage === 1"
          class="px-3 py-1.5 text-sm font-medium rounded-lg border transition-colors"
          :class="currentPage === 1 ? 'border-gray-100 text-gray-300 cursor-not-allowed' : 'border-gray-200 text-gray-600 hover:bg-gray-50'"
        >
          ก่อนหน้า
        </button>
        <button
          v-for="p in totalPages"
          :key="p"
          @click="currentPage = p"
          class="w-8 h-8 text-sm font-semibold rounded-lg transition-colors"
          :class="currentPage === p ? 'bg-primary-600 text-white' : 'text-gray-600 hover:bg-gray-50'"
        >
          {{ p }}
        </button>
        <button
          @click="currentPage++"
          :disabled="currentPage === totalPages"
          class="px-3 py-1.5 text-sm font-medium rounded-lg border transition-colors"
          :class="currentPage === totalPages ? 'border-gray-100 text-gray-300 cursor-not-allowed' : 'border-gray-200 text-gray-600 hover:bg-gray-50'"
        >
          ถัดไป
        </button>
      </div>
    </div>

    <!-- Detail modal -->
    <Teleport to="body">
      <div
        v-if="selected && !reviewing"
        class="fixed inset-0 bg-black/40 flex items-center justify-center z-50 p-4"
        @click.self="selected = null"
      >
        <div class="bg-white rounded-3xl shadow-xl w-full max-w-lg max-h-[82vh] flex flex-col overflow-hidden">
          <div class="bg-block-mint px-6 py-5 flex items-start justify-between">
            <div>
              <p class="text-[11px] font-semibold text-primary-700 uppercase tracking-widest mb-1">รายละเอียดคำขอ</p>
              <h2 class="text-lg font-bold text-gray-900 tracking-tight">{{ selected.name }}</h2>
              <p class="text-xs text-gray-500 mt-0.5">{{ selected.applicant }} · {{ selected.city }}</p>
              <span :class="statusClass(selected.status)" class="text-xs font-semibold px-2.5 py-1 rounded-full inline-block mt-2">
                {{ statusLabel(selected.status) }}
              </span>
            </div>
            <button @click="selected = null" class="text-gray-400 hover:text-gray-700 transition-colors mt-1">
              <X class="w-5 h-5" />
            </button>
          </div>

          <div class="overflow-y-auto flex-1 p-6 space-y-5">
            <div v-if="selected.description">
              <p class="text-[11px] font-semibold text-gray-400 uppercase tracking-widest mb-1.5">รายละเอียด</p>
              <p class="text-sm text-gray-700 leading-relaxed">{{ selected.description }}</p>
            </div>
            <div v-if="selected.staffNote" class="bg-amber-50 border border-amber-100 rounded-2xl p-4">
              <p class="text-[11px] font-semibold text-amber-700 uppercase tracking-widest mb-1.5">บันทึกเจ้าหน้าที่</p>
              <p class="text-sm text-amber-800">{{ selected.staffNote }}</p>
            </div>
            <div>
              <p class="text-[11px] font-semibold text-gray-400 uppercase tracking-widest mb-3">
                สมาชิก ({{ selected.members.length }} คน)
              </p>
              <div class="space-y-2">
                <div
                  v-for="(m, i) in selected.members.slice(0, 10)"
                  :key="m.id"
                  class="flex items-center gap-3 p-3 rounded-xl bg-primary-50"
                >
                  <span class="w-6 h-6 rounded-full bg-primary-600 text-white text-xs font-bold flex items-center justify-center shrink-0">
                    {{ i + 1 }}
                  </span>
                  <div class="flex-1 min-w-0">
                    <p class="text-sm font-semibold text-gray-800">{{ m.fullName }}</p>
                    <p class="text-xs text-gray-400">{{ [m.nationalId, m.phone].filter(Boolean).join(' · ') }}</p>
                  </div>
                </div>
                <p v-if="selected.members.length > 10" class="text-xs text-gray-400 text-center pt-1">
                  + อีก {{ selected.members.length - 10 }} คน
                </p>
              </div>
            </div>
            <div class="text-xs text-gray-400 space-y-1 pt-4 border-t border-gray-200">
              <p>วันที่ยื่น: {{ formatDate(selected.createdAt) }}</p>
              <p v-if="selected.reviewedAt">วันที่ตรวจสอบ: {{ formatDate(selected.reviewedAt) }}</p>
            </div>
          </div>

          <div v-if="selected.status === 'pending'" class="p-5 border-t border-gray-200 flex justify-end">
            <button
              @click="openReview(selected); selected = null"
              class="px-6 py-2.5 text-sm font-semibold text-white bg-primary-600 rounded-full hover:bg-primary-700 transition-colors"
            >
              ตรวจสอบคำขอนี้
            </button>
          </div>
        </div>
      </div>
    </Teleport>

    <!-- Review modal -->
    <Teleport to="body">
      <div
        v-if="reviewing"
        class="fixed inset-0 bg-black/40 flex items-center justify-center z-50 p-4"
        @click.self="cancelReview"
      >
        <div class="bg-white rounded-3xl shadow-xl w-full max-w-md overflow-hidden">
          <div class="bg-block-mint px-6 py-5 flex items-center justify-between">
            <div>
              <p class="text-[11px] font-semibold text-primary-700 uppercase tracking-widest mb-1">การตรวจสอบของเจ้าหน้าที่</p>
              <h2 class="text-lg font-bold text-gray-900 tracking-tight">{{ reviewing.name }}</h2>
            </div>
            <button @click="cancelReview" class="text-gray-400 hover:text-gray-700 transition-colors">
              <X class="w-5 h-5" />
            </button>
          </div>

          <div class="p-6 space-y-5">
            <div>
              <p class="text-[11px] font-semibold text-gray-400 uppercase tracking-widest mb-3">การตัดสินใจ</p>
              <div class="grid grid-cols-2 gap-2">
                <button
                  @click="reviewForm.action = 'approve'"
                  :class="reviewForm.action === 'approve'
                    ? 'bg-primary-600 text-white border-primary-600'
                    : 'bg-white text-gray-600 border-gray-200 hover:border-primary-300 hover:bg-primary-50'"
                  class="py-3 text-sm font-semibold rounded-full border transition-all flex items-center justify-center gap-2"
                >
                  <CheckCircle class="w-4 h-4" />
                  อนุมัติ
                </button>
                <button
                  @click="reviewForm.action = 'reject'"
                  :class="reviewForm.action === 'reject'
                    ? 'bg-red-600 text-white border-red-600'
                    : 'bg-white text-gray-600 border-gray-200 hover:border-red-300 hover:bg-red-50'"
                  class="py-3 text-sm font-semibold rounded-full border transition-all flex items-center justify-center gap-2"
                >
                  <XCircle class="w-4 h-4" />
                  ปฏิเสธ
                </button>
              </div>
            </div>

            <div>
              <label class="block text-[11px] font-semibold text-gray-400 uppercase tracking-widest mb-1.5">
                บันทึกเจ้าหน้าที่
                <span class="normal-case font-normal text-gray-300 tracking-normal ml-1">(ไม่บังคับ)</span>
              </label>
              <textarea
                v-model="reviewForm.note"
                rows="3"
                placeholder="เพิ่มหมายเหตุสำหรับผู้ยื่นคำขอ..."
                class="w-full px-4 py-3 rounded-xl border border-gray-200 bg-white text-sm text-gray-900 placeholder-gray-300 focus:outline-none focus:ring-2 focus:ring-primary-300 focus:border-primary-300 transition resize-none"
              />
            </div>
          </div>

          <div class="px-6 pb-6 flex justify-end gap-3">
            <button
              @click="cancelReview"
              class="px-5 py-2.5 text-sm font-semibold text-gray-500 hover:text-gray-900 rounded-full hover:bg-gray-100 transition-colors"
            >
              ยกเลิก
            </button>
            <button
              @click="submitReview"
              :disabled="!reviewForm.action || submitting"
              class="inline-flex items-center gap-2 px-6 py-2.5 text-sm font-semibold text-white rounded-full transition-all disabled:opacity-50 disabled:cursor-not-allowed"
              :class="reviewForm.action === 'reject' ? 'bg-red-600 hover:bg-red-700' : 'bg-primary-600 hover:bg-primary-700'"
            >
              <span v-if="submitting" class="w-4 h-4 border-2 border-white/30 border-t-white rounded-full animate-spin" />
              {{ submitting ? 'กำลังส่ง...' : reviewForm.action === 'reject' ? 'ปฏิเสธคำขอ' : 'อนุมัติคำขอ' }}
            </button>
          </div>
        </div>
      </div>
    </Teleport>

  </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue'
import { FileText, CheckCircle, XCircle, X, Search } from 'lucide-vue-next'
import { mockAllCooperatives } from '@/mock/cooperatives'

const PER_PAGE = 5

const list         = ref(mockAllCooperatives.map(c => ({ ...c })))
const search       = ref('')
const statusFilter = ref('all')
const currentPage  = ref(1)
const selected     = ref(null)
const reviewing    = ref(null)
const submitting   = ref(false)
const reviewForm   = ref({ action: '', note: '' })

watch([search, statusFilter], () => { currentPage.value = 1 })

const filteredList = computed(() => {
  const q  = search.value.toLowerCase()
  const sf = statusFilter.value
  return list.value.filter(c => {
    const matchesSearch = !q || c.name.toLowerCase().includes(q) || c.applicant.toLowerCase().includes(q)
    const matchesStatus = sf === 'all' || c.status === sf
    return matchesSearch && matchesStatus
  })
})

const totalPages = computed(() => Math.max(1, Math.ceil(filteredList.value.length / PER_PAGE)))
const rangeStart = computed(() => (currentPage.value - 1) * PER_PAGE + 1)
const rangeEnd   = computed(() => Math.min(currentPage.value * PER_PAGE, filteredList.value.length))
const pagedList  = computed(() =>
  filteredList.value.slice((currentPage.value - 1) * PER_PAGE, currentPage.value * PER_PAGE)
)

function openReview(coop) {
  reviewing.value  = coop
  reviewForm.value = { action: '', note: '' }
}

function cancelReview() { reviewing.value = null }

async function submitReview() {
  if (!reviewForm.value.action) return
  submitting.value = true
  await new Promise(r => setTimeout(r, 600))
  const target = list.value.find(c => c.id === reviewing.value.id)
  if (target) {
    target.status     = reviewForm.value.action === 'approve' ? 'approved' : 'rejected'
    target.staffNote  = reviewForm.value.note.trim() || null
    target.reviewedAt = new Date().toISOString()
  }
  submitting.value = false
  reviewing.value  = null
}

function statusClass(status) {
  if (status === 'approved') return 'bg-primary-100 text-primary-800'
  if (status === 'rejected') return 'bg-red-100 text-red-700'
  return 'bg-amber-100 text-amber-700'
}

function statusLabel(status) {
  if (status === 'approved') return 'อนุมัติแล้ว'
  if (status === 'rejected') return 'ถูกปฏิเสธ'
  return 'รอดำเนินการ'
}

function formatDate(dateStr) {
  if (!dateStr) return ''
  return new Date(dateStr).toLocaleDateString('th-TH', { day: 'numeric', month: 'short', year: 'numeric' })
}
</script>
