<script setup>
import {ref} from 'vue'
import earningCalculator from '@/views/EarningCalculator.vue';
import Top_NavBar from './components/Top_NavBar.vue';
import Side_NavBar from './components/Side_NavBar.vue';

import FinancialSummary from './views/FinancialSummary.vue';
import LossReport from './views/LossReport.vue';

const pageNum = ref(0)

</script>

<template>

<div class = "parent-container">

    <div class = "top-nav">
        <Top_NavBar class = "side-nav"/>
    </div>

    <div class = "hero-section-ea">

        <div class = "side-nav-bar">
            <Side_NavBar @change-page = "(n) => pageNum = n"/>
        </div>
        
        <div class = "input-section" v-if = "pageNum === 0">
            <earningCalculator />
        </div>
        <div class = "input-section" v-else-if = "pageNum === 1">
            <FinancialSummary />
        </div>
        <div class = "input-section" v-else-if = "pageNum === 2">
            <LossReport />
        </div>
    </div>

</div>



<!-- on app load
 check all endpoints
 check backend status and security
 check data base status and integration
 load front end
 load navigation / router
 -->

</template>

<style scoped>

.app {
    background-color: red;
}

.parent-container {
    display: flex;
    flex-direction: column;
    gap: 20px;
    box-sizing: border-box;
    background-color: white;
    box-sizing: border-box;
}


.hero-section-ea {
    width: 100%;
    height: 100%;
    display: flex;
    flex-direction: row;
    gap: 20px;
}

.vertical-line {
  border-left: 2px solid #ccc;
  height: 100; /* Set desired height */
  margin: 0 20px;
}

.input-section {
    width: 100%;
}


</style>