<template>
  <div>
    <div class="heading">Data Utility</div>
    <div class="status">{{ state.status }}</div>
    <Button
      label="Reset Tables"
      @click="resetTables"
      class="p-button-outlined"
    />
  </div>
</template>
<script>
import { reactive } from "vue";

export default {
  setup() {
    let state = reactive({
      status: "",
    });
    const resetTables = async () => {
      let url = "https://localhost:44368/api/Data";
      try {
        state.status = "resetting tables ...";
        let response = await fetch(`${url}`);
        state.status = await response.json();
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };
    return {
      resetTables,
      state,
    };
  },
};
</script>