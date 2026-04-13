<script setup>

import {ref} from 'vue'

const selectedYear = ref()
const selectedYear_Options = ref([
    {text: '2025', value: 2025},
    {text: '2024', value: 2024},
    {text: '2023', value: 2023},
])

</script>

<template>

<div class = "parent-container">
    <div class = "selected-year-input">
        <select name="SelectedYearInput" id="InputYear" v-model = "selectedYear">
            <option disabled value="">Select a Year</option>
            <option v-for = "year in selectedYear_Options" :key = "year.value" :value = "year.value">
                {{ year.text }}
            </option>
        </select>
        <p>Selected Year: {{ selectedYear }}</p>
    </div>
</div>



</template>

<style scoped>

</style>