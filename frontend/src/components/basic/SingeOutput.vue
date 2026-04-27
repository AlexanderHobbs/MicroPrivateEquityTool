<script setup>

import {ref} from 'vue'

const props = defineProps({
    label: {type: String, required: true},
    value: {type: [String, Number, null], default: null},
    style: {type: String, default: "output-container"}
})

</script>

<template>

<div :class="style">
    <span class="label">{{label}}</span>
    <span class="value">{{value}}</span>
</div>

</template>

<style scoped>


.output-container {
    background: #f9fafb;
    padding: 10px;
    border-radius: 10px;
    display: flex;
    justify-content: space-between;
    font-size: 13px;
}

.output-container .label {
    color: #6b7280;
    font-weight: 500;
}

.output-container .value {
    color: #111827;
    font-weight: 600;
}

</style>