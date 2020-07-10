﻿import Vue from 'vue';
import Vuetify from 'vuetify'
import axios from 'axios';
import SupplierComponent from './supplier.vue';

Vue.use(Vuetify);
Vue.prototype.$axios = axios
Vue.config.devtools = true;

let vue = new Vue({
    el: "#app",
    vuetify: new Vuetify(),
    axios: new axios(),
    components: {
        SupplierComponent
    }
});