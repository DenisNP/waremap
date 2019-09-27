<template>
  <div id="app">
    <ControlsLayer/>
    <DepotEditor/>
  </div>
</template>

<script>
import DepotEditor from './components/DepotEditor.vue';
import ControlsLayer from './components/ControlsLayer.vue';

export default {
  name: 'app',
  components: {
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
