<script setup>

import {ref} from 'vue'

const props = defineProps({
    label: {type: String, required: true},
    value: {type: [String, Number, null], default: null},
    style: {type: String, default: "output-container"}
})

</script>

<template>

<div :class = "style">
    <span class="label">{{label}}</span>
    <span v-if = "typeof value === 'number'" class="value">{{Number(value).toFixed(2)}}</span>
    <span v-else class = "value">{{ value }}</span>
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
    font-weight: 500;
    font-size: 15px;
}

.no-border {
    padding: 10px 0px;
    display: flex;
    justify-content: space-between;
    font-size: 16px;
}

.no-border .label {
    flex: 1;
    color: #6b7280;
    font-weight: 500;
}

.no-border .value {
    flex: 1;
   color: #6b7280;
    font-weight: 600;
}

.st-output {
    background: #f9fafb;
    padding: 10px;
    border-radius: 10px;
    display: flex;
    flex-direction: column;
    font-size: 13px;
    flex: 1;
    gap: 22px;
    transition: .2s ease;
}

.st-output:hover {
  transform: scale(1.01);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
}

.st-output .label {
    color: #6b7280;
    font-weight: 500;
}

.st-output .value {
    color: #111827;
    font-weight: 600;
}



</style>