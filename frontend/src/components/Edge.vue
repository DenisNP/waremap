<template>
  <g
    cursor="pointer"
    @click="onClick"
  >
    <line
      :x1="fromNode.x"
      :y1="fromNode.y"
      :x2="toNode.x"
      :y2="toNode.y"
      stroke-width="3"
      :stroke="selected ? '#f00' : '#000'"
    />
    <line
      :x1="fromNode.x"
      :y1="fromNode.y"
      :x2="toNode.x"
      :y2="toNode.y"
      stroke-width="20"
      stroke="rgba(0,0,0,0.05)"
    />
  </g>
</template>

<script>
export default {
  name: 'Edge',
  props: [
    'data',
  ],
  methods: {
    onClick(e) {
      e.preventDefault();
      e.stopPropagation();
      this.$store.commit('depotEditor/selectEdge', this.data);
    },
  },
  computed: {
    id() {
      return this.data.from + '_' + this.data.to;
    },
    selected() {
      const selected = this.$store.state.depotEditor.selectedEdge;
      return selected && selected.from === this.data.from && selected.to === this.data.to;
    },
    fromNode() {
      return this.$store.getters['serverState/nodeById'](this.data.from);
    },
    toNode() {
      return this.$store.getters['serverState/nodeById'](this.data.to);
    },
  }
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
