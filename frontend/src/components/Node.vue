<template>
  <g class="node" :class="{show, ['node-' + data.icon]: true, selected}">
    <line class="edge edge-Dashed" :x1="data.x" :y1="data.y" :x2="newX" :y2="newY" stroke="black" v-if="selected" style="pointer-events: none;"/>

    <foreignObject
      @click="onClick"
      @contextmenu="onMenu"
      :id="data.id"
      :x="(draggingX || data.x) - w/2"
      :y="(draggingY || data.y) - h/2"
      :width="w"
      :height="h"
    >
      <div class="node-icon" v-if="$store.state.icons.node[data.icon]">
        <img :src="$store.state.icons.node[data.icon].i" />
      </div>
    </foreignObject>

    <foreignObject
      v-show="showMenu"
      :x="(draggingX || data.x)"
      :y="(draggingY || data.y)"
    >
      <div class="menu">
        <ul class="menu-options">
          <li class="menu-option">Связь отсюда</li>
          <li class="menu-option">Связь сюда</li>
        </ul>
      </div>
    </foreignObject>
  </g>
</template>

<script>
const MIN_DRAGGING_DISTANCE = 10;

export default {
  name: 'Node',
  props: [
    'data',
    'selected',
  ],
  mounted() {
    window.addEventListener('click', () => {
      this.showMenu = false;
    });
  },
  data() {
    return {
      showMenu: false,
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
    },
    iconWidth() {
      let icon = this.$store.state.icons.node[this.data.icon];
      console.log(this.data.icon)
      if (icon) return icon.w;
    }
  },
  methods: {
    onMenu(e) {
      e.preventDefault();
      this.showMenu = true;
    },
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

<style>
  .menu {
    width: 120px;
    box-shadow: 0 4px 5px 3px rgba(0, 0, 0, 0.2);
    position: relative;
    display: none;
  }

  .menu-options {
    list-style: none;
    padding: 10px 0;
  }

  .menu-option {
    font-weight: 500;
    font-size: 14px;
    padding: 10px 40px 10px 20px;
    cursor: pointer;
  }

/*.node.selected .node-icon {
  filter: hue-rotate(180deg);
}*/

.node.selected .node-icon:before {
  z-index: -1;
  content: '';
  display: block;
  position: absolute;
  top: 50%;
  left: 50%;
  margin: -45%;
  width: 90%;
  height: 90%;
  box-shadow: 0 0 0 6px rgba(255,0,0,1);
  border-radius: 9px;
}
.node foreignObject {
  overflow: visible;
}

</style>
