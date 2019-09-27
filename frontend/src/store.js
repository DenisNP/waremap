import Vue from 'vue';
import Vuex from 'vuex';
import helpers from './common/helpers';
import config from './common/config';
import API from './common/api';
import serverState from './store/serverState';

Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
    serverState,
  },
  state: {
    currentDepotId: null, // if null = creating new depo
    currentFloor: 0,
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
        data = await API.getState();
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
