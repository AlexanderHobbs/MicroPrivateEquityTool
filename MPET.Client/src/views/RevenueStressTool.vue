<script setup>
import StressTest from '@/services/StressTest.vue';

const props = defineProps({
    id: String
});

</script>

<template>
    <div class="parent">
        <StressTest :sessionId = "props.id"/>
    </div>
</template>

<style scoped>
.parent {
    display: flex;
    flex-direction: row;
    width: 100%;
    height: 100%;
    background: #f2f2f2;
    border-radius: 12px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.04);
    box-sizing: border-box;
    padding: 40px;
}
</style>