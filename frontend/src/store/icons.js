import Vue from 'vue';

export default {
  namespaced: true,
  modules: {
  },
  state: {
    node: {
      Machine: {i: require('../assets/machine-tool.svg'), w: 40},
      Node: {i: require('../assets/node.svg'), w: 15},
      Ladder: {i: require('../assets/stairs.svg'), w: 40},
      Elevator: {i: require('../assets/lift.svg'), w: 40},
      Door: {i: require('../assets/door.svg'), w: 40},

      undefined: {i: require('../assets/node.svg'), w: 15},
      '': {i: require('../assets/node.svg'), w: 15},
    },
    pallete: {
      Cursor: {i: require('../assets/cursor.svg'), w: 32},
      Door: {i: require('../assets/add_door.svg'), w: 32},
      Stairs: {i: require('../assets/add_stairs.svg'), w: 32},
      Elevator: {i: require('../assets/add_elevator.svg'), w: 32},
      Depot: {i: require('../assets/add_depot.svg'), w: 32},
      Machine: {i: require('../assets/machine-tool.svg'), w: 32},
      Edges: {i: require('../assets/create_edge.svg'), w: 32},
      Node: {i: require('../assets/add_edge.svg'), w: 32},
      UploadBg: {i: require('../assets/cursor_01.svg'), w: 32}
    },
    edgeParams: {
      Road: {i: require('../assets/create_edge.svg'), w: 32},
      Footway: {i: require('../assets/create_edge.svg'), w: 32},
      Elevator: {i: require('../assets/create_edge.svg'), w: 32},
      Ladder: {i: require('../assets/create_edge.svg'), w: 32},
    }
  },
  getters: {
  },
  mutations: {
  },
  actions: {
  }
};
