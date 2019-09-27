import API from '../common/api';

export default {
  namespaced: true,
  modules: {
  },
  state: {
    mode: 'default', // 'draggingNode'?, 'addingNode', 'addingEdge', 'nodeSelected', 'edgeSelected'
    addingNodeIcon: null, // enum('Machine, Point, Ladder, Elevator, Door')
    selectedNodeId: null,
    selectedEdge: null,
    floor: 0,
    depot: 0,
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

    selectNode(state, node) {
      state.mode = 'nodeSelected';
      state.selectedNodeId = node.id;
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
    },

    async updateNode(c, data) {
      const newState = await API.updateNode(data);
      c.commit('setServerState', newState, {root: true});
    },

    async removeSelectedNode(c) {
      const newState = await API.removeNode(c.state.selectedNodeId);
      c.commit('setServerState', newState, {root: true});
    }
  }
};
