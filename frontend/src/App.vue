<template>
  <div id="app">
    <ControlsLayer floor="2" tool="3" />
    <Editor/>
    <EdgeParams
      v-if="$store.state.editor.mode === 'edgeSelected'"
      :data="$store.state.editor.selectedEdge"
    />
  </div>
</template>

<script>
import Editor from './components/Editor.vue';
import ControlsLayer from './components/ControlsLayer.vue';
import EdgeParams from './components/EdgeParams.vue';

export default {
  name: 'app',
  components: {
    EdgeParams,
    Editor,
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
