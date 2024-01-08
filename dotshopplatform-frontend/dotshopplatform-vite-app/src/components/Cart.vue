<template>
  <div class="container" style="padding-bottom: 10vh">
    <img class="product-image" style="margin-top: 2vh" src="../assets/shoppingcart.jpg" />
    <div class="heading">Cart Contents</div>
    <div class="status">{{ state.status }}</div>
    <div v-if="state.cart.length > 0" id="cart">
      <DataTable :value="state.cart" :scrollable="true" scrollHeight="38vh" dataKey="id" class="p-datatable-striped"
        id="cart-contents">
        <Column header="Name" field="product.productName" />
        <Column header="Qty" field="qty" />
        <Column id="price" header="Price" field="product.fprice" />
        <Column id="extended" header="Extended" field="product.fprice" />
      </DataTable>
      <div class="cartTotals">
        SubTotal: &nbsp;&nbsp;{{ formatCurrency(state.subTotal) }} <br />
        Tax: &nbsp;&nbsp; &nbsp;{{ formatCurrency(state.tax) }}<br />
        Total: &nbsp;&nbsp;{{ formatCurrency(state.total) }}<br />
      </div>
    </div>

    <Button style="margin-top: 2vh" label="Confirm Order" @click="saveCart" class="p-button-outlined margin-button1" />
    &nbsp;
    <Button style="margin-top: 2vh" label="Clear Cart" @click="clearCart" class="p-button-outlined margin-button1" />
  </div>
</template>

<script>
import { reactive, onMounted } from "vue";
import { poster } from "../util/apiutil";

export default {
  setup() {
    onMounted(() => {
      if (sessionStorage.getItem("cart")) {
        state.cart = JSON.parse(sessionStorage.getItem("cart"));
        state.cart.map((cartProduct) => {
          cartProduct.product.fprice = formatCurrency(
            cartProduct.product.msrp
          );
          state.subTotal += cartProduct.product.msrp * cartProduct.qty;
          state.tax += cartProduct.product.msrp * cartProduct.qty * 0.13;
          state.total += cartProduct.product.msrp * cartProduct.qty * 1.13;
        });
      } else {
        state.cart = [];
      }
    });

    let state = reactive({
      status: "",
      subTotal: 0,
      tax: 0,
      total: 0,
      cart: [],
    });

    const clearCart = () => {
      sessionStorage.removeItem("cart");
      state.cart = [];
      state.status = "cart cleared";
    };

    const formatCurrency = (value) => {
      return value.toLocaleString("en-US", {
        style: "currency",
        currency: "USD",
      });
    };

    const saveCart = async () => {
      let customer = JSON.parse(sessionStorage.getItem("customer"));
      let cart = JSON.parse(sessionStorage.getItem("cart"));
      if (!cart) {return;}
      try {
        state.status = "Sending cart info to server";
        let cartHelper = { email: customer.email, selections: cart};
        let response = await poster("order", cartHelper);
        if (response.message.indexOf("not") > 0) {
          state.status = response.message;
        } else {
          clearCart();
          state.status = "Order Sent!";
        }
      } catch (err) {
        console.error(err);
        state.status = `Error saving Cart: ${err.message}`;
      }
    };


    return {
      state,
      clearCart,
      saveCart,
      formatCurrency,
    };
  },
};
</script>   

<style>
#cartcontents th:nth-child(2) {
  margin-left: 45vw;
}
</style>