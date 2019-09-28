import Vue from 'vue';

export default {
  namespaced: true,
  modules: {
  },
  state: {
    node: {
      Machine: {i: require('../assets/machine-tool.svg'), w: 50},
      Point: {i: require('../assets/node.svg'), w: 30},
      Ladder: {i: require('../assets/stairs.svg'), w: 40},

      undefined: {i: require('../assets/node.svg'), w: 30},
    }
  },
  getters: {
  },
  mutations: {
  },
  actions: {
  }
};
