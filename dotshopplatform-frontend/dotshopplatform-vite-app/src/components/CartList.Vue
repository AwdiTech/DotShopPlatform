<template> <!-- INCOMPLETE at Lab 9-->
  <div class="container">
    <div class="heading">Your Orders</div>
    <div class="status">{{ state.status }}</div>
    <div id="order-list">
      <DataTable
        v-if="state.carts.length > 0"
        :value="state.carts"
        :scrollable="true"
        scrollHeight="60vh"
        dataKey="id"
        class="p-datatable-striped"
        v-model:selection="state.cartSelected"
        selectionMode="single"
        @row-select="onRowSelect"
      >
        <Column header="Cart #" field="id" />
        <Column header="Date">
          <template #body="slotProps">
            {{ formatDate(slotProps.data.dateCreated) }}
          </template>
        </Column>
      </DataTable>

      <Dialog v-model:visible="state.selectedACart" class="dialog-border">
        <div style="font-weight: bold; margin-left: -8vw" class="cart-head">
          Cart: #{{ state.cartDetails[0].trayId }}
        </div>
        <DataTable
          :value="state.cartDetails"
          :scrollable="true"
          scrollHeight="38vh"
          dataKey="id"
          class="p-datatable-striped"
        >
          <Column header="Qty" field="qty" style="margin-right: -50vw" />
          <Column id="desc" header="Description" field="description" />
        </DataTable>
        <div>
          <div class="tray-total-head">Tray Totals</div>
          <table style="border: solid; margin-top: 1vh; margin-left: 1vw">
            <tr>
              <td style="width: 20%; font-weight: bold">Protein</td>
              <td style="width: 10%; text-align: right; padding-right: 3vw">
                {{ state.traySelected.totalProtein }}
              </td>
              <td style="width: 20%; font-weight: bold">Calories</td>
              <td style="width: 10%; text-align: right; padding-right: 3vw">
                {{ state.traySelected.totalCalories }}
              </td>
              <td style="width: 20%; font-weight: bold">Carbs.</td>
              <td style="width: 10%; text-align: right; padding-right: 3vw">
                {{ state.traySelected.totalCarbs }}
              </td>
            </tr>
            <tr>
              <td style="width: 20%; font-weight: bold">Fibre</td>
              <td style="width: 10%; text-align: right; padding-right: 3vw">
                {{ state.traySelected.totalFibre }}
              </td>
              <td style="width: 20%; font-weight: bold">Choles.</td>
              <td style="width: 10%; text-align: right; padding-right: 3vw">
                {{ state.traySelected.totalCholesterol }}
              </td>
              <td style="width: 20%; font-weight: bold">Salt</td>
              <td style="width: 10%; text-align: right; padding-right: 3vw">
                {{ state.traySelected.totalSalt }}
              </td>
            </tr>
            <tr>
              <td style="width: 20%; font-weight: bold">Fat</td>
              <td style="width: 10%; text-align: right; padding-right: 3vw">
                {{ state.traySelected.totalFat }}
              </td>
              <td colspan="4">&nbsp;</td>
            </tr>
          </table>
        </div>
      </Dialog>
    </div>
  </div>
</template>

<script>
import { reactive, onMounted } from "vue";
import { fetcher } from "../util/apiutil";
export default {
  setup() {
    let state = reactive({
      status: "",
      dialogStatus: "",
      trays: [],
      selectedATray: false,
      traySelected: {},
      trayDetails: [],
    });
    onMounted(() => {
      loadTrays();
    });
    const loadTrays = async () => {
      try {
        let user = JSON.parse(sessionStorage.getItem("user"));
        const payload = await fetcher(`tray/${user.email}`);
        if (payload.error === undefined) {
          state.trays = payload;
          state.status = `loaded ${state.trays.length} trays`;
          state.selectedATray = false;
        } else {
          state.status = payload.error;
        }
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };
    const formatDate = (date) => {
      let d;
      // see if date is coming from server
      date === undefined ? (d = new Date()) : (d = new Date(Date.parse(date))); // from server
      let _day = d.getDate();
      let _month = d.getMonth() + 1;
      let _year = d.getFullYear();
      let _hour = d.getHours();
      let _min = d.getMinutes();
      if (_min < 10) {
        _min = "0" + _min;
      }
      return _year + "-" + _month + "-" + _day;
    };

    const onRowSelect = async (event) => {
      try {
        let user = JSON.parse(sessionStorage.getItem("user"));
        state.traySelected = event.data;
        const payload = await fetcher(
          `tray/${state.traySelected.id}/${user.email}`
        );
        state.trayDetails = payload;
        state.dialogStatus = `details for tray ${state.traySelected.id}`;
        state.selectedATray = true;
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };

    return {
      state,
      formatDate,
      onRowSelect
    };
  },
};
</script>