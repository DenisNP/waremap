import Vue from 'vue';
import Vuex from 'vuex';
import helpers from './common/helpers';
import config from './common/config';
import API from './common/api';
import serverState from './store/serverState';
import editor from './store/editor';
import icons from './store/icons';

Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
    serverState,
    editor,
    icons,
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
    setServerState(state, newState) {
      state.serverState = newState;
    }
  },
  actions: {
    async init(c) {
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

      c.commit('setServerState', data);
    },
  }
});
