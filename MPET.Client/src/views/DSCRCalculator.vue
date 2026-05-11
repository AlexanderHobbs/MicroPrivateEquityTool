<script setup>

import DSCRCalculator from '@/services/DSCRCalculator.vue';
import { ref } from 'vue';

const prop = defineProps({
    id: {crypto}
})

const calculationResults = ref(null)
const successful = ref(false)

function DscrResults(data){
    calculationResults.value = data;
    successful.value = true
}

</script>

<template>
    <div class = "parent">

        <div class="input">
            <DSCRCalculator :sessionId = "prop.id" @calculate = "DscrResults"/>
        </div>

        <div class="output" v-if = "successful">
            
        </div> 

    </div>
</template>

<style scoped>
.parent {
    display: flex;
    flex-direction: column;
    width: 100%;
    height: 100%;
    background: #f2f2f2;
    border-radius: 12px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 2px 6px rgba(0,0,0,0.04);
    box-sizing: border-box;
    padding: 40px;
}

.input{
    width: 100%;
    box-sizing: border-box;
}

.output {
    display: flex;
    flex-direction: column;
    width: 100%;
    gap: 10%;
    padding: 40px 20px;
    border-radius: 14px;
    box-sizing: border-box;
    background: rgba(255, 255, 255);
    border: 1px solid #e5e7eb;
    box-shadow: 0 6px 14px rgba(0,0,0,0.05);
    margin-top: 50px;
}

</style>