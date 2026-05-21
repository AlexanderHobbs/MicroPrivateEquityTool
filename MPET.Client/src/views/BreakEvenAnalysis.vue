<script setup>
import BreakEvenService from '@/services/BreakEvenService.vue';

const prop = defineProps({
    id: String 
})

</script>

<template>
<section class="parent">
    <BreakEvenService 
        :sessionId = prop.id
    />
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