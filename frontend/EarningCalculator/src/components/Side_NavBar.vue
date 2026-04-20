<script setup>

import {ref} from 'vue' 


const isOpen = ref(true)
var navbar_logo = ref("←");

const toggleNav = () => {
    isOpen.value = !isOpen.value
    navbar_logo.value = isOpen.value ? "←" :  "→" ;
}

</script>

<template>

    
<div :class="['navbar', { closed: !isOpen }]">
    <div class = "navbar-links" v-show = "isOpen">
        <ul>
            <li><router-link to = "/earnings">Earning Calculator</router-link></li>
            <li><router-link to = "/loss">Loss Report</router-link></li>
            <li><router-link to = "/summary">Financial Summary</router-link></li>
        </ul>
    
    </div>

    <div class = "toggle-navBar-btn">
        <button @click = "toggleNav">{{ navbar_logo }}</button>
    </div>
</div>


</template>

<style scoped>

.navbar {
    display: flex;
    flex-direction: row;
    width: 220px;
    color: black;
    transition: width 0.25s ease;
    overflow: hidden;
    height: 100vh;
    background: #ffffff;
    padding: 12px 14px;
    border-radius: 12px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 2px 6px rgba(0,0,0,0.04);

}

.navbar.closed {
    width: 60px;
}


/* Nav section */
.navbar-links {
    flex: 1;
    padding: 20px 10px;
}

.navbar-links ul {
    list-style: none;
    padding: 0;
    margin: 0;
}

.navbar-links li {
    margin-bottom: 12px;
}

router-link {
    text-decoration: none;
    color: #1f2937;
    font-size: 14px;
    display: block;
    padding: 8px 10px;
    border-radius: 8px;
    transition: background 0.2s ease;
}

router-link:hover {
    background: #1f2937;
    color: white;
}

router-link-active {
    background: #374151;
    color: white;
}

/* Toggle button container */
.toggle-navBar-btn {
    display: flex;
    align-items: center;
    padding: 10px;
}

.toggle-navBar-btn button {
    background: #1f2937;
    border: none;
    color: white;
    padding: 8px;
    border-radius: 6px;
    cursor: pointer;
    height: auto;
}

.toggle-navBar-btn button:hover {
    background: #374151;
}


</style>