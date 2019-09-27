<template>
    <div class="controls">
        <div class="floors">
            <div class="floors-heading">Этаж</div>
            <div class="floor" v-for="(floor, index) in floors" :key="index" :class="{selected: floor === selectedFloor}">
                {{ floor }} / 
                {{ index }}
            </div>
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

</style>