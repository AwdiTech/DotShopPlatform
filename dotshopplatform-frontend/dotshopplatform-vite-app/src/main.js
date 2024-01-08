import { createApp } from 'vue'
import App from './App.vue'
import PrimeVue from 'primevue/config';
import Button from 'primevue/button';
import Dropdown from 'primevue/dropdown';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Dialog from 'primevue/dialog';
import InputNumber from 'primevue/inputnumber';
import Toolbar from 'primevue/toolbar';
import Menu from 'primevue/menu';
import router from "./router";
import InputText from 'primevue/inputtext';

import 'primevue/resources/primevue.min.css'; //core css
import 'primevue/resources/themes/saga-orange/theme.css'; //theme
import 'primeicons/primeicons.css'; //icons
import './assets/site.css';

const app = createApp(App)
app.use(PrimeVue);
app.use(router);
app.component('Button', Button);
app.component('Dropdown', Dropdown);
app.component('DataTable', DataTable);
app.component('Column', Column);
app.component('Dialog', Dialog);
app.component('InputNumber', InputNumber);
app.component('Toolbar', Toolbar);
app.component('Menu', Menu);
app.component('InputText', InputText);
app.mount('#app');