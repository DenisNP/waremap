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

.node {
  display: none;
  cursor: pointer;
}
.node.show {
  display: block;
}

.walls {
    position: relative;
    display: none;
    width: 100%;
    height: 100%;
    background-color: #FFF;
    cursor: pointer;
    background-image: url(./assets/tiles/tile.svg);
}
.walls.selected {
  border: 5px solid #F00;
  box-sizing: border-box;
  overflow: hidden;
}
.wall {
  position: absolute;
  width: 100%;
  height: 100%;
  background-image: url(./assets/tiles/main.svg);
  background-size: 30px;
  background-repeat: repeat-x;
  background-position: center;
  left: 0;
}
.wall.top {   top: calc(-50% + 5px);  bottom: auto;             transform: rotate(0deg); }
.wall.bottom {top: auto;              bottom: calc(-50% + 5px); transform: rotate(-180deg); }
.wall.left {  left: calc(-50% + 5px); right: auto;              transform: rotate(90deg); }
.wall.right { left: auto;             right: calc(-50% + 5px);  transform: rotate(-90deg); }

.corner {
  width: 30px;
  height: 30px;
  margin: -10px;
  position: absolute;
  z-index: 1;
  background-image: url(./assets/tiles/corner.svg);
  background-size: 30px;
  background-repeat: repeat-x;
  background-position: center;
}
.corner.left {   top: 0;    bottom: auto; left: 0;    right: auto; transform: rotate(0deg); }
.corner.top {    top: 0;    bottom: auto; left: auto; right: 0;    transform: rotate(90deg); }
.corner.right {  top: auto; bottom: 0;    left: 0;    right: auto; transform: rotate(270deg); }
.corner.bottom { top: auto; bottom: 0;    left: auto; right: 0;    transform: rotate(180deg); }

.node-icon {
  display: flex;
  width: 100%;
  height: 100%;
  justify-content: center;
  align-items: center;
}
.node-icon img {
  max-width: 98%;
  max-height: 100%;
}

.node-Depot .walls {
  display: block;
}
.node-Depot .node-icon {
  display: none;
}

</style>
