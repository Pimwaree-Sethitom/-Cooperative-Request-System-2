<template>
  <div>
    <label v-if="label" :for="id" class="block text-sm font-normal text-gray-500 mb-1">
      {{ label }} <span v-if="required" class="text-red-500">*</span>
    </label>
    <input
      :id="id"
      :type="type"
      :value="modelValue"
      :placeholder="placeholder"
      :required="required"
      class="w-full px-4 py-3 rounded-lg border bg-gray-100 text-sm text-gray-900 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent transition"
      :class="error ? 'border-red-400' : 'border-transparent'"
      @input="$emit('update:modelValue', $event.target.value)"
    />
    <p v-if="error" class="mt-1 text-xs text-red-500">{{ error }}</p>
  </div>
</template>

<script setup>
defineProps({
  id: String,
  modelValue: String,
  type: { type: String, default: 'text' },
  label: String,
  placeholder: String,
  error: String,
  required: Boolean,
})

defineEmits(['update:modelValue'])
</script>
