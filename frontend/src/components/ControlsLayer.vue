<template>
  <div class="controls">
    <div class="pallete tools">
      <div class="pallete-item"
           :class="{selected: isDefaultMode}"
           key="setDefaultMode"
           @click="setDefaultMode"
      >
        <img class="pallete-item-icon" :src="tool.icon"/>
        <span class="pallete-item--name">Режим курсора</span>
      </div>

      <div class="pallete-item"
           v-for="(tool, index) in tools"
           :key="index"
           :class="{selected: isAddingNodeMode && index == selectedTool}"
           @click="toolSelect(index)"
      >
        <img class="pallete-item-icon" :src="tool.icon" v-if="tool.icon"/>
        <span class="pallete-item--name">{{ tool.name }}</span>
      </div>

      <div class="pallete-item"
           key="autoComputeEdges"
           @click="autoComputeEdges()"
      >
        <img class="pallete-item-icon" :src="tool.icon"/>
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
            name: 'Участок',
            key: 'Machine',
            icon: require('../assets/logo.png')
          },
          {
            name: 'Узел',
            key: 'Node',
            icon: ''
          },
          {
            name: 'Лестница',
            key: 'Ladder',
            icon: ''
          },
          {
            name: 'Лифт',
            key: 'Elevator',
            icon: ''
          },
          {
            name: 'Дверь',
            key: 'Door',
            icon: ''
          }
        ],
        selectedTool: false,

        _floor: null,
        floors: 3,
        selectedFloor: false
      };
    },
    computed: {
      isDefaultMode() {
        return ['default', 'nodeSelected', 'edgeSelected'].includes(this.$store.state.depotEditor.mode);
      },
      isAddingNodeMode() {
        return ['addingNode', 'addingEdge'].includes(this.$store.state.depotEditor.mode);
      }
    },
    mounted() {
        if (!this.tool) this._tool = 0;
        else this._tool = this.tool;
        this.selectedTool = this._tool;
        this.toolSelect(this.selectedTool);

        if (!this.floor) this._floor = 1;
        else this._floor = this.floor;
        this.selectedFloor = this._floor;
        this.floorSelect(this.selectedFloor);
    },
    methods: {
      setDefaultMode() {
        this.$store.commit('depotEditor/setDefaultMode');
      },
      toolSelect(index) {
        this.selectedTool = index;
        this.$root.$emit('toolSelected', this.tools[index]);
      },
      floorSelect(floor) {
        this.selectedFloor = floor;
        this.$root.$emit('floorSelected', floor);
      },
      autoComputeEdges() {
        this.$store.dispatch('depotEditor/autoComputeEdges');
      }
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
    justify-content: start;
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
    margin-right: 10px;
    max-height: 25px;
  }

  .floors .pallete-item {
    justify-content: center;
  }

</style>
