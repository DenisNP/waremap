import API from '../common/api';
import helpers from '../common/helpers';

export default {
  namespaced: true,
  modules: {
  },
  state: {
    mode: 'default', // 'draggingNode'?, 'addingNode', 'addingEdge', 'nodeSelected', 'edgeSelected', 'depotSelected', 'addingDepot'
    addingNodeIcon: null, // enum('Machine, Point, Ladder, Elevator, Door')
    selectedNodeId: null,
    selectedDepotId: null,
    selectedEdge: null,
    displayMode: 'floor', // 'floor', 'depot'
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

    startAddingDepot(state, nodeIcon) {
      state.mode = 'addingDepot';
      state.addingNodeIcon = nodeIcon;
    },

    selectNode(state, node) {
      state.mode = 'nodeSelected';
      state.selectedEdge = null;
      state.selectedNodeId = node.id;
      state.selectedDepotId = null;
    },

    selectEdge(state, edge) {
      state.mode = 'edgeSelected';
      state.selectedEdge = edge;
      state.selectedNodeId = null;
      state.selectedDepotId = null;
    },

    selectDepot(state, depot) {
      console.log('select depot', depot.id);
      state.mode = 'depotSelected';
      state.selectedEdge = null;
      state.selectedNodeId = null;
      state.selectedDepotId = depot.id;
    },

    unselect(state) {
      state.mode = 'default';
      state.selectedNodeId = null;
      state.selectedEdge = null;
      state.selectedDepotId = null;
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
      const existingEdge = c.rootState.serverState.geo.edges.find(edge =>
        (edge.from === from && edge.to === to) ||
        (edge.from === to && edge.to === from)
      );

      if (existingEdge) {
        console.log('existingEdge');
        return;
      }

      const newState = await API.addOrUpdateEdge({
        type: 'Road',
        weight: helpers.distance(c.rootGetters['serverState/nodeById'](from), c.rootGetters['serverState/nodeById'](to)),
        from,
        to
      });

      c.commit('setServerState', newState, {root: true});
    },

    async updateEdge(c, {from, to, type, weight}) {
      const existingEdge = c.rootState.serverState.geo.edges.find(edge =>
        (edge.from === from && edge.to === to) ||
        (edge.from === to && edge.to === from)
      );

      if (!existingEdge) {
        console.log('not existingEdge');
        return;
      }

      const newState = await API.addOrUpdateEdge({
        type,
        weight,
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

    async addDepot(c, data) {
      const newState = await API.addDepot(data);
      c.commit('setServerState', newState, {root: true});
    },

    async removeSelectedDepot(c) {
      const newState = await API.removeDepot(c.state.selectedDepotId);
      c.commit('setServerState', newState, {root: true});
    },
  }
};
