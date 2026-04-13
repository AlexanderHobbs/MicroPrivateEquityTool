<script setup>

import {ref} from 'vue'

const isOpen = ref(false)
</script>

<template>

<div class="parent-container">

    <div class = "navbar-links" v-show = "isOpen">
        <ul>
            <li><h4>Earning Calc</h4></li>
            <li><h4>Loss Report</h4></li>
            <li><h4>Financial Sumarry</h4></li>
            <li><h4>History</h4></li>
        </ul>
       
    </div>

    <div class = "toggle-navBar-btn">
        <button @click = "isOpen = !isOpen"> Toggle NavBar </button>
    </div>

</div>

</template>

<style scoped>

.parent-container{
    display: flex;
}

.navbar-links{
    font: 1em sans-serif;
    border: 1px solid black ;
    flex: 3;
}

.toggle-navBar-btn {
    flex: 1;
    display: flex;
    align-self: center;
    justify-items: center;
    transform: rotate(90deg);
}

button{
    width: fit-content;
}

</style>