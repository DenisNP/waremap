
export default {
  namespaced: true,
  modules: {
  },
  state: {
    geo: {
      depots: [],
      nodes: [
        {x: 10, y: 50, id: 1},
        {x: 110, y: 101, id: 2}
      ],
      edges: [{
        from: 1,
        to: 2,
      }]
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
    },
  },
  mutations: {
  },
  actions: {

  }
};
