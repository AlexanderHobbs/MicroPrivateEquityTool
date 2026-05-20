<script setup>

import {watch, ref, onActivated} from 'vue'
import SingleInput from '@/components/basic/SingleInput.vue';
import SingeOutput from '@/components/basic/SingeOutput.vue';

const props = defineProps({sessionId: String});

const savedData = ref(null);

watch(
    () => props.sessionId,
    (newId) => {
        if (newId) loadData()
    },
    { immediate: true }
)

onActivated(() => {
    loadData();
});

const dscrForm = ref({
    prefferedDSCR: 1,
    annualDebtService: 0,
    annualProfit: 0,
})

async function loadData() {
    try{

        const response = await fetch('/api/dscr/default', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json', 
                'X-Session-Id' : props.sessionId
            },
        })

        if (!response.ok) {
            const errorText = await response.text(); // Try to get server error details
            alert(`Server error: ${response.status} - ${errorText}`);
            return;
        }

        savedData.value = await response.json();

        // console.log(savedData.value);

        dscrForm.value.annualDebtService = savedData.value.annualDebtService;
        dscrForm.value.annualProfit = savedData.value.annualProfit;

    }catch(err){
      console.error('Fetch failed:', err.name, err.message);
    }
}

const emits = defineEmits(['calculate'])

async function calculateDscr(){
  try{

    const calculatedData = {
        DSCR: {...dscrForm.value}
    }

    // console.log(calculatedData)

    const response = await fetch('/api/dscr/calculate', {
      method: 'POST',
      headers: {'Content-Type': 'application/json', 
      'X-Session-Id' : props.sessionId
      },
      body: JSON.stringify(calculatedData)
    })

    if (!response.ok) {
        const errorText = await response.text(); // Try to get server error details
        alert(`Server error: ${response.status} - ${errorText}`);
        return;
    }

    const data = await response.json();

    emits('calculate', data)

  }catch (err){
    console.error('Fetch failed:', err.name, err.message);
  }

}



</script>

<template>
    <div class = "parent-container"  v-if = "savedData">
        <h2>Calculate DSCR</h2>
        <div class = "input-container">
          <div class = "prev-input">
            <SingeOutput label = "Annual Debt Service: " :value = "savedData.annualDebtService" :style = "'no-border'"/>
            <SingeOutput label = "Annual Profit: " :value = "savedData.annualProfit" :style = "'no-border'"/>
          </div>
            <SingleInput label = "Preffered DSCR amount: " :inputType = "4" v-model = "dscrForm.prefferedDSCR"/>
        </div>
        <div>
          <button class = "save-btn" @click="calculateDscr">Calculate DSCR</button>
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
    gap: 22px;
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

.prev-input {
  display: flex;
  flex-direction: column;
  width: 50%;
  gap: 12px;
}


.save-btn {
    width: fit-content;
    padding: 10px 14px;
    border-radius: 10px;
    border: none;
    background: #111827;
    color: #ffffff;
    font-size: 13px;
    cursor: pointer;
    transition: all 0.15s ease;
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
