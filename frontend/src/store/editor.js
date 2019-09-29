import API from '../common/api';
import helpers from '../common/helpers';

export default {
  namespaced: true,
  modules: {
  },
  state: {
    mode: 'default', // 'draggingNode'?, 'addingNode', 'addingEdge', 'nodeSelected', 'edgeSelected', 'depotSelected', 'addingDepot'
    floorBackground: null,
    floorBackgroundMap: {},
    addingNodeIcon: null, // enum('Machine, Point, Ladder, Elevator, Door')
    selectedNode: null,
    selectedNodeId: null,
    selectedDepotId: null,
    selectedEdge: null,
    highlightedNodes: {},
    highlightedEdges: {},
    isSomeHighlighted: false,
    isSelectedSomething: false,
    FloorToFloorEdge: null,
    displayMode: 'floor', // 'floor', 'depot'
    floor: 1,
    depot: {
      id: 0
    },
  },
  getters: {
  },
  mutations: {
    setFloor(state, floor) {
      // state.floorBackground = null;
      if (state.selectedNode && ['Ladder', 'Elevator'].indexOf(state.selectedNode.icon) > -1 && state.selectedNode.floor != floor) {
        let floorElement = document.querySelector('.pallete-item.floor-' + state.selectedNode.floor);
        if (!floorElement) return false;

        state.FloorToFloorEdge = {
          fromNode: state.selectedNode,
          fromFloor: state.selectedNode.floor,
          toFloor: floor,
          floorPos: {
            x: window.innerWidth / 2 - 260 + floorElement.offsetLeft + floorElement.offsetWidth / 2,
            y: floorElement.offsetTop + floorElement.offsetHeight
          }
        }
      }
      state.floor = floor;
      state.floorBackground = state.floorBackgroundMap[floor];
    },

    setDepot(state, depot) {
      state.depot = depot;
    },

    setDefaultMode(state) {
      unselect(state);
      state.mode = 'default';
      state.isSelectedSomething = false;
      state.selectedEdge = null;
      state.selectedNode = null;
      state.selectedNodeId = null;
      state.selectedDepotId = null;
    },

    startAddingNode(state, nodeIcon) {
      unselect(state);
      state.mode = 'addingNode';
      state.addingNodeIcon = nodeIcon;
    },

    startAddingDepot(state, nodeIcon) {
      unselect(state);
      state.mode = 'addingDepot';
      state.addingNodeIcon = nodeIcon;
    },

    selectNode(state, node) {
      state.mode = 'nodeSelected';
      state.isSelectedSomething = true;
      state.selectedEdge = null;
      state.selectedNode = node;
      state.selectedNodeId = node.id;
      state.selectedDepotId = null;
      state.isSomeHighlighted = false;
    },

    selectEdge(state, edge) {
      state.mode = 'edgeSelected';
      state.isSelectedSomething = true;
      state.selectedEdge = edge;
      state.selectedNode = null;
      state.selectedNodeId = null;
      state.selectedDepotId = null;
      state.isSomeHighlighted = false;
    },

    selectDepot(state, depot) {
      state.mode = 'depotSelected';
      state.isSelectedSomething = true;
      state.selectedEdge = null;
      state.selectedNode = null;
      state.selectedNodeId = null;
      state.selectedDepotId = depot.id;
      state.isSomeHighlighted = false;
    },

    unselect(state) {
      unselect(state);
    },

    highlightNodes(state, nodesIds) {
      let highlightedNodes = {};
      nodesIds.map((id) => highlightedNodes[id] = true);
      state.highlightedNodes = highlightedNodes;
      state.isSomeHighlighted = (Object.keys(highlightedNodes).length > 0);
    },

    highlightedEdges(state, edges) {
      let highlightedEdges = {};
      edges.map(({from_node, to_node}) => highlightedEdges[[from_node, to_node].sort().join('_')] = true);
      state.highlightedEdges = highlightedEdges;
      state.isSomeHighlighted = (Object.keys(highlightedEdges).length > 0);
    }
  },
  actions: {
    async endAddingNode(c, {x, y}) {
      const nodeType = c.state.addingNodeIcon === 'Machine' ? 'Machine' : 'Point';
      const newState = await API.addNode({
        type: nodeType,
        x,
        y,
        icon: c.state.addingNodeIcon,
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
      const newState = await API.autoComputeEdges(c.state.depot.id, c.state.floor);
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
      const newState = await API.addDepot({
        ...data,
        floor: c.state.floor
      });
      c.commit('setServerState', newState, {root: true});
    },

    async removeSelectedDepot(c) {
      const newState = await API.removeDepot(c.state.selectedDepotId);
      c.commit('setServerState', newState, {root: true});
    },

    async uploadFloorBackground(c, base64) {
      try {
        await API.sendBackground(base64, c.state.floor);
        c.state.floorBackground = base64;
      } catch(e) {
        console.log('uploadFloorBackground exception')
      }
    },

    async downloadFloorBackground(c) {
      if (c.state.floorBackgroundMap[c.state.floor]) {
        c.state.floorBackground = c.state.floorBackgroundMap[c.state.floor];
        return;
      }

      c.state.floorBackground = '';
      try {
        c.state.floorBackground = await API.getBackground(c.state.floor);
        c.state.floorBackgroundMap[c.state.floor] = c.state.floorBackground;
      } catch(e) {
        return;
      }

      console.log('downloaded background', c.state.floorBackground.length);
    }
  }
};

function unselect(state) {
  state.mode = 'default';
  state.isSelectedSomething = false;
  state.selectedNodeId = null;
  state.selectedEdge = null;
  state.selectedDepotId = null;
  state.highlightedNodes = {};
  state.highlightedEdges = {};
  state.isSomeHighlighted = false;
  state.FloorToFloorEdge = null;
}
