import Vue from 'vue';

export default {
  namespaced: true,
  modules: {
  },
  state: {
    geo: {
      depots: [],
      nodes: [],
      edges: []
    },
    equipment: {
      cars: [],
      parts: []
    }
  },
  getters: {
    nodeById: state => nodeId => {
      if (!state.geo.nodes) {
        return false;
      }

      return state.geo.nodes.find(r => r.id === nodeId);
    }
  },
  mutations: {
    updateNode(state, newNode) {
      if (!state.geo.nodes) {
        return false;
      }

      let nodeIndex = state.geo.nodes.findIndex(r => r.id === newNode.id);
      if (!nodeIndex) {
        return false;
      }

      Vue.set(state.geo.nodes, nodeIndex, {...state.geo.nodes[nodeIndex], ...newNode});
    }
  },
  actions: {
  }
};
