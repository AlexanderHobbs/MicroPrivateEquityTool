<script setup>

import DebtPaymentInput from '@/services/DebtPaymentInput.vue';

</script>

<template>
    <div class="parent">
        <div class="input">
            <DebtPaymentInput />
        </div>
    </div>
</template>

<style scoped>

.parent {
    display: flex;
    width: 100%;
    height: 100%;
    background: #f2f2f2;
    border-radius: 12px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 2px 6px rgba(0,0,0,0.04);
    box-sizing: border-box;
}

.input{
    width: 100%;
    display: flex;
    flex-direction: column;
    box-sizing: border-box;
}

</style>