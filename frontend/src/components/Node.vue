<template>
  <rect :x="data.x" :y="data.y" width="10" height="10"/>
</template>

<script>
export default {
  name: 'Node',
  props: [
    'data',
  ],
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