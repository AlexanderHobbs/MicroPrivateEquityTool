<script setup>
import StressTest from '@/services/StressTest.vue';

const props = defineProps({
    id: String
});

const emit = defineEmits(['navigateTo'])

const navigateToDebtPage = (data) => {
    showDebtPopup.value = false;
    emit('navigateTo', data);
};

</script>

<template>
    <div class="parent">
        <h2>Calculate Stress Test</h2>

        <StressTest 
            :sessionId = "props.id" 
            @navigateTo = "navigateToDebtPage"
        />
        
    </div>
</template>

<style scoped>
.parent {
    display: flex;
    flex-direction: column;
    width: 100%;
    height: 100%;
    box-sizing: border-box;
    padding: 30px;
    gap: 22px;
}
</style>