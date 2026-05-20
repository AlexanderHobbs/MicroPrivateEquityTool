<script setup>
import BreakEvenService from '@/services/BreakEvenService.vue';


</script>

<template>
<section class="parent">
    <h2>Conduct a Break Even Analysis</h2>

    <BreakEvenService />
</section>

</template>

<style scoped>

.parent {
    display: flex;
    flex-direction: column;
    width: 100%;
    height: 100%;
    padding: 30px;
    box-sizing: border-box;
}

</style>