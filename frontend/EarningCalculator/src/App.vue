<script setup>

import {ref, computed} from 'vue'

import Top_NavBar from './components/Top_NavBar.vue';
import Side_NavBar from './components/Side_NavBar.vue';

import EarningCalculator from '@/views/EarningCalculator.vue';
import DebtPayment from './views/DebtPaymentCalculator.vue';
import DSCRCalculator from './views/DSCRCalculator.vue';
import RevenueStressTool from './views/RevenueStressTool.vue';
import BreakEvenAnalysis from './views/BreakEvenAnalysis.vue';
import SummaryDashboard from './views/SummaryDashboard.vue';
import ScenarioComparison from './views/ScenarioComparison.vue';

const pageNum = ref(0)

const component = [
    EarningCalculator,
    DebtPayment,
    DSCRCalculator,
    RevenueStressTool,
    BreakEvenAnalysis,
    SummaryDashboard,
    ScenarioComparison
]

const currentPageComponent = computed(() => component[pageNum.value]);


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
        
        <div class = "input-section">
            <Transition name = "fade" mode = "out-in">
                <component :is = "currentPageComponent" />
            </Transition>
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
.parent-container {
    display: flex;
    flex-direction: column;
    gap: 20px;
    padding: 10px;
    box-sizing: border-box;
    background-color: white;
    box-sizing: border-box;
}


.hero-section-ea {
    width: 100%;
    height: 100%;
    display: flex;
    flex-direction: row;
    gap: 30px;
}

.vertical-line {
  border-left: 2px solid #ccc;
  height: 100; /* Set desired height */
  margin: 0 20px;
}

.input-section {
    width: 100%;
}

.fade-enter-active,
.fade-leave-active {
  transition: 0.2s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: .25;
}


</style>