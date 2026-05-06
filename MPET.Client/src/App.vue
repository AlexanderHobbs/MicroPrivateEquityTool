<script setup>

import {ref, computed} from 'vue'

import Top_NavBar from './components/Top_NavBar.vue';
import Side_NavBar from './components/Side_NavBar.vue';

import Settings from './views/Settings.vue';

import Dashboard from './views/Dashboard.vue';
import EarningCalculator from '@/views/EarningCalculator.vue';
import DebtPayment from './views/DebtPaymentCalculator.vue';
import DSCRCalculator from './views/DSCRCalculator.vue';
import RevenueStressTool from './views/RevenueStressTool.vue';
import BreakEvenAnalysis from './views/BreakEvenAnalysis.vue';
import SummaryDashboard from './views/SummaryDashboard.vue';
import ScenarioComparison from './views/ScenarioComparison.vue';

const currentPage = ref('earning')
const previousPage = ref()

const pages = {
    dashboard: Dashboard,
    earning: EarningCalculator,
    debt: DebtPayment,
    dscr: DSCRCalculator,
    stress:RevenueStressTool,
    breakEven: BreakEvenAnalysis,
    summary:SummaryDashboard,
    comparison: ScenarioComparison,
    settings: Settings
}

function changePage(page){
    if(currentPage.value !== 'settings'){
        previousPage.value = currentPage.value
    }
    currentPage.value = page;
}

function goBackPage(){
    currentPage.value = previousPage.value
}

const currentPageComponent = computed(() => pages[currentPage.value]);


</script>

<template>

<div class = "parent-container">

    <div class = "top-nav">
        <Top_NavBar class = "side-nav" @change-page = "changePage"/>
    </div>

    <div class = "hero-section-ea" v-if = "currentPage !== 'settings'">

        <div class = "side-nav-bar">
            <Side_NavBar @change-page = "changePage"/>
        </div>
        
        <div class = "input-section">
            <Transition name = "fade" mode = "out-in">
                <component :is = "currentPageComponent" />
            </Transition>
        </div>
    </div>
    <div class = "hero-section-ea" v-else>
        <Settings @close = "goBackPage"/>
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
    gap: 10px;
    padding: 10px;
    background: rgb(247, 247, 247);
    box-sizing: border-box;
}


.hero-section-ea {
    width: 100%;
    height: 100%;
    display: flex;
    flex-direction: row;
    gap: 30px;
    box-sizing: border-box;
}

.vertical-line {
  border-left: 2px solid #ccc;
  height: 100; /* Set desired height */
  margin: 0 20px;
}

.side-nav-bar{
    flex: 1;
}

.input-section {
    width: 100%;
    flex: 7;
}

.fade-enter-active,
.fade-leave-active {
  transition: 0.175s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: .25;
}


</style>