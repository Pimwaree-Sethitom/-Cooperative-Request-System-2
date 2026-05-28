<template>
  <div class="p-8 max-w-3xl">
    <div class="mb-6">
      <RouterLink to="/dashboard" class="text-sm text-primary-600 hover:text-primary-700 flex items-center gap-1">
        <ChevronLeft class="w-4 h-4" />
        Back to Dashboard
      </RouterLink>
    </div>

    <form @submit.prevent="submit" class="space-y-5">
      <!-- Basic Info -->
      <div class="bg-white rounded-xl border border-gray-200 p-6 space-y-4">
        <h2 class="text-sm font-bold text-gray-900">Cooperative Information</h2>
        <div>
          <label class="block text-sm text-gray-500 mb-1.5">Cooperative Name <span class="text-red-500">*</span></label>
          <input
            v-model="form.name"
            type="text"
            placeholder="e.g. Green Valley Farmers Cooperative"
            class="w-full px-3 py-2.5 bg-gray-50 border border-gray-200 rounded-lg text-sm text-gray-900 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent transition"
            required
          />
        </div>
        <div>
          <label class="block text-sm text-gray-500 mb-1.5">Description <span class="text-gray-400 font-normal text-xs">(optional)</span></label>
          <textarea
            v-model="form.description"
            rows="3"
            placeholder="Brief description of the cooperative's purpose..."
            class="w-full px-3 py-2.5 bg-gray-50 border border-gray-200 rounded-lg text-sm text-gray-900 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent transition resize-none"
          />
        </div>
      </div>

      <!-- Members -->
      <div class="bg-white rounded-xl border border-gray-200 p-6">
        <div class="flex items-center justify-between mb-4">
          <div>
            <h2 class="text-sm font-bold text-gray-900">Founding Members</h2>
            <p class="text-xs text-gray-400 mt-0.5">
              {{ form.members.length }} member{{ form.members.length !== 1 ? 's' : '' }}
              <span :class="form.members.length >= 10 ? 'text-green-600' : 'text-red-500'">
                (min. 10 required)
              </span>
            </p>
          </div>
          <button
            type="button"
            @click="addMember"
            class="flex items-center gap-1.5 px-3 py-1.5 text-xs font-medium text-primary-700 bg-primary-50 rounded-lg hover:bg-primary-100 transition-colors"
          >
            <Plus class="w-3.5 h-3.5" />
            Add Member
          </button>
        </div>

        <div class="space-y-3">
          <div
            v-for="(member, i) in form.members"
            :key="i"
            class="grid grid-cols-12 gap-2 items-start"
          >
            <div class="col-span-1 flex items-center justify-center h-10">
              <span class="w-6 h-6 rounded-full bg-gray-100 text-gray-500 text-xs font-medium flex items-center justify-center">
                {{ i + 1 }}
              </span>
            </div>
            <div class="col-span-4">
              <input
                v-model="member.fullName"
                type="text"
                placeholder="Full name *"
                class="w-full px-3 py-2 bg-gray-50 border border-gray-200 rounded-lg text-sm text-gray-900 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent transition"
                required
              />
            </div>
            <div class="col-span-3">
              <input
                v-model="member.nationalId"
                type="text"
                placeholder="National ID"
                class="w-full px-3 py-2 bg-gray-50 border border-gray-200 rounded-lg text-sm text-gray-900 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent transition"
              />
            </div>
            <div class="col-span-3">
              <input
                v-model="member.phone"
                type="text"
                placeholder="Phone"
                class="w-full px-3 py-2 bg-gray-50 border border-gray-200 rounded-lg text-sm text-gray-900 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent transition"
              />
            </div>
            <div class="col-span-1 flex items-center justify-center h-10">
              <button
                v-if="form.members.length > 1"
                type="button"
                @click="removeMember(i)"
                class="text-gray-300 hover:text-red-400 transition-colors"
              >
                <X class="w-4 h-4" />
              </button>
            </div>
          </div>
        </div>

        <button
          type="button"
          @click="addMember"
          class="mt-4 w-full py-2.5 border border-dashed border-gray-200 rounded-lg text-sm text-gray-400 hover:border-primary-300 hover:text-primary-600 transition-colors flex items-center justify-center gap-2"
        >
          <Plus class="w-4 h-4" />
          Add another member
        </button>
      </div>

      <div v-if="error" class="bg-red-50 border border-red-200 rounded-lg px-4 py-3 text-sm text-red-600">
        {{ error }}
      </div>

      <div class="flex items-center justify-end gap-3">
        <RouterLink to="/dashboard" class="px-4 py-2 text-sm font-medium text-gray-600 hover:text-gray-900 transition-colors">
          Cancel
        </RouterLink>
        <button
          type="submit"
          :disabled="submitting"
          class="flex items-center gap-2 px-5 py-2.5 bg-primary-600 text-white text-sm font-medium rounded-lg hover:bg-primary-700 disabled:opacity-60 disabled:cursor-not-allowed transition-colors"
        >
          <span v-if="submitting" class="w-4 h-4 border-2 border-white/40 border-t-white rounded-full animate-spin" />
          {{ submitting ? 'Submitting...' : 'Submit Request' }}
        </button>
      </div>
    </form>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { ChevronLeft, Plus, X } from 'lucide-vue-next'

const router = useRouter()
const submitting = ref(false)
const error = ref('')

const form = ref({
  name: '',
  description: '',
  members: Array.from({ length: 10 }, () => ({ fullName: '', nationalId: '', phone: '' }))
})

function addMember() {
  form.value.members.push({ fullName: '', nationalId: '', phone: '' })
}

function removeMember(i) {
  form.value.members.splice(i, 1)
}

async function submit() {
  error.value = ''
  const filled = form.value.members.filter(m => m.fullName.trim())
  if (filled.length < 10) {
    error.value = 'At least 10 members with full names are required.'
    return
  }
  submitting.value = true
  await new Promise(r => setTimeout(r, 800))
  submitting.value = false
  router.push('/my-requests')
}
</script>
