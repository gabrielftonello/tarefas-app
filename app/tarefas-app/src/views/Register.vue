<!-- src/views/Register.vue -->
<template>
  <DefaultLayout>
    <Register />
  </DefaultLayout>
</template>

<script>
import DefaultLayout from '@/layouts/DefaultLayout.vue';
import Register from '@/components/Auth/Register.vue';

export default {
  components: {
    DefaultLayout,
    Register,
  },
};
</script>

<style scoped>
/* Add any register view-specific styles here */
</style>
