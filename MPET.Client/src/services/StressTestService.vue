<script setup>
import LoadingScreen from '@/components/basic/LoadingScreen.vue';
import STIcon from '../assets/loading-icons/stress-test.png'
import RangeInput from '@/components/basic/RangeInput.vue';
import SingeOutput from '@/components/basic/SingeOutput.vue';
import { watch, ref, onMounted } from 'vue';

const props = defineProps({sessionId: String})

watch(
  () => props.sessionId,
  (newId) => {
    if (newId) loadData()
  },
  { immediate: true }
)

const success = ref(true)

const replicatedData = ref({
  nerveLevel: 0,
  revenue: 0,
  expense: 0,
  revenueDrop: 0,
  marginLevelShift: 0,
  interestRateShift: 0
})

const showDebtPopup = ref(false);

const data = ref({
  adjustedProfit: 0,
  updatedDSCR: 0,
  updatedCashFlow: 0,
  suggestion: 'Awaiting calculation...'
});

async function loadData(){
  try{
    const response = await fetch("/api/stresstest/default", {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'X-Session-Id' : props.sessionId
      }
    });
    if (!response.ok) {
      alert(`Server error: ${response.status}`);
      return;
    }
    replicatedData.value = await response.json();
  }catch(err){
    console.error('Fetch failed:', err.name, err.message);
  }
}

const emit = defineEmits(['update', 'navigateTo'])

async function onInput(){
  try{
    const stData = { STModel: {...replicatedData.value} };
    console.log(stData);
    const response = await fetch("/api/stresstest/calculate", {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'X-Session-Id' : props.sessionId
      },
      body: JSON.stringify(stData)
    });
    if (!response.ok) {
      showDebtPopup.value = true;
      return;
    }
    data.value = await response.json();
    emit('update', data)
  }catch(err){
    console.error('Fetch failed:', err.name, err.message);
  }
}

const promptManualService = () => {
  const value = prompt("Please enter your Annual Debt Service amount:");
  if (value && value.trim() !== "") {
    showDebtPopup.value = false;
    console.log('User manually entered debt service:', value);
  }
};

const navigateToDebtPage = () => {
    showDebtPopup.value = false;
    emit('navigateTo', "debt");
};
</script>

<template>
  <div class="parent-container" v-if = "success">
    <div class="input-container">
      <RangeInput label="Nerve Levels" type="none" v-model="replicatedData.nerveLevel" :max="10" :min="0" @input = "onInput()"/>
      <RangeInput label="Revenue" v-model="replicatedData.revenue" @input = "onInput()"/>
      <RangeInput label="Expense" v-model="replicatedData.expense" @input = "onInput()"/>
      <RangeInput label="Revenue Drop (%)" :type="percent" v-model="replicatedData.revenueDrop" :max="100" :step="10" @input = "onInput()" />
      <RangeInput label="Margin Levels Shift" v-model="replicatedData.marginLevelShift" type="percent" :max="100" :step="10" @input = "onInput()"/>
      <RangeInput label="Interest Rate Change" :type="percent" v-model="replicatedData.interestRateShift" :max="20" :step=".1" @input = "onInput()"/>
    </div>

    <div class="output-container">
      <SingeOutput :style = "'st-output'" label="Adjusted Profit" :value = "data.adjustedProfit"/>
      <SingeOutput :style = "'st-output'" label="Updated DSCR" :value = "data.adjustedDSCR"/>
      <SingeOutput :style = "'st-output'" label="Updated cash Flow after Debt" :value = "data.cashFlow"/>
      <SingeOutput :style = "'st-output'" label="Suggestion" :value = "data.generatedSuggestion"/>
    </div>

    <!-- The Refined Popup Layout -->
    <div v-if="showDebtPopup" class="popup-overlay">
      <div class="popup-box">
        <h4 class="popup-title">Annual Debt Service Required</h4>
        <p class="popup-text">
          You must enter your <strong>Annual Debt Service</strong> details first before running a stress test calculation.
        </p>
        <div class="button-group">
          <button @click="promptManualService" class="btn-secondary">
            Enter Value Here
          </button>
          <button @click="navigateToDebtPage" class="btn-primary">
            Go to Debt Page
          </button>
        </div>
        <button @click="showDebtPopup = false" class="btn-cancel">
          Cancel
        </button>
      </div>
    </div>
  </div>
  <div v-else>
    <LoadingScreen :iconSrc = "STIcon"/>
  </div>
</template>

<style scoped>
/* Your App's Core Structural Layout */
.parent-container {
  flex: 1;
  display: flex;
  gap: 80px;
  box-sizing: border-box;
}
.output-container {
  display: flex;
  flex: 1;
  flex-direction: column;
  box-sizing: border-box;
  justify-content: space-between;
  z-index: 1;
}
.input-container {
  display: flex;
  flex: .5;
  flex-direction: column;
  box-sizing: border-box;
  justify-content: space-between;
}

/* Button Component Base Styling (Inheriting app parameters) */
button {
  cursor: pointer;
  font-size: 14px;
  box-sizing: border-box;
}

/* Redesigned Cohesive Popup Styles */
.popup-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.35); /* Softened background shade */
  z-index: 9999;
  display: flex;
  justify-content: center;
  align-items: center;
}

.popup-box {
  background: #ffffff;
  width: 420px;
  max-width: 90%;
  padding: 24px;
  border-radius: 4px; /* Clean flat layout matching standard dashboards */
  border: 1px solid #e2e8f0;
  box-shadow: 0 10px 25px -5px rgba(0, 0, 0, 0.05), 0 8px 10px -6px rgba(0, 0, 0, 0.05);
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.popup-title {
  margin: 0;
  font-size: 18px;
  font-weight: 600;
  color: #1e293b;
  text-align: left; /* Aligns with standard dashboard heading patterns */
}

.popup-text {
  margin: 0;
  color: #64748b;
  font-size: 14px;
  line-height: 1.5;
  text-align: left;
}

.button-group {
  display: flex;
  gap: 12px;
  width: 100%;
  margin-top: 8px;
}

.btn-primary {
  flex: 1;
  padding: 10px 16px;
  background: #0f172a; /* Dark gray/black theme to match classic control panels */
  color: #ffffff;
  border: none;
  border-radius: 4px;
  font-weight: 500;
}

.btn-secondary {
  flex: 1;
  padding: 10px 16px;
  background: #ffffff;
  color: #0f172a;
  border: 1px solid #e2e8f0;
  border-radius: 4px;
  font-weight: 500;
}

.btn-cancel {
  background: none;
  border: none;
  color: #94a3b8;
  align-self: center;
  font-size: 13px;
  padding: 4px;
}

/* Interactive States */
.btn-primary:hover { background: #1e293b; }
.btn-secondary:hover { background: #f8fafc; }
.btn-cancel:hover { color: #64748b; text-decoration: underline; }

.loading-container {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 100%;
  width: 100%;
}
</style>
