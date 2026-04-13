<script setup>
import navBar from '@/components/Side_NavBar.vue';
import earningInput from '@/components/earningInput.vue';


import {ref} from 'vue'
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
    height: 100vh;
}

.input{
    flex: 75;
    background-color: #f0f0f0;
    padding: 20px;
    border: 1px solid #111;
}

.navbar{
    flex: 25;
    background-color: #f0f0f0;
    padding: 1px;
    border: 1px solid #111;
}

.child-div:hover {
    transition: 1s;
    background-color: #ccc;
}

.vertical-line {
  border-left: 2px solid #ccc;
  height: 100; /* Set desired height */
  margin: 0 20px;
}




</style>