<script setup>

import {ref} from 'vue' 

const emit = defineEmits('change-page')

const isOpen = ref(true)
var navbar_logo = ref("←");

const toggleNav = () => {
    isOpen.value = !isOpen.value
    navbar_logo.value = isOpen.value ? "←" :  "→" ;
}

</script>

<template>
<nav :class="['navbar', { closed: !isOpen }]">
    <div class = "navbar-links" v-show = "isOpen">
        <div class = "logo"><img src="@/assets/logo.png" alt="logo"></div>
        <ul>
            <li><button @click = "$emit('change-page', 0)">Earning Calculator</button></li>
            <li><button @click = "$emit('change-page', 1)">Loss Report</button></li>
            <li><button @click = "$emit('change-page', 2)">Financial Summary</button></li>
        </ul>
    </div>

    <div class = "toggle-navBar-btn">
        <button @click = "toggleNav">{{ navbar_logo }}</button>
    </div>
</nav>


</template>

<style scoped>

.navbar {
    display: flex;
    flex-direction: row; /* critical for sidebar layout */
    width: 220px;
    height: 100vh;

    background: #f2f2f2;
    color: black;

    padding: 14px 12px;
    border-radius: 14px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 2px 6px rgba(0,0,0,0.04);

    overflow: hidden;

    transition: width 0.25s ease, background 0.25s ease;
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

.navbar-links button {
    width: 100%;
    text-align: left;

    display: flex;
    align-items: center;
    gap: 10px;

    padding: 10px 12px;
    margin-bottom: 6px;

    border: none;
    border-radius: 10px;

    background: transparent;
    color: black;

    font-size: 14px;
    cursor: pointer;

    transition: background 0.2s ease, color 0.2s ease, transform 0.15s ease;
}

.navbar-links button:hover {
    background: rgba(255, 255, 255, 0.10);
    color: gray;
    transform: translateX(3px);
}

.navbar-links button:active {
    background: rgba(59, 130, 246, 0.18);
    color: #60a5fa;
    font-weight: 500;

    position: relative;
}

.navbar-links button:active::before {
    content: "";
    position: absolute;
    left: -6px;
    top: 50%;
    transform: translateY(-50%);

    width: 3px;
    height: 60%;
    border-radius: 6px;

    background: #3b82f6;
}

.navbar-links button:active:hover {
    background: rgba(59, 130, 246, 0.25);
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

.logo img {
    width: 44px;
    border-radius: 10px;

    border: 1px solid rgba(255,255,255,0.08);
    transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.logo img:hover {
    transform: scale(1.06);
    box-shadow: 0 8px 18px rgba(0,0,0,0.35);
}

/* Click feedback */
.logo img:active {
    transform: scale(0.95);
}


</style>