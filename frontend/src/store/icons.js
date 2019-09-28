import Vue from 'vue';

export default {
  namespaced: true,
  modules: {
  },
  state: {
    node: {
      Machine: {i: require('../assets/machine-tool.svg'), w: 50},
      Node: {i: require('../assets/node.svg'), w: 30},
      Ladder: {i: require('../assets/stairs.svg'), w: 40},
      Elevator: {i: require('../assets/lift.svg'), w: 40},
      Door: {i: require('../assets/door.svg'), w: 40},
      undefined: {i: require('../assets/node.svg'), w: 30},
      '': {i: require('../assets/node.svg'), w: 30},
    }
  },
  getters: {
  },
  mutations: {
  },
  actions: {
  }
};
