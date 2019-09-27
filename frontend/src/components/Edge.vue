<template>
  <div>
    <h6>{{ fromNode.x }}, {{ fromNode.y }}</h6>
  </div>
</template>

<script>
export default {
  name: 'Edge',
  props: [
    'data'
  ],
  computed: {
    fromNode() {
      return this.$store.getters['serverState.nodeById'](this.from);
    },
    toNode() {
      return this.$store.getters['serverState.nodeById'](this.to);
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
