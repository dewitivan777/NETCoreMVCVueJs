import Vue from 'vue';
import Vuetify from 'vuetify'
import CategoryComponent from './category.vue';

Vue.use(Vuetify);
Vue.config.devtools = true;

let vue = new Vue({
    el: "#app",
    vuetify: new Vuetify(),
    components: {
        CategoryComponent
    }
});