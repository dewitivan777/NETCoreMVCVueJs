import Vue from 'vue';
import Vuetify from 'vuetify'
import axios from 'axios';
import ProductComponent from './product.vue';
import MultiFiltersPlugin from './plugins/MultiFilters'

Vue.use(Vuetify);
Vue.use(MultiFiltersPlugin);
Vue.prototype.$axios = axios
Vue.config.devtools = true;

let vue = new Vue({
    el: "#app",
    vuetify: new Vuetify(),
    axios: new axios(),
    components: {
        ProductComponent
    }
});