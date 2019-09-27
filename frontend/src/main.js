import Vue from 'vue';
import App from './App.vue';
import store from './store';
import Vuex from 'vuex';

Vue.use(Vuex);
Vue.config.productionTip = false;

new Vue({
  store,
  render: h => h(App),
  async created() {
    this.$store.dispatch('init');
  }
}).$mount('#app');
