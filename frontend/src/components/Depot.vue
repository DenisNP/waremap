<template>
  <g v-show="show">  
    <foreignObject
      class="node node-Depot show"
      @click="onClick"
      @mousedown="onMouseDown"
      :x="data.x"
      :y="data.y"
      :width="data.w"
      :height="data.h"
    >
      <div
        class="walls"
        :class="{selected}"
      >
        <span class="depot-name">{{ data.name }}</span>
        <!-- <span class="depot-name-alt">{{ data.name }}</span> -->

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

export default {
  name: 'Depot',
  props: [
    'data',
    'selected',
    'isNew',
  ],
  computed: {
    show() {
      return this.isNew ||
        (this.$store.state.editor.displayMode === 'floor' && this.data.floor === this.$store.state.editor.floor) ||
        (this.$store.state.editor.displayMode === 'depot' && this.data.depot === this.$store.state.editor.depot.id);
    }
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

<style>

.node.node-Depot .depot-name {
  z-index: 1;
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  color: rgba(255,255,255,.5);
  max-width: 100%;
  padding: 0 15px;
  box-sizing: border-box;
}

/*.node.node-Depot .depot-name-alt {
  font-size: 12px;
  position: absolute;
  top: 0;
  left: 0;
  color: #FFF;
  z-index: 2;
  padding: 1px 3px;
  background-color: rgba(0,0,0,.3);
}*/
</style>
