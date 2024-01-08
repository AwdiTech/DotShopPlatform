<template>
  <div class="container">
    <div class="heading">Logout</div>
    <div class="status">{{ status }}</div>
  </div>
</template>
<script>
import { ref, onMounted } from "vue";
export default {
  setup() {
    let status = ref("goodbye");
    onMounted(() => {
      logout();
    });
    const logout = () => {
      sessionStorage.clear();
    };
    return { status };
  },
};
</script>