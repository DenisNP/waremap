<template>
  <div class="controls">
    <div class="pallete tools">
      <template v-for="(item, index) in tools">
        <div class="pallete-heading" v-if="item.heading">{{ item.heading }}</div>
        <div class="pallete-item" v-else
          :key="index"
          :class="{selected: index == selectedTool}"
          @click="toolSelect(index)"
        >
          <div v-if="item.icon" class="pallete-item-icon"><img :src="item.icon" v-if="item.icon"/></div>
          <span class="pallete-item--name">{{ item.name }}</span>
        </div>
      </template>
    </div>

    <div class="pallete pallete-top floors">
      <div class="pallete-heading">Этажи</div>
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

    <!-- <label class="myLabel">
      <div class="pallete-item"
           key="uploadBackground"
      >
        <div class="pallete-item-icon"><img :src="$store.state.icons.pallete.UploadBg.i" /></div>
          <input type="file" @change="onFileSelected" accept=".jpg, .jpeg, .png"/>

          <span class="pallete-item--name">
            Загрузить план
          </span>
      </div>
    </label> -->
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
          heading: 'Конструктор'
        },
        {
          name: 'Цех',
          key: 'Depot',
          icon: this.$store.state.icons.pallete.Depot.i
        },
        {
          name: 'Участок',
          key: 'Machine',
          icon: this.$store.state.icons.pallete.Machine.i
        },
        {
          name: 'Создать узел',
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
          name: 'Создать связи',
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
.pallete.tools {
  left: 0;
  top: 0;
  box-sizing: border-box;
  background-image: url(../assets/left.svg);
  height: 620px;
  box-shadow: none;
  background-repeat: no-repeat;
  background-position: top left;
  margin-top: 72px;
  padding: 35px 0 0;
}
.tools .pallete-item {
  padding: 11px 35px;
}
.tools .pallete-item-icon {
  margin: -16px 15px -16px 0;
}
.tools .pallete-item.selected {
  font-weight: inherit;
  color: #333;
}
.tools .pallete-item:before {
  display: none;
}
.tools .pallete-item--name {
  font-size: 18px;
  color: #F2F2F2;
}
.tools .pallete-item.selected .pallete-item--name {
  color: #333;
}
.tools .selected .pallete-item-icon img {
    transform: scale(1.2);
}
.tools .pallete-heading {
  font-size: 18px;
  color: #333;
  padding: 20px 35px 10px;
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

/* -------------------------------------------------------- */

.pallete.floors {
  padding: 0 0 4px;
  top: 0;
  left: 50%;
  width: 520px;
  font-size: 18px;
  margin: 0 -260px;
  box-shadow: none;
  border-radius: 0 0 25px 25px;
  display: flex;
  justify-content: center;
  align-items: center;
  color: #333;
  background-repeat: no-repeat;
  background-position: center top;
  background-image: url(../assets/top.svg);
}
.pallete.floors .pallete-item:before {
  bottom: 5px;
  top: auto;
  left: 50%;
  margin: 0 0 0 -8px;
}
.pallete.floors .pallete-item:hover:before {
  border-left-color: transparent;
  border-bottom-color: #EE4722;
}
.pallete.floors .pallete-item.selected {
  color: #3878FF;
}
.pallete.floors .pallete-item.selected:before {
  border-left-color: transparent;
  border-bottom-color: #EE4722;
}
.pallete.floors .pallete-heading {
  font-size: 18px;
}

/* -------------------------------------------------------------------- */

.pallete.edges {
  left: 0;
  top: 530px;
  padding-left: 30px;
  background-color: transparent;
  box-shadow: none;
  color: #333;
}
.pallete.edges .inline-group {
  display: flex;
  justify-content: flex-start;
  align-items: center;
}
.pallete.edges .pallete-heading {
  padding: 5px;
  font-size: 16px;
}
.pallete.edges .edge-type {
  padding: 5px;
  cursor: pointer;
}
.pallete.edges .edge-type.selected {
  transform: scale(1.2);
}
.pallete.edges .edge-weight-input {
  width: 95px;
  margin-left: 5px;
  box-sizing: border-box;
  padding: 3px 5px;
  border: 1px solid transparent;
  border-radius: 4px;
}

</style>
