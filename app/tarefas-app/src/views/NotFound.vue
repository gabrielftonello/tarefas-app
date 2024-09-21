<!-- src/views/NotFound.vue -->
<template>
  <DefaultLayout>
    <v-container class="text-center">
      <h1>404 - Page Not Found</h1>
      <p>The page you are looking for does not exist.</p>
      <v-btn color="primary" to="/">Go Home</v-btn>
    </v-container>
  </DefaultLayout>
</template>

<script>
import DefaultLayout from '@/layouts/DefaultLayout.vue';

export default {
  components: {
    DefaultLayout,
  },
};
</script>

<style scoped>
/* Add any not found view-specific styles here */
</style>
