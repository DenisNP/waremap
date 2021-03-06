<template>
  <div @click="onClick">
    <svg class="floorBgContainer">
      <image id="floorBg" :href="$store.state.editor.floorBackground" x="250" y="120" width="1000" />
    </svg>
    <svg @mousemove="onMouseMove" :class="{highlighted: $store.state.editor.isSomeHighlighted}" @click="clickBg">
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
        :highlighted="$store.state.editor.highlightedEdges[[data.from, data.to].sort().join('_')] === true"
        :highlighted_car="highlightedCarEdges[[data.from, data.to].sort().join('_')] === true"
        :data="data"
      ></Edge>
      <Node
        v-for="data in $store.state.serverState.geo.nodes"
        :key="'node' + data.id"
        :data="data"
        :selected="$store.state.editor.selectedNodeId === data.id"
        :highlighted="$store.state.editor.highlightedNodes[data.id] === true"
        :badge="floorsEdges[data.id]"
      ></Node>

      <Detail
        v-for="(data, index) in $store.state.serverState.equipment.parts"
        :highlighted="$store.state.editor.highlightedDetails[data.id]"
        :key="index"
        :data="data"
      ></Detail>

      <Car :data="$store.state.serverState.car_roadmap"></Car>

      <FloorToFloorEdge
        :data="$store.state.editor.selectedNodeId">
      </FloorToFloorEdge>
    </svg>
  </div>
</template>

<script>
import Node from './Node.vue';
import Edge from './Edge.vue';
import Depot from './Depot.vue';
import Detail from './Detail.vue';
import Car from './Car.vue';

import FloorToFloorEdge from './FloorToFloorEdge.vue';

export default {
  name: 'Editor',
  components: {
    Node,
    Depot,
    Edge,
    Detail,
    Car,
    FloorToFloorEdge
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
    this.$el.addEventListener('mousewheel', this.onMouseWheel);
  },
  props: {
    msg: String,
  },
  data() {
    return {
      zoom: 1,
      newDepot: null,
      isDrawingDepot: false,
      startX: null,
      startY: null,
    }
  },
  computed: {
    highlightedCarEdges() {
      this.$store.commit('editor/highlightedEdges', {edges: this.$store.state.serverState.car_roadmap.path, isCar: true});
      return this.$store.state.editor.highlightedCarEdges;
    },
    floorsEdges() {
      let floorsEdges = {};

      this.$store.state.serverState.geo.edges.map((edge) => {
        const from = this.$store.getters['serverState/nodeById'](edge.from);
        const to = this.$store.getters['serverState/nodeById'](edge.to);

        if (from.floor !== to.floor) {
          if (!(from.id in floorsEdges)) floorsEdges[from.id] = [];
          if (!(to.id in floorsEdges)) floorsEdges[to.id] = [];
          floorsEdges[from.id].push(to.floor);
          floorsEdges[to.id].push(from.floor);

          floorsEdges[from.id].sort();
          floorsEdges[to.id].sort();
        }
      });

      return floorsEdges;
    }
  },
  methods: {
    onMouseWheel(e) {
      console.log('wheel', e);
      e.preventDefault();

      if (e.deltaY < 0) {
        this.zoom *= 1.05;
      } else {
        this.zoom /= 1.05;
      }

      // this.$el.style.transform = 'scale(' + this.zoom + ')';
    },
    clickBg(e) {
      if (this.$store.state.editor.isSomeHighlighted) {
        if (e.target.tagName === 'svg' || e.target.classList.contains('node-Depot')) {
          this.$store.commit('editor/unselect');
        }
      }
    },
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
        this.$store.commit('editor/unselect');
      }
      if (e.which == 27) {
        this.$store.commit('editor/unselect');
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
    }
  },
  destroyed() {
    window.removeEventListener('keydown', this.onKeyDown);
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">

.floorBgContainer {
  opacity: .2;
  filter: invert(1);
}

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
