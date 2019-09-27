<template>
  <div>
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
  components: {
    Node,
    Edge
  },
  props: {
    msg: String,
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
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
