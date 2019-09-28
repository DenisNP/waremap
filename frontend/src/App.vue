<template>
  <div id="app">
    <ControlsLayer floor="2" tool="3" />
    <DepotEditor/>
    <EdgeParams
      v-if="$store.state.depotEditor.mode === 'edgeSelected'"
      :data="$store.state.depotEditor.selectedEdge"
    />
  </div>
</template>

<script>
import DepotEditor from './components/DepotEditor.vue';
import ControlsLayer from './components/ControlsLayer.vue';
import EdgeParams from './components/EdgeParams.vue';

export default {
  name: 'app',
  components: {
    EdgeParams,
    DepotEditor,
    ControlsLayer
  },
};
</script>

<style lang="scss">
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
