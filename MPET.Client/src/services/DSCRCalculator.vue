<script setup>

import {onMounted, ref} from 'vue'
import SingleInput from '@/components/basic/SingleInput.vue';


async function loadData() {
    try{
    const response = await fetch("http://localhost:5000/api/dscr/controller", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(debtData)
    })

    const data = await response.json();

    }catch(err){
        console.log(err)
    }
}

onMounted(loadData);

</script>

<template>
    <div class = "parent-conatiner">
        <div>
            <SingleInput />
            <SingleInput />
            <SingleInput />
        </div>

        <div>

        </div>
    </div>
</template>

<style scoped>

.parent-conatiner {
    display: flex;
    box-sizing: border-box;
}

</style>

<!-- 
DSCR calculator (Debt Service Coverage Ratio)
Will the bank approve this?
Profit / Annual loan payment (want above 1.25)
Input (carry over):
Profit
Annual loan payment 
Preferred DSCR level
Output:
DSCR calculation
Green / yellow / red status
Cash flow remaining after debt
Warning if below preferred level
 -->
