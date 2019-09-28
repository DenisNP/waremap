<template>
  <div id="app">
    <AppQr v-if="page === 'qr'"></AppQr>
    <AppMobile v-if="page === 'mobile'"></AppMobile>
    <div v-if="page === 'main'">
      <ControlsLayer floor="1" tool="0" />
      <Editor/>
      <EdgeParams
        v-if="$store.state.editor.mode === 'edgeSelected'"
        :data="$store.state.editor.selectedEdge"
      />
      <MachineParams
        v-if="isMachineSelected"
        :data="$store.state.editor.selectedNode"
      />
    </div>
  </div>
</template>

<script>
import AppQr from './components/AppQr.vue';
import AppMobile from './components/AppMobile.vue';
import Editor from './components/Editor.vue';
import ControlsLayer from './components/ControlsLayer.vue';
import EdgeParams from './components/EdgeParams.vue';
import MachineParams from './components/MachineParams.vue';

export default {
  name: 'app',
  components: {
    AppQr,
    AppMobile,
    EdgeParams,
    MachineParams,
    Editor,
    ControlsLayer
  },
  data() {
    return {
      page: ''
    }
  },
  async mounted() {
    console.log('location', location);
    const params = location.hash.replace('#', '').split('/');
    this.page = params[0] || 'main';

    if (this.page === 'main') {
      await this.$store.dispatch('init');
      await this.$store.dispatch('editor/downloadFloorBackground');
    }
  },
  computed: {
    isMachineSelected() {
      return this.$store.state.editor.mode === 'nodeSelected' && this.$store.state.editor.selectedNode.type === 'Machine';
    }
  }
};
</script>

<style lang="scss">

body {
  color: #fff;
  background-color: #333;
}

svg {
  -webkit-touch-callout: none;
    -webkit-user-select: none;
     -khtml-user-select: none;
       -moz-user-select: none;
        -ms-user-select: none;
            user-select: none;
}

#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

/* ---------------------------------------------------------------- */

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
.walls.selected .wall,
.walls.selected .corner {
    filter: hue-rotate(270deg)
}
.wall {
  position: absolute;
  width: 100vmax;
  height: 100vmax;
  background-image: url(./assets/tiles/main.svg);
  background-size: 30px;
  background-repeat: repeat-x;
  background-position: center;
  left: 0;
}
.wall.top {   top: calc(-50vmax + 5px);  bottom: auto;                transform: rotate(0deg); }
.wall.bottom {top: auto;                 bottom: calc(-50vmax + 5px); transform: rotate(-180deg); }
.wall.left {  left: calc(-50vmax + 5px); right: auto;                 transform: rotate(90deg); }
.wall.right { left: auto;                right:  calc(-50vmax + 5px); transform: rotate(-90deg); }

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

/* ---------------------------------------------------------------- */

.edge.edge-Dashed,
.edge.edge-Footway {
  stroke: rgba(255,255,255,.3);
  stroke-width: 2px;
  fill: transparent;
  stroke-dasharray: 10 5;
  animation: dash 200s linear infinite;
}
.edge.inverse,
.edge.edge-Dashed {
  animation-direction: reverse;
}

@keyframes dash {
  to {
    stroke-dashoffset: 2000;
  }
}

.edge.edge-Road {
  fill: transparent;
  stroke: rgba(255,255,255,.6);
  stroke-width: 2px;
}

.edge.selected {
  stroke: #F00;
}

</style>
