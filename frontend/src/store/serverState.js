import API from '../common/api';
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
    },
  },
  actions: {
    async importFullState(c, fullState) {
      debugger;
      /*
      const res = await Promise.all([
        API.sendAction('load', fullState.state),
        ...fullState.backgrounds.filter(Boolean).map(bg =>
          API.sendBackground(bg.base64, bg.floor),
        )
      ]);
      */
      const newState = await API.sendAction('load', fullState.state);

      for (const bg of fullState.backgrounds) {
        await API.sendBackground(bg.base64, bg.floor);
      }

      c.commit('setServerState', newState, {root: true});
    },
  }
};
