<template>
  <div class="bg-white rounded-2xl shadow-sm border border-gray-100 p-6">
    <slot />
    <div v-if="$slots.footer" class="mt-6 pt-6 border-t border-gray-100">
      <slot name="footer" />
    </div>
  </div>
</template>
