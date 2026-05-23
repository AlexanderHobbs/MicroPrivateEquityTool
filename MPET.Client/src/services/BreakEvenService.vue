<script setup>
import LoadingScreen from '@/components/basic/LoadingScreen.vue';
import breakEvenIcon from '../assets/loading-icons/break-even.png'
import SingeOutput from '@/components/basic/SingeOutput.vue';
import SingleInput from '@/components/basic/SingleInput.vue';
import {ref, computed, watch} from 'vue'


// ---------------------
// Props & Ref Variables
// ---------------------

const props = defineProps({sessionId: String});

const success = ref(false);

const debtData = ref();


watch(
    () => props.sessionId,
    (newId) => {
        if (newId) loadData()
    },
    { immediate: true }
)


const earningData = ref();



// ---------------------
// Fixed Cost List
// ---------------------

const fixedCostItems = ref([]);
let nextFCId = 0;

function addFixedCost(){
    fixedCostItems.value.push({ id: nextFCId++, label: '', amount: 0 })
}

function removeFixedCost(data){
    fixedCostItems.value = fixedCostItems.value.filter(item => item.id !== data)
}

const totalFixedCost = computed(() =>
    fixedCostItems.value.reduce((sum, item) => sum + Number(item.amount), 0)
)

// ---------------------
// Variable Cost List
// ---------------------
const variableCostItems = ref([]);
let nextVId = 0;

function addVarCost(){
    variableCostItems.value.push({id: nextVId++, label: '', amount: 0})
}

function removeVarCost(data){
    variableCostItems.value = variableCostItems.value.filter(item => item.id !== data)
}

const totalVariableCost = computed(() =>
    variableCostItems.value.reduce((sum, item) => sum + Number(item.amount), 0)
)


const breakEvenForm = ref({
    debtService: 8000,
    currentRevenue: 75000,
    fixedCost: totalFixedCost || 0,
    variableCost: totalVariableCost || 0
});



// ---------------------
// HttpGet Methods
// ---------------------
async function loadData() {

    try{
        
        debtData.value = await fetchJson("/api/central/debt", props.sessionId);
        success.value = true;
        // earningData.value = await fetchJson("/api/central/earning", props.sessionId);

        // breakEvenForm.value.DebtService = debtData.value.annualDebtService;

    }catch(err){
        success.value = false;
        console.error('Fetch failed:', err.name, err.message);
    }

}

async function fetchJson(url, sessionId) {
    const response = await fetch(url, {
        method: 'GET',
        headers: {                          // headers were missing the 'headers' key
            'Content-Type': 'application/json',
            'X-Session-Id': sessionId
        }
    })
    if (!response.ok) {
        const text = await response.text()
        throw new Error(`${response.status}: ${text}`)
    }
    return response.json()
}


// ---------------------
// HttpPost Methods
// ---------------------
const emit = defineEmits(['calculate', 'revenue']);

async function calculate(){
    try{
        const BEData = {
            BreakEvenModel: {...breakEvenForm.value}
        }

        const response = await fetch("/api/breakeven/calculate", {
            method: 'POST',
            headers: {
                'Content-Type' : 'application/json',
                'X-Session-Id' : props.sessionId
            },
            body: JSON.stringify(BEData)
        })

        if (!response.ok) {
            success.value = false;
            const text = await response.text()
            throw new Error(`${response.status}: ${text}`)
        }

        const data = await response.json();
        emit('calculate', data);
        emit('revenue', breakEvenForm.currentRevenue);
    }catch(err){
        success.value = false;
        console.error('Fetch failed:', err.name, err.message);
    }

}

// ---------------------
// Helper Functions
// ---------------------
function formatCurrency(val) {
  if (val == null || val === '') return '—'
  return '$' + Number(val).toLocaleString('en-US', { minimumFractionDigits: 0 })
}


</script>

<template>
  <div class="parent-container" v-if="success">
    <div class="db-input-container">
 
      <div class="business-purchase-price">
        <div class="section-header">
          <h3 class="section-title">Break Even Analysis</h3>
        </div>
 
        <div class="read-only-row">
            <SingeOutput label = "Gross revenue (current year)" :value = "formatCurrency(breakEvenForm.currentRevenue)" :style = "'no-border'"/>
            <SingeOutput label = "Monthly debt service" :value = "formatCurrency(breakEvenForm.debtService)" :style = "'no-border'"/>
        </div>
      </div>
 
      <div class="SBA-loan-metrics">
        <div class="SBA-loan-container">
          <div class="cost-section-header">
            <h4 class="cost-title">Fixed costs</h4>
            <button class="add-btn" @click="addFixedCost()">+ Add</button>
          </div>
 
          <div v-for="item in fixedCostItems" :key="item.id" class="cost-row">
            <div class="cost-inputs">
              <input v-model="item.label" placeholder="Description" />
              <input v-model="item.amount" placeholder="0.00" type="number" class="amount-input" />
            </div>
            <button class="remove-btn" @click="removeFixedCost(item.id)" aria-label="Remove">&#10005;</button>
          </div>
 
          <div class="cost-total">
            <span>Total fixed costs</span>
            <span class="total-value">{{ formatCurrency(totalFixedCost) }}</span>
          </div>
        </div>
 
        <div class="sellers-note-container">
          <div class="cost-section-header">
            <h4 class="cost-title">Variable costs</h4>
            <button class="add-btn" @click="addVarCost()">+ Add</button>
          </div>
 
          <div
            v-for="item in variableCostItems"
            :key="item.id"
            class="cost-row"
          >
            <div class="cost-inputs">
              <input v-model="item.label" placeholder="Description" />
              <input v-model="item.amount" placeholder="0" type="number" class="amount-input" />
            </div>
            <button class="remove-btn" @click="removeVarCost(item.id)" aria-label="Remove">&#10005;</button>
          </div>
 
          <div class="cost-total">
            <span>Total variable costs</span>
            <span class="total-value">${{ totalVariableCost }}</span>
          </div>
        </div>
      </div>
 
      <button class="save-btn" @click="calculate">See break even analysis</button>
    </div>
  </div>
 
  <div v-else class="loading-container">
    <LoadingScreen :iconSrc="breakEvenIcon" />
  </div>
