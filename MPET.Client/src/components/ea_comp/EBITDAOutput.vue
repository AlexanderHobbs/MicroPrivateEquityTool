<script setup>

const props = defineProps({
    label: { type: String, required: true },
    data: {
        type: Object,
        default: () => ({})
    }
});


</script>

<template>

    <div class = "output-container">
        <div><span>Interest Rate: </span><span>{{ data.InterestRate }}</span></div>
        <div><span>Taxes: </span><span>{{ data.Taxes }}</span></div>
        <div><span>Depreciation: </span><span>{{ data.Depreciation }}</span></div> 
        <div><span>Amortization: </span><span>{{ data.Amortization }}</span></div>
    </div>

</template>

<style scoped>

.output-container {
    display: flex;
    flex-direction: column;
    gap: 22px;
    margin-bottom: 22px;
}

</style>