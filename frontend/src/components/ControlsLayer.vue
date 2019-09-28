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

      <label class="myLabel">
        <div class="pallete-item"
             key="uploadBackground"
        >
          <div class="pallete-item-icon"><img :src="$store.state.icons.pallete.UploadBg.i" /></div>
            <input type="file" @change="onFileSelected" accept=".jpg, .jpeg, .png"/>

            <span class="pallete-item--name">
              Загрузить план
            </span>
        </div>
      </label>
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
import * as helpers from '../common/helpers';

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
          icon: this.$store.state.icons.pallete.Cursor.i
        },
        {
          name: 'Участок',
          key: 'Machine',
          icon: this.$store.state.icons.pallete.Machine.i
        },
        {
          name: 'Узел',
          key: 'Node',
          icon: this.$store.state.icons.pallete.Node.i
        },
        {
          name: 'Лестница',
          key: 'Ladder',
          icon: this.$store.state.icons.pallete.Stairs.i
        },
        {
          name: 'Лифт',
          key: 'Elevator',
          icon: this.$store.state.icons.pallete.Elevator.i
        },
        {
          name: 'Дверь',
          key: 'Door',
          icon: this.$store.state.icons.pallete.Door.i
        },
        {
          name: 'Цех',
          key: 'Depot',
          icon: this.$store.state.icons.pallete.Depot.i
        },
        {
          name: 'Сгенерировать связи',
          key: 'autoComputeEdges',
          icon: this.$store.state.icons.pallete.Edges.i
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
    async onFileSelected(e) {
      const base64 = await helpers.toBase64(e.target.files[0]);
      await this.$store.dispatch('editor/uploadFloorBackground', base64);
      console.log(base64.length, 'bytes uploaded');
    },
    setDefaultMode() {
      this.$store.commit('editor/setDefaultMode');
    },
    toolSelect(index) {
      this.selectedTool = index;
      this.$root.$emit('toolSelected', this.tools[index]);

      if (this.tools[index].key == 'setDefaultMode') {
        this.setDefaultMode();
      }
      if (this.tools[index].key == 'autoComputeEdges') {
        this.autoComputeEdges();
      }
    },
    async floorSelect(floor) {
      console.log('select floor', floor);
      this.$store.commit('editor/setFloor', floor);
      await this.$store.dispatch('editor/downloadFloorBackground');
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
  background-color: #4D4D4D;
  color: #FFF;
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
  padding: 13px 18px;
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
  width: 32px;
  height: 32px;
  margin: -4px 14px -4px 0;
  display: flex;
  justify-content: center;
  align-items: center;
}
.pallete-item-icon img {
  max-width: 100%;
}

.pallete.tools {
    left: 0;
    top: 0;
    height: 100%;
    overflow-y: auto;
    overflow-x: hidden;
    box-sizing: border-box;
}
::-webkit-scrollbar {
  width: 14px;
}
::-webkit-scrollbar-track {
  width: 14px;
}
::-webkit-scrollbar-thumb {
  width: 14px;
  background-color: #999;
  border: 4px solid #4D4D4D;
  border-radius: 7px;
}



.floors .pallete-item {
  justify-content: center;
}

label.myLabel input[type="file"] {
  position:absolute;
  top: -1000px;
}

label.myLabel {
  cursor: pointer;
}

</style>
