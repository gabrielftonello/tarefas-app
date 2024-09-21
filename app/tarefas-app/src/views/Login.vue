<!-- src/views/Login.vue -->
<template>
  <DefaultLayout>
    <Login />
  </DefaultLayout>
</template>

<script>
import DefaultLayout from '@/layouts/DefaultLayout.vue';
import Login from '@/components/Auth/Login.vue';

export default {
  components: {
    DefaultLayout,
    Login,
  },
};
</script>

<style scoped>
</style>
