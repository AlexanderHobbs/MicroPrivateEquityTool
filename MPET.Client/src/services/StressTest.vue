<script setup>
import SingeOutput from '@/components/basic/SingeOutput.vue';
import SingleInput from '@/components/basic/SingleInput.vue';
const props = defineProps({sessionId: crypto})

</script>

<template>
    <div class="parent-container">
        <div class="input-container">
            <h4>Calculate Stress Revenue</h4>
            <SingleInput label = "Nerve Levels" :inputType = "4"/>
            <SingleInput label = "Revenue" :inputType = "4"/>
            <SingleInput label = "Expense" :inputType = "4"/>
            <SingleInput label = "Revenue Drop (%)" :inputType = "4"/>
            <SingleInput label = "Margin Levels Shift" :inputType = "4"/>
            <SingleInput label = "Interest Rate Change" :inputType = "4"/>
        </div>

        <div class="output-container">
            <SingeOutput />
        </div>

    </div>

</template>

<style scoped>
.parent-container {
    flex: 1;
    display: flex;
    gap: 22px;
    box-sizing: border-box;
}

.output-container {
    display: flex;
    flex: 1;
    flex-direction: column;
    box-sizing: border-box;
}

.input-container {
    display: flex;
    flex: .75;
    flex-direction: column;
    gap: 22px;
    box-sizing: border-box;
}
</style>