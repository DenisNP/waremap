<template>
  <g class="node" :class="{show, ['node-' + data.type]: true}">
    <line :x1="data.x" :y1="data.y" :x2="newX" :y2="newY" stroke="black" v-if="selected" style="pointer-events: none;"/>

    <foreignObject
      @click="onClick"
      :id="data.id"
      :x="(draggingX || data.x) - w/2"
      :y="(draggingY || data.y) - h/2"
      :width="w"
      :height="h"
    >
      <div class="node-icon" v-if="$store.state.icons.node[data.type]">
        <img :src="$store.state.icons.node[data.type].i" />
      </div>
      <div
        class="walls"
        :class="{selected}"
      >
        <div class="wall top"></div>
        <div class="wall right"></div>
        <div class="wall bottom"></div>
        <div class="wall left"></div>

        <div class="corner top"></div>
        <div class="corner right"></div>
        <div class="corner bottom"></div>
        <div class="corner left"></div>
      </div>
    </foreignObject>
  </g>
</template>

<script>
const MIN_DRAGGING_DISTANCE = 50;

export default {
  name: 'Node',
  props: [
    'data',
    'selected',
  ],
  data() {
    return {
      w: 40,
      h: 40,
      draggingX: null,
      draggingY: null,
      down: false,
      dragging: false,
      newX: null,
      newY: null
    };
  },
  computed: {
    show() {
      return (this.$store.state.editor.displayMode == 'floor' && this.data.floor == this.$store.state.editor.floor)
        ||   (this.$store.state.editor.displayMode == 'depot' && this.data.depot == this.$store.state.editor.depot.id);
    }
  },
  methods: {
    onClick(e) {
      e.preventDefault();
      e.stopPropagation();

      if (this.$store.state.editor.mode === 'nodeSelected' && this.data.id !== this.$store.state.editor.selectedNodeId) {
        this.$store.dispatch('editor/createEdge', {
          from: this.$store.state.editor.selectedNodeId,
          to: this.data.id
        });
        this.$store.commit('editor/unselect');
      }
    },
    onMouseDown(e) {
      e.preventDefault();
      this.down = true;
    },
    onMouseMove(e) {
      this.newX = e.clientX;
      this.newY = e.clientY;

      if (this.down !== true) return;

      if (!this.dragging) {
        const distance = Math.max(
          Math.abs(this.data.x - this.newX),
          Math.abs(this.data.y - this.newY)
        );

        if (distance < MIN_DRAGGING_DISTANCE) {
          return;
        }
      }

      this.dragging = true;
      this.draggingX = this.newX;
      this.draggingY = this.newY;

      this.$store.commit('serverState/updateNode', {
        id: this.data.id,
        x: this.draggingX,
        y: this.draggingY
      });
      this.$store.commit('editor/unselect');
    },
    onMouseUp(e) {
      const wasDown = this.down;
      this.down = false;

      if (wasDown && !this.dragging && !this.$store.state.editor.selectedNodeId) {
        // we just selected
        this.$root.$emit('nodeSelected', this.data);
        return;
      }

      if (this.dragging) {
        console.log('update node');
        this.$root.$emit('nodeUpdated', {...this.data, x: this.draggingX, y: this.draggingY});
        this.dragging = false;
      }
    }
  },
  mounted() {
    if (this.iconWidth) {
      this.w = this.iconWidth;
      this.h = this.iconWidth;
    }

    this.$el.addEventListener('mousedown', this.onMouseDown);
    window.addEventListener('mousemove', this.onMouseMove);
    this.$el.addEventListener('mouseup', this.onMouseUp);
  },
  updated() {
    this.data.draggingX = null;
    this.data.draggingY = null;
  },
  destroyed() {
    this.$el.removeEventListener('mousedown', this.onMouseDown);
    window.removeEventListener('mousemove', this.onMouseMove);
    this.$el.removeEventListener('mouseup', this.onMouseUp);
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
    display: none;
    width: 100%;
    height: 100%;
    background-color: #FFF;
    cursor: pointer;
    background-image: url(../assets/tiles/tile.svg);
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
  background-image: url(../assets/tiles/main.svg);
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
  background-image: url(../assets/tiles/corner.svg);
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
