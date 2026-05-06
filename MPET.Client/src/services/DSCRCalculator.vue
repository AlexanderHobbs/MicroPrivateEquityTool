<script setup>

import {ref} from 'vue'
import SingleInput from '@/components/basic/SingleInput.vue';


const emit = defineEmits(['save', 'results']);

async function saveData() {

    const dscrData = {
      
    }

    if(!dataIsNotNull(dscrData)){
        alert("Not all fiels have data entered — Please fill in all entries!");
        return;
    }

    try{
        
        const response = await fetch('http://localhost:5000/api/debt/calculate', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(dscrData)
        });

        emit('save', dscrData);

        if (!response.ok) {
            const errorText = await response.text(); // Try to get server error details
            return;
        }

        const data = await response.json();
        emit('results', data)

    }catch(err){
        console.error("API Error:", err);
        alert(`API call failed: ${err.message}`);
    }

}

function dataIsNotNull(obj) { 
    for(let key in obj){ 
        if(obj[key] === null){ 
            return false; 
        } 
        if(typeof obj[key] === 'object' && !dataIsNotNull(obj[key])){ 
            return false; } 
    } 
    
    return true;
}

</script>

<template>
    <div class = "parent-conatiner">

    </div>
</template>

<style scoped>

.parent-conatiner {
    display: flex;
    box-sizing: border-box;
}

</style>