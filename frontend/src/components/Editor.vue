<template>
  <div @click="onClick">
    <svg @mousemove="onMouseMove">
      <image width="1000" height="700" x="260" y="20" :xlink:href="$store.state.editor.floorBackground" v-if="$store.state.editor.floorBackground" />

      <Depot
        v-for="data in $store.state.serverState.geo.depots"
        :key="'depot' + data.id"
        :data="data"
        :selected="$store.state.editor.selectedDepotId === data.id"
      ></Depot>
      <Depot
        v-if="isDrawingDepot"
        :data="newDepot"
        :isNew="true"
      ></Depot>
      <Edge
        v-for="data in $store.state.serverState.geo.edges"
        :key="'edge' + data.from + '_' + data.to"
        :data="data"
      ></Edge>
      <Node
        v-for="data in $store.state.serverState.geo.nodes"
        :key="'node' + data.id"
        :data="data"
        :selected="$store.state.editor.selectedNodeId === data.id"
      ></Node>

      <Text>{{ $store.state.editor.floor }}</Text>
      <Text>{{ $store.state.editor.depot.id }}</Text>
    </svg>
  </div>
</template>

<script>
import Node from './Node.vue';
import Edge from './Edge.vue';
import Depot from './Depot.vue';

export default {
  name: 'Editor',
  components: {
    Node,
    Depot,
    Edge
  },
  mounted() {
    this.$root.$on('toolSelected', e => {
      const {name, key, icon} = e;

      if (key !== 'Depot') {
        this.$store.commit('editor/startAddingNode', key);
      } else {
        this.$store.commit('editor/startAddingDepot', key);
      }
    });

    this.$root.$on('floorSelected', floor => {
    });

    this.$root.$on('nodeUpdated', data => {
      this.$store.dispatch('editor/updateNode', data);
    });

    this.$root.$on('nodeSelected', data => {
      this.$store.commit('editor/selectNode', data);
    });

    window.addEventListener('keydown', this.onKeyDown);
  },
  props: {
    msg: String,
  },
  data() {
    return {
      newDepot: null,
      isDrawingDepot: false,
      startX: null,
      startY: null,
    }
  },
  computed: {

  },
  methods: {
    async onKeyDown(e) {
      if (e.keyCode === 46) { // delete
        if (this.$store.state.editor.mode === 'nodeSelected') {
          await this.$store.dispatch('editor/removeSelectedNode');
        }
        if (this.$store.state.editor.mode === 'edgeSelected') {
          await this.$store.dispatch('editor/removeSelectedEdge');
        }
        if (this.$store.state.editor.mode === 'depotSelected') {
          await this.$store.dispatch('editor/removeSelectedDepot');
        }
      }
    },
    onMouseMove(e) {
      const x = e.clientX;
      const y = e.clientY;

      if (this.isDrawingDepot) {
        this.newDepot = {
          x: Math.min(x, this.startX),
          y: Math.min(y, this.startY),
          w: Math.abs(x - this.startX),
          h: Math.abs(y - this.startY),
        }
      }
    },
    onClick(e) {
      const x = e.clientX;
      const y = e.clientY;

      if (this.$store.state.editor.mode === 'addingNode') {
        this.$store.dispatch('editor/endAddingNode', {x, y});
      } else if (this.$store.state.editor.mode === 'addingDepot') {
        if (!this.isDrawingDepot) {
          this.startX = x;
          this.startY = y;

          this.newDepot = {
            x: Math.min(x, this.startX),
            y: Math.min(y, this.startY),
            w: Math.abs(x - this.startX),
            h: Math.abs(y - this.startY),
          };
          this.isDrawingDepot = true;
        } else {
          this.$store.dispatch('editor/addDepot', {
            x: Math.min(x, this.startX),
            y: Math.min(y, this.startY),
            w: Math.abs(x - this.startX),
            h: Math.abs(y - this.startY),
          });

          this.startX = null;
          this.startY = null;
          this.isDrawingDepot = false;
        }
      } else if (['nodeSelected', 'edgeSelected', 'depotSelected'].includes(this.$store.state.editor.mode)) {
        this.$store.commit('editor/unselect');
      }
    },
    onKeyDown(e) {
      if (e.which == 27) {
        this.$store.commit('editor/unselect');
      }
    }
  },
  mounted() {
    window.addEventListener('keydown', this.onKeyDown);
  },
  destroyed() {
    window.removeEventListener('keydown', this.onKeyDown);
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">

svg {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100vh;
  z-index: 0;
}

h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
