import Vue from 'vue';
import Vuex from 'vuex';
import helpers from './common/helpers';
import config from './common/config';
import API from './common/api';

Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
  },
  state: {
  },
  getters: {
  },
  mutations: {
    set(state, {field, value}) {
      state[field] = value;
    },
  },
  actions: {
    async init(context) {
      let data;
      let initNeeded = true;

      try {
        data = await API.startGame();
      } catch (err) {
        data = require('./common/backend-response.dist.json');
        initNeeded = false;
        console.warn('TEST DATA USED');
      }

      if (!initNeeded) {
        return;
      }

      // set state from data
    },
  }
});
