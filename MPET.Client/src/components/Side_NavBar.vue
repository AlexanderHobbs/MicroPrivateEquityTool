<script setup>

import {ref} from 'vue' 

const emit = defineEmits('change-page')

const activePage = ref('dashboard')

function changePage(pageName){
    activePage.value = pageName;
    emit('change-page', pageName);
}

</script>

<template>
<nav class= "navbar">
    <div class = "navbar-links">
        <ul>
            <li><button :class = "{active: activePage === 'dashboard'}" @click = "changePage('dashboard')"> <img src = "../assets/navbar-icons/dashboard.png"/>Dashboard</button></li>
            <li><button :class = "{active: activePage === 'earning'}" @click = "changePage('earning')"> <img src = "../assets/navbar-icons/earning.png"/>Earning Calc</button></li>
            <li><button :class = "{active: activePage === 'debt'}" @click = "changePage('debt')"> <img src = "../assets/navbar-icons/debt.png"/>Debt Payment</button></li>
            <li><button :class = "{active: activePage === 'dscr'}" @click = "changePage('dscr')"> <img src = "../assets/navbar-icons/dscr.png"/>DSCR Calc</button></li>
            <li><button :class = "{active: activePage === 'stress'}" @click = "changePage('stress')"> <img src = "../assets/navbar-icons/stress-test.png"/>Stress Test</button></li>
            <li><button :class = "{active: activePage === 'breakEven'}" @click = "changePage('breakEven')"> <img src = "../assets/navbar-icons/break-even.png"/>Break-even</button></li>
            <li><button :class = "{active: activePage === 'summary'}" @click = "changePage('summary')"> <img src = "../assets/navbar-icons/summary.png"/>Summary</button></li>
            <li><button :class = "{active: activePage === 'comparison'}" @click = "changePage('comparison')"> <img src = "../assets/navbar-icons/comparison.png"/>Scenarios</button></li>
        </ul>
    </div>
</nav>


</template>

<style scoped>

.navbar {
    display: flex;
    flex-direction: row; /* critical for sidebar layout */
    width: 100%;
    height: 100%;
    background: #f2f2f2;
    color: black;
    border-radius: 20px;
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
    display: flex;
    flex-direction: column;
    width: 100%;
    flex: 1;
    margin-top: 25px;
}

.navbar-links ul {
    list-style: none;
    padding: 0;
    margin: 0;
}

.navbar-links li {
    margin-bottom: 25px;
}

.navbar-links button {
    padding: 10px 20px;
    width: 100%;
    display: flex;
    text-align: left;
    align-items: center;
    gap: 15px;
    margin-bottom: 6px;
    border: none;
    color: black;
    font-size: 15px;
    cursor: pointer;
    background-color: transparent;

    transition: background 0.2s ease, color 0.2s ease, transform 0.15s ease;
}

.navbar-links button:hover {
    color: gray;
    /* transform: translateX(3px); */
}

button.active {
    background-color: rgba(174, 176, 180, 0.25);
    border-left: 4px solid;
    border-image: linear-gradient(to bottom, #f5c518, #c9920a) 1;
}

.navbar-links button.active:hover {
    color: black;
}

.navbar-links button.active:hover img {
    opacity: 1;
}

.navbar-links img {
    width: 12.5%;
    height: auto;
    transition: 0.2s ease;
}

.navbar-links button:hover img {
    opacity: .4;
}

.navbar-links button::after {
    background-color: rgba(0, 145, 255, 0.277);
}


/* Toggle button container */
.toggle-navBar-btn {
    display: flex;
    flex: .1;
    align-items: center;
    padding: 1px;
    right: 1px;
}

.toggle-navBar-btn button {
    border: none;
    color: #1f2937;
    padding: 4px;
    border-radius: 6px;
    cursor: pointer;
    height: auto;
}

.toggle-navBar-btn button:hover {
    background: #374151;
    color: white;
}

.logo img {
    width: 44px;
    border-radius: 10px;

    border: 1px solid rgba(255,255,255,0.08);
    transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.logo img:hover {
    transform: scale(1.06);
    box-shadow: 0 6px 15px rgba(0,0,0,0.16);
}

/* Click feedback */
.logo img:active {
    transform: scale(0.95);
}

/* .navbar-links button:active {
    background: rgba(59, 130, 246, 0.18);
    color: #60a5fa;
    font-weight: 500;

    position: relative;
} */

/* .navbar-links button:active::before {
    content: "";
    position: absolute;
    left: -6px;
    top: 50%;
    transform: translateY(-50%);

    width: 3px;
    height: 60%;
    border-radius: 6px;

    background: #3b82f6;
} */

</style>