</template>

<style scoped>
.parent-container {
  display: flex;
  box-sizing: border-box;
}
 
.db-input-container {
  display: flex;
  flex: 1.5;
  flex-direction: column;
  gap: 22px;
  box-sizing: border-box;
}
 
.business-purchase-price {
  display: flex;
  flex-direction: column;
  padding: 20px;
  gap: 16px;
  border-radius: 14px;
  box-sizing: border-box;
  background: #ffffff;
  border: 1px solid #e5e7eb;
  box-shadow: 0 6px 14px rgba(0, 0, 0, 0.05);
}
 
.section-header {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 4px;
}

.section-title {
    margin: 0;
}

 
.read-only-row {
  display: flex;
  flex-direction: column;
  gap: 12px;
}
 
.read-only-row:last-child {
  border-bottom: none;
}
 
.ro-label {
  font-size: 13px;
  color: #6b7280;
  font-weight: 500;
}
 
.ro-value {
  font-size: 13px;
  font-weight: 600;
  color: #111827;
}
 
.SBA-loan-metrics {
  display: flex;
  flex-direction: row;
  width: 100%;
  gap: 10%;
  padding: 20px;
  border-radius: 14px;
  box-sizing: border-box;
  background: #ffffff;
  border: 1px solid #e5e7eb;
  box-shadow: 0 6px 14px rgba(0, 0, 0, 0.05);
}
 
.SBA-loan-container,
.sellers-note-container {
  flex: 1;
  display: flex;
  flex-direction: column;
  box-sizing: border-box;
  gap: 12px;
}
 
.cost-section-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 4px;
}
 
.cost-title {
  font-size: 13px;
  font-weight: 600;
  color: #111827;
  margin: 0;
}
 
.add-btn {
  padding: 5px 10px;
  border-radius: 6px;
  border: 1px solid #e5e7eb;
  background: #f9fafb;
  color: #374151;
  font-size: 12px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.15s ease;
}
 
.add-btn:hover {
  background: #f3f4f6;
  border-color: #d1d5db;
}
 
.cost-row {
  display: flex;
  align-items: center;
  gap: 8px;
}
 
.cost-inputs {
  flex: 1;
  display: flex;
  gap: 8px;
}
 
.cost-inputs input {
  flex: 1;
  padding: 9px 11px;
  border-radius: 8px;
  border: 1px solid transparent;
  box-sizing: border-box;
  outline: none;
  font-size: 13px;
  background: #f9fafb;
  transition: all 0.2s ease;
  font-family: 'Arial', sans-serif;
}
 
.cost-inputs input:hover {
  background: #f3f4f6;
}
 
.cost-inputs input:focus {
  background: #ffffff;
  border-color: #111827;
  box-shadow: 0 0 0 2px rgba(0, 0, 0, 0.05);
}
 
.amount-input {
  max-width: 90px;
}
 
.remove-btn {
  width: 28px;
  height: 28px;
  border-radius: 6px;
  border: 1px solid #e5e7eb;
  background: transparent;
  color: #9ca3af;
  font-size: 11px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.15s ease;
  flex-shrink: 0;
}
 
.remove-btn:hover {
  background: #fef2f2;
  border-color: #fca5a5;
  color: #ef4444;
}
 
.cost-total {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 12px;
  background: #f9fafb;
  border-radius: 8px;
  margin-top: 4px;
}
 
.cost-total span {
  font-size: 12px;
  color: #6b7280;
  font-weight: 500;
}
 
.total-value {
  font-size: 13px !important;
  font-weight: 600 !important;
  color: #111827 !important;
}
 
.save-btn {
  width: fit-content;
  padding: 10px 14px;
  border-radius: 10px;
  border: none;
  background: #111827;
  color: #ffffff;
  font-size: 13px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.15s ease;
}
 
.save-btn:hover {
  background: #1f2937;
  transform: translateY(-1px);
}
 
.save-btn:active {
  transform: translateY(0);
}
 
.loading-container {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 100%;
  width: 100%;
}
</style>