import API from '../common/api';
import helpers from '../common/helpers';

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
    depot: {
      id: 0
    },
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

    setDefaultMode(state) {
      state.mode = 'default';
      state.selectedEdge = null;
      state.selectedNodeId = null;
    },

    startAddingNode(state, nodeIcon) {
      state.mode = 'addingNode';
      state.addingNodeIcon = nodeIcon;
    },

    selectNode(state, node) {
      state.mode = 'nodeSelected';
      state.selectedEdge = null;
      state.selectedNodeId = node.id;
    },

    selectEdge(state, edge) {
      state.mode = 'edgeSelected';
      state.selectedEdge = edge;
      state.selectedNodeId = null;
    },

    unselect(state) {
      state.mode = 'default';
      state.selectedNodeId = null;
      state.selectedEdge = null;
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
        depot: c.state.depot.id,
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
    },

    async autoComputeEdges(c) {
      const newState = await API.autoComputeEdges(c.state.depot.id);
      c.commit('setServerState', newState, {root: true});
    },

    async createEdge(c, {from, to}) {
      const newState = await API.addOrUpdateEdge({
        type: 'Road',
        weight: helpers.distance(c.rootGetters['serverState/nodeById'](from), c.rootGetters['serverState/nodeById'](to)),
        from,
        to
      });

      c.commit('setServerState', newState, {root: true});
    },

    async removeSelectedEdge(c) {
      const newState = await API.removeEdge({
        from: c.state.selectedEdge.from,
        to: c.state.selectedEdge.to,
      });
      c.commit('setServerState', newState, {root: true});
    },
  }
};
