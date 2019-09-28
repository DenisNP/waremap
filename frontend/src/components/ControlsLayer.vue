<template>
  <div class="controls">
    <div class="pallete tools">
      <div class="pallete-item"
        v-for="(tool, index) in tools"
        :key="index"
        :class="{selected: index == selectedTool}"
        @click="toolSelect(index)"
      >
        <div v-if="tool.icon" class="pallete-item-icon"><img :src="tool.icon" v-if="tool.icon"/></div>
        <span class="pallete-item--name">{{ tool.name }}</span>
      </div>

      <div class="pallete-item"
        key="autoComputeEdges"
        @click="autoComputeEdges()"
      >
        <div class="pallete-item-icon"><img src="@/assets/edge.svg"/></div>
        <span class="pallete-item--name">Заполнить связи</span>
      </div>
    </div>

    <div class="pallete pallete-right floors">
      <div class="pallete-heading">Этаж</div>
      <div class="pallete-item"
        v-for="floor in floors"
        :key="floor"
        :class="{selected: floor == selectedFloor}"
        @click="floorSelect(floor)"
      >
        <span class="pallete-item-name">
          {{ floor }}
        </span>
      </div>
      <div class="pallete-item" :class="{selected: selectedFloor === 'new'}" @click="selectedFloor = 'new'">+</div>
    </div>
  </div>
</template>

<script>

export default {
  name: 'ControlsLayer',
  props: [
    'floor',
    'tool'
  ],
  data() {
    return {
      _tool: null,
      tools: [
        {
          name: 'Режим курсора',
          key: 'setDefaultMode',
          icon: require('../assets/cursor_02.svg')
        },
        {
          name: 'Участок',
          key: 'Machine',
          icon: require('../assets/machine-tool.svg')
        },
        {
          name: 'Узел',
          key: 'Node',
          icon: require('../assets/node.svg')
        },
        {
          name: 'Лестница',
          key: 'Ladder',
          icon: require('../assets/stairs.svg')
        },
        {
          name: 'Лифт',
          key: 'Elevator',
          icon: require('../assets/lift.svg')
        },
        {
          name: 'Дверь',
          key: 'Door',
          icon: require('../assets/door.svg')
        },
        {
          name: 'Цех',
          key: 'Depot',
          icon: ''
        }
      ],
      selectedTool: false,

      floors: 3,
    };
  },
  computed: {
    isDefaultMode() {
      return ['default', 'nodeSelected', 'edgeSelected', 'depotSelected'].includes(this.$store.state.editor.mode);
    },
    isAddingNodeMode() {
      return ['addingNode', 'addingEdge'].includes(this.$store.state.editor.mode);
    },
    isAddingDepotMode() {
      return ['addingDepot'].includes(this.$store.state.editor.mode);
    },
    selectedFloor() {
      return this.$store.state.editor.floor;
    }
  },
  mounted() {
      if (!this.tool) this._tool = 0;
      else this._tool = this.tool;
      this.selectedTool = this._tool;
      this.toolSelect(this.selectedTool);
  },
  methods: {
    setDefaultMode() {
      this.$store.commit('editor/setDefaultMode');
    },
    toolSelect(index) {
      this.selectedTool = index;
      this.$root.$emit('toolSelected', this.tools[index]);

      if (this.tools[index].key == 'setDefaultMode') {
        this.setDefaultMode();
      }
    },
    floorSelect(floor) {
      console.log('select floor', floor);
      this.$store.commit('editor/setFloor', floor);
    },
    autoComputeEdges() {
      this.$store.dispatch('editor/autoComputeEdges');
    },
  }
}

</script>

<style>

.pallete {
  z-index: 10;
  background-color: #FFF;
  position: absolute;
  top: 15px;
  left: 15px;
  right: auto;
  padding: 5px 0;
  box-shadow: 0 2px 5px 0 rgba(0, 0, 0, .1);
  border-radius: 5px;
}

.pallete-heading {
  padding: 10px;
  font-size: 13px;
}

.pallete-right {
  left: auto;
  right: 15px;
}

.pallete-item {
  cursor: pointer;
  position: relative;
  padding: 10px 15px;
  display: flex;
  justify-content: flex-start;
  align-items: center;
}

.pallete-item.selected {
  font-weight: 600;
}

.pallete-item:before {
  content: "";
  display: block;
  border: 8px solid transparent;
  position: absolute;
  left: 0;
  right: auto;
  top: 50%;
  margin-top: -8px;
}

.pallete-item:hover:before {
  border-left-color: #F1F1F1;
}

.pallete-item.selected:before {
  border-left-color: #E5E5E5;
}

.pallete-right .pallete-item:before {
  left: auto;
  right: 0;
}

.pallete-right .pallete-item:hover:before {
  border-right-color: #F1F1F1;
  border-left-color: transparent;
}

.pallete-right .pallete-item.selected:before {
  border-right-color: #E5E5E5;
  border-left-color: transparent;
}

.pallete-item-icon {
  width: 24px;
  height: 24px;
  margin: -3px 14px -3px 0;
  display: flex;
  justify-content: center;
  align-items: center;
}
.pallete-item-icon img {
  max-width: 100%;
}



.floors .pallete-item {
  justify-content: center;
}

</style>
