<script setup>

import {onMounted, ref} from 'vue'
import SingleInput from '@/components/basic/SingleInput.vue';
import SingeOutput from '@/components/basic/SingeOutput.vue';

const props = defineProps({sessionId: {crypto}});


const savedData = ref(null);
const calculatedData = ref(null)

const prefferedDSCR = ref(1);

async function loadData() {
    try{

        const response = await fetch('http://localhost:5000/api/dscr/default', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json', 'X-Session-Id' : props.sessionId
            },
        })

        if (!response.ok) {
            const errorText = await response.text(); // Try to get server error details
            alert(`Server error: ${response.status} - ${errorText}`);
            return;
        }

        savedData.value = await response.json();

    }catch(err){
        console.log(err)
    }
}

onMounted(loadData);

const emits = defineEmits('calculate')

async function calculateDscr(){
  try{
    const results = await fetch('http://localhost:5000/api/dscr/default', {
      method: 'POST',
      headers: {'Content-Type': 'application/json', 'X-Session-Id' : props.sessionId}
    })

    if (!response.ok) {
        const errorText = await response.text(); // Try to get server error details
        alert(`Server error: ${response.status} - ${errorText}`);
        return;
    }

    calculatedData = await results.json();

    emits('calculate', calculatedData)

  }catch (err){
    console.error("API Error:", err);
    alert(`API call failed: ${err.message}`);
  }

}



</script>

<template>
    <div class = "parent-container"  v-if = "savedData">
        <h2>Calculate Debt Payment</h2>
        <div class = "input-container">
            <SingeOutput v-if = "!savedData.annualDebtService" label = "Annual Debt Service" :value = "savedData.annualDebtService" :style = "'no-border'"/>
            <div v-else><h4>No Annual Debt Service Exist</h4></div>
            <SingeOutput label = "Annual Payment" :value = "savedData.annualProfit" :style = "'no-border'"/>
            <SingleInput label = "Preffered DSCR amount: " :inputType = "4" v-model = "prefferedDSCR"/>
        </div>
        
    </div>
    <div v-else class = "loading-container">
        <transition name="fade-slide" mode="out-in">
            <div>
                <div class="loader">
                    <span></span>
                    <span></span>
                    <span></span>
                </div>

                <p class="loading-text">Loading DSCR Calculator...</p>
            </div>
        </transition>
    </div>
</template>

<style scoped>

.parent-container {
    display: flex;
    flex-direction: column;
    box-sizing: border-box;
    width: 100%;
    height: 100%;
}

.input-container {
    display: flex;
    flex-direction: column;
    flex: 1;
    padding: 20px;
    gap: 22px;
    border-radius: 14px;
    box-sizing: border-box;
    background: rgba(255, 255, 255);
    border: 1px solid #e5e7eb;
    box-shadow: 0 6px 14px rgba(0,0,0,0.05);
}




.fade-slide-enter-active,
.fade-slide-leave-active {
  transition: all 0.4s ease;
}

.fade-slide-enter-from,
.fade-slide-leave-to {
  opacity: 0;
  transform: translateY(10px);
}

.loading-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 3rem;
  gap: 1rem;
}

.loader {
  display: flex;
  gap: 0.5rem;
}

.loader span {
  width: 12px;
  height: 12px;
  border-radius: 999px;
  background: #6366f1;
  animation: bounce 0.8s infinite ease-in-out;
}

.loader span:nth-child(2) {
  animation-delay: 0.15s;
}

.loader span:nth-child(3) {
  animation-delay: 0.3s;
}

.loading-text {
  font-size: 0.95rem;
  color: #94a3b8;
  letter-spacing: 0.05em;
  animation: pulse 1.5s infinite;
}

@keyframes bounce {
  0%, 80%, 100% {
    transform: scale(0.7);
    opacity: 0.5;
  }

  40% {
    transform: scale(1);
    opacity: 1;
  }
}

@keyframes pulse {
  0%, 100% {
    opacity: 0.5;
  }

  50% {
    opacity: 1;
  }
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
