import API from '../common/api';

export default {
  namespaced: true,
  modules: {
  },
  state: {
    mode: 'default', // 'draggingNode', 'addingNode', 'addingEdge'
    addingNodeIcon: null, // enum('Machine, Point, Ladder, Elevator, Door')
    floor: null,
    depot: null,
  },
  getters: {
  },
  mutations: {
    setFloor(state, floor) {
      state.floor = floor;
    },

    setDepot(state, depot) {
      state.depot = depot;
    },

    startAddingNode(state, nodeIcon) {
      state.mode = 'addingNode';
      state.addingNodeIcon = nodeIcon;
    },
  },
  actions: {
    async endAddingNode(c, {x, y}) {
      const nodeType = c.state.addingNodeIcon === 'Machine' ? 'Machine' : 'Point';
      const newState = await API.addNode({
        type: nodeType,
        x,
        y,
        floor: c.state.floor,
        depot: c.state.depot,
      });

      c.commit('setServerState', newState, {root: true});
    }
  }
};
