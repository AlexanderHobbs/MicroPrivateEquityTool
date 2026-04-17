<script setup>
import navBar from '@/components/Side_NavBar.vue';
import earningInput from '@/components/earningInput.vue';

import {ref} from 'vue';

const isOpen = ref(false)

</script>

<template>

    <div class = "parent-container">
        
        <div class = "navbar">
            <navBar/>
        </div>


        <div class="vertical-line"></div>

        <div class = "input">
            <earningInput/>
        </div>

    </div>

    
</template>

<style scoped>

.parent-container {
    display: flex;
    width: 100%;
    height: 100%;
}

.vertical-line {
  border-left: 2px solid #ccc;
  height: 100; /* Set desired height */
  margin: 0 20px;
}

.input{
    width: 100%;
}




</style>