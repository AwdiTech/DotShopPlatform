<template>
  <div>
    <div class="heading">Brands</div>
    <div class="status">{{ state.status }}</div>
    <Dropdown
      v-if="state.brands.length > 0"
      v-model="state.selectedBrand"
      style="text-align: left"
      :options="state.brands"
      optionLabel="name"
      optionValue="id"
      placeholder="Select Brand"
      @change="loadProducts"
    />
    <div style="margin-top: 2vh" v-if="state.products.length > 0">
      <DataTable
        :value="state.products"
        :scrollable="true"
        scrollHeight="60vh"
        selectionMode="single"
        class="p-datatable-striped"
        @row-select="onRowSelect"
      >
        <Column style="margin-right: -35vw">
          <template #body="slotProps">
            <img
              :src="`/img/${slotProps.data.graphicName}`"
              :alt="slotProps.data.productName"
              class="product-image"
            />
          </template>
        </Column>
        <Column field="description" header="Select a Product"></Column>
      </DataTable>
      <Dialog v-model:visible="state.productSelected" class="dialog-border">
        <div style="text-align: center">
          <img
            :src="`/img/${state.selectedProduct.graphicName}`"
            :alt="state.selectedProduct.description"
            class="dialog-product-image"
          />
        </div>
        <div
          style="
            font-weight: bold;
            font-size: larger;
            margin-top: 3vh;
            text-align: center;
          "
        >
          {{ state.selectedProduct.productName }} -
          {{ formatCurrency(state.selectedProduct.msrp) }}
        </div>
        <br />
        {{ state.selectedProduct.description }}
        <div style="margin-top: 2vh; text-align: center">
          <span style="margin-left: -10vw; margin-right: 2vw">Qty:</span>
          <span>
            <InputNumber
              id="qty"
              :min="0"
              v-model="state.qty"
              :step="1"
              incrementButtonClass="plus"
              showButtons
              buttonLayout="horizontal"
              decrementButtonIcon="pi pi-minus"
              incrementButtonIcon="pi pi-plus"
            />
          </span>
        </div>

        <div style="text-align: center; margin-top: 2vh">
          <Button
            label="Add To Cart"
            @click="addToCart"
            class="p-button-outlined margin-button1"
          />
          &nbsp;
          <Button
            label="View Cart"
            class="p-button-outlined margin-button1"
            v-if="state.cart.length > 0"
            @click="viewCart"
          />
        </div>

        <div
          style="text-align: center"
          v-if="state.dialogStatus !== ''"
          class="dialog-status"
        >
          {{ state.dialogStatus }}
        </div>
      </Dialog>
    </div>
  </div>
</template>

<script>
import { reactive, onMounted } from "vue";
import { fetcher } from "../util/apiutil";
import { useRouter } from "vue-router";

export default {
  setup() {
    const router = useRouter();

    onMounted(() => {
      loadBrands();
    });
    let state = reactive({
      status: "",
      brands: [],
      products: [],
      selectedBrand: {},
      selectedProduct: {},
      dialogStatus: "",
      productSelected: false,
      qty: 0,
      cart: [],
    });
    const loadBrands = async () => {
      try {
        state.status = "loading Brands...";
        const payload = await fetcher(`Brand`);
        if (payload.error === undefined) {
          state.brands = payload;
          state.status = `loaded ${state.brands.length} brands`;
        } else {
          state.status = payload.error;
        }
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };

    const loadProducts = async () => {
      try {
        state.status = `finding products for brand ${state.selectedBrand}...`;
        let payload = await fetcher(`Product/${state.selectedBrand}`);
        state.products = payload;
        state.status = `loaded ${state.products.length} menu items`;
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
      if (sessionStorage.getItem("cart")) {
        state.cart = JSON.parse(sessionStorage.getItem("cart"));
      }
    };

    const onRowSelect = (event) => {
      try {
        state.selectedProduct = event.data;
        state.dialogStatus = "";
        state.productSelected = true;
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };

    const addToCart = () => {
      const index = state.cart.findIndex(
        // is item already on the tray
        (product) => product.id === state.selectedProduct.id
      );
      if (state.qty !== 0) {
        index === -1 // add
          ? state.cart.push({
              id: state.selectedProduct.id,
              qty: state.qty,
              product: state.selectedProduct,
            })
          : (state.cart[index] = {
              // replace
              id: state.selectedProduct.id,
              qty: state.qty,
              product: state.selectedProduct,
            });
        state.dialogStatus = `${state.qty} item(s) added`;
      } else {
        index === -1 ? null : state.cart.splice(index, 1); // remove
        state.dialogStatus = `item(s) removed`;
      }
      sessionStorage.setItem("cart", JSON.stringify(state.cart));
      state.qty = 0;
    };

    const formatCurrency = (value) => {
      return value.toLocaleString("en-US", {
        style: "currency",
        currency: "USD",
      });
    };

    const viewCart = () => {
      router.push("cart");
    };

    return {
      state,
      loadProducts,
      onRowSelect,
      addToCart,
      formatCurrency,
      viewCart,
    };
  },
};
</script>