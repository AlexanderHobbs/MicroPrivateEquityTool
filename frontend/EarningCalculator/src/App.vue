<script setup>
import earningCalculator from '@/views/EarningCalculator.vue';
import Top_NavBar from './components/Top_NavBar.vue';
import Side_NavBar from './components/Side_NavBar.vue';
</script>

<template>

<div class = "parent-container">

    <div class = "top-nav">
    <Top_NavBar />
    </div>
    <div class = "hero-section-ea">
        <Side_NavBar />
        <div class="vertical-line"></div>
        <earningCalculator />
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
}

.hero-section-ea {
    width: 100%;
    height: 100%;
    display: flex;
    flex-direction: row;

}

.vertical-line {
  border-left: 2px solid #ccc;
  height: 100; /* Set desired height */
  margin: 0 20px;
}


</style>