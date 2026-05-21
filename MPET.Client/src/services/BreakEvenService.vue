<script setup>
import LoadingScreen from '@/components/basic/LoadingScreen.vue';
import breakEvenIcon from '../assets/loading-icons/break-even.png'
import SingeOutput from '@/components/basic/SingeOutput.vue';
import SingleInput from '@/components/basic/SingleInput.vue';
import {ref, computed, watch} from 'vue'

const props = defineProps({sessionId: String});

const success = ref(false);
const debtData = ref();

const earningData = ref();

const breakEvenForm = ref({
    DebtService: 0,
    CurrentRevenue: 0,
    FixedCost: 0,
    VariableCost: 0
});

let nextId = 0;

const fixedCostItems = ref([]);

const totalFixedCost = computed(() =>
    fixedCostItems.value.reduce((sum, item) => sum + Number(item.amount), 0)
)

function addFixedCost(){
    fixedCostItems.value.push({ id: nextId++, label: '', amount: 0 })
}

function removeFixedCost(data){
    fixedCostItems.value = fixedCostItems.value.filter(item => item.id !== data)
}

async function loadData() {

    try{
        
        debtData.value = await fetchJson("/api/central/debt", props.sessionId);
        success.value = true;
        // earningData.value = await fetchJson("/api/central/earning", props.sessionId);

    }catch(err){
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

async function calculate(){

    const BEData = {
        BreakEvenModel: {...breakEvenForm}
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
        const text = await response.text()
        throw new Error(`${response.status}: ${text}`)
    }

    const data = await response.json();
    emit('results', data);

}

watch(
    () => props.sessionId,
    (newId) => {
        if (newId) loadData()
    },
    { immediate: true }
)


</script>

<template>
    <div class = "parent-container" v-if = "success">
        <h2>Conduct a Break Even Analysis</h2>
        <div class = "input-container">
            <!-- <SingeOutput label = "Gross Revenue for most Current Year" :value = "earningData.annualDebtService" /> -->
            <SingeOutput label = "Monthly Debt Service: " :value = "debtData.annualDebtService" />

            <div class = "fixed-cost-container">
            <h4>Fixed Cost</h4>
            <button @click="addFixedCost">+ Add Fixed Cost</button>
            <div
                v-for="item in fixedCostItems"
                :key="item.id"
                class="fixed-cost"
            >
                <div>
                    <input v-model="item.label"  placeholder="Description" />
                    <input v-model="item.amount" placeholder="Amount" type="number" />
                </div>
                <button @click="removeFixedCost(item.id)">−</button>
            </div>
            <p>Total Fixed Costs: {{ totalFixedCost }}</p>
            </div>

            <div>
            <h4>Variable Cost %</h4>
            <SingleInput label = "COGS"/>
            <button @click = "calculate">See Break Even Analysis</button>
            </div>
        </div>
    </div>
    <div v-else class="loading-container">
      <LoadingScreen :iconSrc = "breakEvenIcon"/>
    </div>
</template>

<style scoped>


.fixed-cost {
    display: flex;
    gap: 20%;
}

.fixed-cost :nth-child(1) {
    flex: 1;
    display: flex;
}

.loading-container {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 100%;
  width: 100%;
}

</style>