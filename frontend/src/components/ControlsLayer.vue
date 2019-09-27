<template>
    <div class="controls">
        <div class="floors">
            <div class="floors-heading">Этаж</div>
            <div class="floor" v-for="floor in floors" :key="floor" :class="{selected: floor === selectedFloor}" @click="selectedFloor = floor">
                {{ floor }}
            </div>
            <div class="floor floor-new" :class="{selected: selectedFloor === 'new'}" @click="selectedFloor = 'new'">+</div>
        </div>
    </div>
</template>

<script>

export default {
    name: 'ControlsLayer',
    data() {
        return {
            floors: 3,
            selectedFloor: false
        };
    },
    methods: {

    }
}

</script>

<style>

.floors {
    position: absolute;
    top: 15px;
    right: 15px;
    border-radius: 5px;
    box-shadow: 0 2px 5px 0 rgba(0, 0, 0, 0.1);
    padding: 5px 0;
}
.floors-heading {
    padding: 10px;
    font-size: 13px;
}
.floor {
    cursor: pointer;
    padding: 10px 15px;
    position: relative;
}
.floor.selected {
    font-weight: 600;
}
.floor:before {
    content: "";
    display: block;
    border: 8px solid transparent;
    position: absolute;
    right: 0;
    top: 50%;
    margin-top: -8px;
}
.floor:hover:before {
    border-right-color: #F1F1F1;
}
.floor.selected:before {
    border-right-color: #E5E5E5;
}

</style>