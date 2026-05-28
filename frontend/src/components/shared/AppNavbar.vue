<template>
  <header class="h-[72px] bg-white border-b border-gray-200 px-8 flex items-center justify-between">
    <div>
      <h1 class="text-lg font-bold text-gray-900">{{ title }}</h1>
      <p class="text-sm text-gray-400 mt-0.5">{{ subtitle }}</p>
    </div>
  </header>
</template>

<script setup>
defineProps({
  title: String,
  subtitle: String,
})
</script>
