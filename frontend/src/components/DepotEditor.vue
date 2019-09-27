<template>
  <div @click="onClick">
    <svg>
      <Node
        v-for="data in $store.state.serverState.geo.nodes"
        :key="data.id"
        :data="data"
      ></Node>
      <Edge
        v-for="data in $store.state.serverState.geo.edges"
        :key="data.id"
        :data="data"
      ></Edge>
    </svg>
  </div>
</template>

<script>
import Node from './Node.vue';
import Edge from './Edge.vue';

export default {
  name: 'DepotEditor',
  mounted() {
    this.$root.$on('toolSelected', e => {
      const {name, key, icon} = e;
      this.$store.commit('depotEditor/startAddingNode', key);
    });

    this.$root.$on('floorSelect', floor => {
      this.$store.commit('depotEditor/setFloor', floor);
    });
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
    onClick(e) {
      console.log(111, e);
      const x = e.clientX;
      const y = e.clientY;

      if (this.$store.state.depotEditor.mode === 'addingNode') {
        this.$store.dispatch('depotEditor/endAddingNode', {x, y});
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
