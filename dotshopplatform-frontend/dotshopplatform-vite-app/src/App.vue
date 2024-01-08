<template>
  <div>
    <Toolbar>
      <template #left>
        <span style="font-weight: bold; font-size: larger"
          >DotShopPlatform</span
        >
      </template>
      <template #right>
        <Menu id="overlay_menu" ref="menu" :model="menuproducts" :popup="true" />
        <Button icon="pi pi-bars" @click="toggle" />
      </template>
    </Toolbar>
    <router-view />
    <div class="footer">
      AwdiTech &copy;{{
        new Date().getFullYear()
      }}
    </div>
  </div>
</template>

<script>
import { ref } from "vue";
export default {
  setup() {
    const menuproducts = ref([
      { label: "Home", to: "/" },
      { label: "Brands", to: "/brands" },
      { label: "Register", to: "/register" },
      { label: "Login", to: "/login" },
      { label: "Logout", to: "/logout"},

    ]);
    const menu = ref();
    const toggle = (event) => {
      menu.value.toggle(event);
    };
    return {
      menuproducts,
      menu,
      toggle,
    };
  },
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #43502c;
}
</style>