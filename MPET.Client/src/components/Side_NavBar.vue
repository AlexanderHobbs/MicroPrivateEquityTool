<script setup>

import {ref} from 'vue' 

const emit = defineEmits('change-page')

const isOpen = ref(true)
// var navbar_logo = ref("←");

// const toggleNav = () => {
//     isOpen.value = !isOpen.value
//     navbar_logo.value = isOpen.value ? "←" :  "→" ;
// }

</script>

<template>
<nav :class="['navbar', { closed: !isOpen }]">
    <div class = "navbar-links" v-show = "isOpen">
        <!-- <div class = "logo"><img src="@/assets/logo.png" alt="logo"></div> -->
        <ul>
            <li><button @click = "$emit('change-page', 'dashboard')"><img src = "../assets/navbar-icons/dashboard.png"/>Dashboard</button></li>
            <li><button @click = "$emit('change-page', 'earning')"><img src = "../assets/navbar-icons/earning.png"/>Earning Calculator</button></li>
            <li><button @click = "$emit('change-page', 'debt')"><img src = "../assets/navbar-icons/debt.png"/>Debt Payment</button></li>
            <li><button @click = "$emit('change-page', 'dscr')"><img src = "../assets/navbar-icons/dscr.png"/>DSCR calculator</button></li>
            <li><button @click = "$emit('change-page', 'stress')"><img src = "../assets/navbar-icons/stress-test.png"/>Stress Test Tool</button></li>
            <li><button @click = "$emit('change-page', 'breakEven')"><img src = "../assets/navbar-icons/break-even.png"/>Break-even analysis</button></li>
            <li><button @click = "$emit('change-page', 'summary')"><img src = "../assets/navbar-icons/summary.png"/>Summary Dashboard</button></li>
            <li><button @click = "$emit('change-page', 'comparison')"><img src = "../assets/navbar-icons/comparison.png"/>Scenario Comparison</button></li>
        </ul>
    </div>
    <!-- <div class = "toggle-navBar-btn">
        <button @click = "toggleNav">{{ navbar_logo }}</button>
    </div> -->
</nav>


</template>

<style scoped>

.navbar {
    display: flex;
    flex-direction: row; /* critical for sidebar layout */
    width: 200px;
    height: 100%;

    background: #f2f2f2;
    color: black;

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
    display: flex;
    flex-direction: column;
    width: 100%;
    flex: 1;
    padding: 25px 15px;
    
}

.navbar-links ul {
    list-style: none;
    padding: 0;
    margin: 0;
}

.navbar-links li {
    margin-bottom: 20px;
}

.navbar-links button {
    width: 100%;
    text-align: left;
    display: flex;
    align-items: center;
    gap: 5px;
    padding: 6px 0px;
    margin-bottom: 6px;
    border: none;
    border-radius: 10px;
    background: transparent;
    color: black;
    font-size: 13px;
    cursor: pointer;

    transition: background 0.2s ease, color 0.2s ease, transform 0.15s ease;
}

.navbar-links button:hover {
    background: rgba(255, 255, 255, 0.10);
    color: gray;
    /* transform: translateX(3px); */
}

.navbar-links img {
    width: 20px;
    height: auto;
    transition: 0.2s ease;
}

.navbar-links button:hover img {
    opacity: .4;
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

.navbar-links button.active:hover {
    background: rgba(59, 130, 246, 0.25);
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


</style>