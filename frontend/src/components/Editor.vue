<template>
  <div @click="onClick">
    <svg>
      <Edge
        v-for="data in $store.state.serverState.geo.edges"
        :key="data.from + '_' + data.to"
        :data="data"
      ></Edge>
      <Node
        v-for="data in $store.state.serverState.geo.nodes"
        :key="data.id"
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

export default {
  name: 'Editor',
  mounted() {
    this.$root.$on('toolSelected', e => {
      const {name, key, icon} = e;
      this.$store.commit('editor/startAddingNode', key);
    });

    this.$root.$on('floorSelected', floor => {
      this.$store.commit('editor/setFloor', floor);
    });

    this.$root.$on('nodeUpdated', data => {
      this.$store.dispatch('editor/updateNode', data);
    });

    this.$root.$on('nodeSelected', data => {
      this.$store.commit('editor/selectNode', data);
    });

    window.addEventListener('keydown', this.onKeyDown);
  },
  components: {
    Node,
    Edge
  },
  props: {
    msg: String,
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
      }
    },
    onClick(e) {
      const x = e.clientX;
      const y = e.clientY;

      if (this.$store.state.editor.mode === 'addingNode') {
        this.$store.dispatch('editor/endAddingNode', {x, y});
      }

      if (['nodeSelected', 'edgeSelected'].includes(this.$store.state.editor.mode)) {
        this.$store.commit('editor/unselect');
      }
    }
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
