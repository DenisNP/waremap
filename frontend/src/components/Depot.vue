<template>
  <g>
    <rect
      @click="onClick"
      @mousedown="onMouseDown"
      :x="data.x"
      :y="data.y"
      :width="data.w"
      :height="data.h"
      :fill="selected ? 'rgba(255,0,0,0.3)' : 'rgba(100,100,100,0.3)'"></rect>
  </g>
</template>

<script>

export default {
  name: 'Depot',
  props: [
    'data',
    'selected',
    'isNew',
  ],
  computed: {
  },
  methods: {
    onMouseDown(e) {
      e.preventDefault();
    },
    onClick(e) {
      e.preventDefault();

      if (this.isNew) {
        return;
      }

      if (this.$store.state.editor.mode === 'default' || this.$store.state.editor.isSelectedSomething) {
        this.$store.commit('editor/selectDepot', this.data);
        e.stopPropagation();
      }

      if (this.$store.state.editor.mode === 'depotSelected' && this.data.id !== this.$store.state.editor.selectedDepotId) {
      }
    },
  },
  mounted() {
  },
  updated() {
  },
  destroyed() {
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style lang="scss">

.node {
  display: none;
  cursor: pointer;
}
.node.show {
  display: block;
}

.walls {
    position: relative;
    display: block;
    width: 100%;
    height: 100%;
    background-color: #FFF;
    cursor: pointer;
}
.walls.selected {
  background-color: #f00;
}
.wall, .corner {
    position: absolute;
    background-color: rgba(0,0,0,.5);
}
.wall.left,
.wall.right,
.corner { width: 5px; }
.wall.left,
.wall.right { height: 100%; }
.wall.top,
.wall.bottom,
.corner { height: 5px; }
.wall.top,
.wall.bottom { width: 100%; }
.walls .top { top: 0; bottom: auto; }
.walls .bottom { top: auto; bottom: 0; }
.walls .left { left: 0; right: auto; }
.walls .right { left: auto; right: 0; }


.node-icon {
  display: flex;
  width: 100%;
  height: 100%;
  justify-content: center;
  align-items: center;
}
.node-icon img {
  max-height: 98%;
}

</style>
