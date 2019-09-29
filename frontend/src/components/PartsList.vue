<template>
  <div class="pallete pallete-right PartsList">
    <div class="scrollable">
      <template v-for="group in assemblies">
        <div
          class="pallete-heading"
          :class="{'selected-item': selectedAssemblyId === group.id}"
          @click="showAssemblyNodes(group.id)">{{ group.name }}
        </div>
        <div class="pallete-item"
          v-for="detail in group.details"
          :key="detail.id"
          :class="{'selected-item': selectedDetailId === detail.id}"
          @click="showDetailNodes(detail)"
        >
          <span class="pallete-item-icon">&laquo;</span>
          <span class="pallete-item-name">
            {{ detail.name }}
          </span>

          <OperationList v-if="selectedDetailId === detail.id" :detail="detail" />
        </div>
      </template>
    </div>
  </div>
</template>

<script>
  import OperationList from './OperationList.vue';

  export default {
    name: 'PartsList',
    components: {
      OperationList,
    },
    props: [
      'data',
    ],
    data() {
      return {
        selectedDetailId: null,
        selectedAssemblyId: null,
      };
    },
    computed: {
      assemblies() {
        let assemblies = {};
        this.$store.state.serverState.equipment.parts.map((detail) => {
          if (!(detail.assembly_id in assemblies)) {
            assemblies[detail.assembly_id] = {
              id: detail.assembly_id,
              name: '',
              details: [detail]
            };
          } else {
            assemblies[detail.assembly_id].details.push(detail);
          }
        });
        Object.keys(assemblies).map((id) => {
          let assembly = this.$store.state.serverState.equipment.assemblies.find(r => r.id == id);
          if (assembly && assembly.name) {
            assemblies[id].name = assembly.name;
          } else {
            assemblies[id].name = 'Детали без сборки';
          }
        });
        return assemblies;
      }
    },
    mounted() {
    },
    methods: {
      async showAssemblyNodes(id) {
        this.selectedDetailId = null;
        this.selectedAssemblyId = id;

        let details = this.assemblies[id].details.map(detail => detail);
        let nodesIds = {};
        details.map(detail => {
          detail.roadmap.path.map(p => {
            nodesIds[p.from_node] = true;
            nodesIds[p.to_node] = true;
          });
        });
        this.$store.commit('editor/highlightNodes', Object.keys(nodesIds));
        this.$store.commit('editor/highlightedDetails', details);
        
        let nodeId = null;
        if (Object.keys(nodesIds).length > 0) {
          nodeId = Object.keys(nodesIds)[0];
        }
        if (nodeId) {
          let node = this.$store.getters['serverState/nodeById'](nodeId);
          if (node) {
            if (!(node.floor in this.$store.state.editor.floorBackgroundMap)) {
              await this.$store.dispatch('editor/downloadFloorBackground');
            }
            this.$store.commit('editor/setFloor', node.floor);
          }
        }
      },
      showDetailNodes(detail) {
        this.selectedAssemblyId = null;
        this.selectedDetailId = detail.id;
        let nodesIds = {};
        detail.roadmap.path.map(p => {
          nodesIds[p.from_node] = true;
          nodesIds[p.to_node] = true;
        });
        this.$store.commit('editor/highlightNodes', Object.keys(nodesIds));
        this.$store.commit('editor/highlightedEdges', {edges: detail.roadmap.path});
        this.$store.commit('editor/highlightedDetails', [detail]);
      }
    }
  }

</script>

<style>
.PartsList {
  position: fixed;
}
.PartsList .scrollable {
  overflow-y: auto;
  height: calc(100% - 70px);
}
.PartsList .selected-item {
  background: rgb(56, 120, 255);
  color: white;
}
.PartsList .selected-item:hover {
  color: white !important;
}
</style>
