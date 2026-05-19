<script setup>

import {ref, watch} from 'vue'
import SingleInput from '@/components/basic/SingleInput.vue';
import ToggleBtn from '@/components/basic/ToggleBtn.vue';

// ----------------------
// State
// ----------------------

const props = defineProps({sessionId: String});

watch(
    () => props.sessionId,
    (newId) => {
        if (newId) loadData()
    },
    { immediate: true }
)

const savedData = ref(null);


const termOptions = ref([
    {text: "1 Year", value: 1},
    {text: "2 Years", value: 2},
    {text: "3 Years", value: 3},
    {text: "4 Years", value: 4},
    {text: "5 Years", value: 5},
    {text: "6 Years", value: 6},
    {text: "7 Years", value: 7},
    {text: "8 Years", value: 8},
    {text: "9 Years", value: 9},
    {text: "10 Years", value: 10},
    {text: "11 Years", value: 11},
    {text: "12 Years", value: 12},
    {text: "13 Years", value: 13},
    {text: "14 Years", value: 14},
    {text: "15 Years", value: 15},
    {text: "16 Years", value: 16},
    {text: "17 Years", value: 17},
    {text: "18 Years", value: 18},
    {text: "19 Years", value: 19},
    {text: "20 Years", value: 20},
    {text: "21 Years", value: 21},
    {text: "22 Years", value: 22},
    {text: "23 Years", value: 23},
    {text: "24 Years", value: 24},
    {text: "25 Years", value: 25},
    {text: "26 Years", value: 26},
    {text: "27 Years", value: 27},
    {text: "28 Years", value: 28},
    {text: "29 Years", value: 29},
    {text: "30 Years", value: 30},
])

const purchaseForm = ref({
    purchasePrice: null,
    equityInjection: null
})

const SBA_MetricForm = ref({
    LoanAmount: null,
    DownPayment: null,
    InterestRate: null,
    Term: null,
    autoCalculate: false
})

const SellersNoteForm = ref({
    LoanAmount: null,
    InterestRate: null,
    Term: null
})


// ----------------------
// Action
// ----------------------

function calculateDownPayment(){

        let downPayment = null;

        if(!purchaseForm){
            alert("Enter Purchase Price and Equity Injection")
        }else{
            const percentage = purchaseForm.value.equityInjection * 0.01
            downPayment = purchaseForm.value.purchasePrice * percentage
        }

        return downPayment
}


// ----------------------
// Functions
// ----------------------

function autoFillMarketValues() {
    const price = purchaseForm.value.purchasePrice ?? 500_000;
 
    const equityInjectionPct = 10;                               // 10% SBA minimum
    const equityInjection    = Math.round(price * 0.10);
    const sbaLoanAmount      = price - equityInjection;
    const sellerNoteAmount   = Math.round(price * 0.10);         // market convention
 
    purchaseForm.value.purchasePrice   = price;
    purchaseForm.value.equityInjection = equityInjectionPct;     // stored as % for calculateDownPayment()
 
    SBA_MetricForm.value.LoanAmount    = sbaLoanAmount;
    SBA_MetricForm.value.DownPayment   = equityInjection;
    SBA_MetricForm.value.InterestRate  = 11.25;                  // WSJ Prime + spread, Jun 2025
    SBA_MetricForm.value.Term          = 10;                     // 10-year SBA acquisition max
    SBA_MetricForm.value.autoCalculate = true;
 
    SellersNoteForm.value.LoanAmount   = sellerNoteAmount;
    SellersNoteForm.value.InterestRate = 6.00;                   // typical subordinated seller note
    SellersNoteForm.value.Term         = 5;                      // 5-yr SBA standby requirement
}

async function loadData() {
    
    try{

        const response = await fetch('/api/debt/default', {
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

        setValues(savedData);

    }catch(err){
        console.error('Fetch failed:', err.name, err.message);
    }
}

function setValues(data) {

    purchaseForm.value.purchasePrice   = data.purchase?.PurchasePrice ?? null;
    purchaseForm.value.equityInjection = data.purchase?.EquityInjection ?? null;

    SBA_MetricForm.value.LoanAmount    = data.sBA_Metrics?.LoanAmount ?? null;
    SBA_MetricForm.value.DownPayment   = data.sBA_Metrics?.DownPayment ?? null;
    SBA_MetricForm.value.InterestRate  = data.sBA_Metrics?.InterestRate ?? null;
    SBA_MetricForm.value.Term          = data.sBA_Metrics?.Term ?? null;
    SBA_MetricForm.value.autoCalculate = data.sBA_Metrics?.IsAutocalculated ?? false;


    SellersNoteForm.LoanAmount   = data.sellersNote?.LoanAmount ?? null;
    SellersNoteForm.InterestRate = data.sellersNote?.InterestRate ?? null;
    SellersNoteForm.Term         = data.sellersNote?.Term ?? null;
}

const emit = defineEmits(['save', 'results']);

async function calculateData() {
    
    if(SBA_MetricForm.autoCalculate){
        SBA_MetricForm.value.DownPayment = calculateDownPayment().toFixed(2);
    }

    const debtData = {
        Purchase: {...purchaseForm.value },
        SBA_Metrics: {...SBA_MetricForm.value},
        SellersNote: {...SellersNoteForm.value}
    }

    if(!dataIsNotNull(debtData)){
        alert("Not all fiels have data entered — Please fill in all entries!");
        return;
    }

    try{
        
        const response = await fetch('/api/debt/calculate', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json', 'X-Session-Id' : props.sessionId
            },
            body: JSON.stringify(debtData)
        });


        if (!response.ok) {
            const errorText = await response.text(); // Try to get server error details
            alert(`Server error: ${response.status} - ${errorText}`);
            return;
        }

        const data = await response.json();
        emit('results', data);

        //save data to pinia
        emit('save', debtData);


    }catch(err){
        console.error('Fetch failed:', err.name, err.message);
    }

}

// ----------------------
// Helper
// ----------------------

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
    <div class = "parent-container">
        <div class="db-input-container">
            <h2>Calculate Debt Payment</h2>

            <div class="business-purchase-price">
                <SingleInput 
                    label="Asking/Purchase Price: " 
                    v-model="purchaseForm.purchasePrice" 
                    :placeholder="purchaseForm.purchasePrice ?? '00.00'"/>
                <SingleInput 
                    label="Equity Injection: " 
                    v-model="purchaseForm.equityInjection" 
                    :placeholder="purchaseForm.equityInjection ?? '0%'"/>
            </div>

            <div class="SBA-loan-metrics">

                <div class="SBA-loan-container">
                    <h3>SBA Loan Metrics</h3>
                    <SingleInput 
                        label="Loan Amount/Portion: " 
                        v-model="SBA_MetricForm.LoanAmount"
                        :placeholder="SBA_MetricForm.LoanAmount ?? '0'"/>
                    <div class="down-payment-div">
                        <SingleInput 
                            label="Down Payment Amount: " 
                            v-model="SBA_MetricForm.DownPayment"
                            :placeholder="SBA_MetricForm.DownPayment ?? '0'"/>
                        <ToggleBtn v-model="SBA_MetricForm.autoCalculate" label="Auto Calculate:"/>
                    </div>

                    <SingleInput 
                        label="Loan Interest Rate: " 
                        v-model="SBA_MetricForm.InterestRate" 
                        :placeholder="SBA_MetricForm.InterestRate ?? '0%'"/>

                    <div class="selected-year-input">
                        <label>Term Length:</label>
                        <select v-model="SBA_MetricForm.Term">
                            <option disabled value="">{{ SBA_MetricForm.Term ?? 'Select a term' }}</option>
                            <option v-for="amount in termOptions" :key="amount.value" :value="amount.value">
                                {{ amount.text }}
                            </option>
                        </select>
                    </div>
                </div>

                <div class="sellers-note-container">
                    <h3>Sellers Note Data</h3>
                    <SingleInput 
                        label="Sellers Note Amount" 
                        v-model="SellersNoteForm.LoanAmount"
                        :placeholder="SellersNoteForm.LoanAmount ?? '0'"/>
                    <SingleInput 
                        label="Sellers Note Interest Rate" 
                        v-model="SellersNoteForm.InterestRate"
                        :placeholder="SellersNoteForm.InterestRate ?? '0'"/>
                    <div class="selected-year-input">
                        <label>Term Length:</label>
                        <select v-model="SellersNoteForm.Term">
                            <option disabled value="">{{ SellersNoteForm.Term ?? 'Select a term' }}</option>
                            <option v-for="amount in termOptions" :key="amount.value" :value="amount.value">
                                {{ amount.text }}
                            </option>
                        </select>
                    </div>
                </div>
            </div>

            <button class="save-btn" @click="calculateData">Calculate Debt Payment</button>
            <button class = "save-btn" @click = "autoFillMarketValues">Auto Fill</button>
        </div>
    </div>
</template>

<style scoped>

.parent-container {
    display: flex;
    box-sizing: border-box;
}

.db-input-container {
    display: flex;
    flex: 1.50;
    flex-direction: column;
    gap: 22px;
    box-sizing: border-box;
}

.business-purchase-price {
    display: flex;
    flex-direction: column;
    padding: 20px;
    gap: 22px;
    border-radius: 14px;
    box-sizing: border-box;
    background: rgba(255, 255, 255);
    border: 1px solid #e5e7eb;
    box-shadow: 0 6px 14px rgba(0,0,0,0.05);
}

.SBA-loan-metrics {
    display: flex;
    flex-direction: row;
    width: 100%;
    gap: 10%;
    padding: 20px;
    border-radius: 14px;
    box-sizing: border-box;
    background: rgba(255, 255, 255);
    border: 1px solid #e5e7eb;
    box-shadow: 0 6px 14px rgba(0,0,0,0.05);
}

.SBA-loan-container {
    flex: 1;
    display: flex;
    flex-direction: column;
    box-sizing: border-box;
    gap: 22px;
}

.down-payment-div {
    display: flex;
    flex-direction: column;
}

.sellers-note-container {
    flex: 1;
    display: flex;
    flex-direction: column;
    box-sizing: border-box;
    gap: 22px;
}

.selected-year-input {
    display: flex;
    align-items: center;
    gap: 16px;
    background: #ffffff;
    min-width: none;
    padding: 14px 0px;
    
}

.selected-year-input select {
    flex: 2;
    border-radius: 6px;
    border: 1px solid #e6e8ec;
    outline: none;
}

.selected-year-input label {
    flex: 1;
    font-size: 16px;
    font-weight: 500;
    color: #6b7280;
}

input, select, textarea {
    padding: 10px 12px;
    border-radius: 8px;
    border: 1px solid transparent;
    box-sizing: border-box;
    outline: none;
    font-size: 13px;
    background: #f9fafb;
    transition: all 0.2s ease;
    font-family: 'Arial', sans-serif;
}

input:hover, select:hover, textarea:hover {
    background: #f3f4f6;
}

input:focus, select:focus, textarea:focus {
    background: #ffffff;
    border-color: #111827;
    box-shadow: 0 0 0 2px rgba(0,0,0,0.05);
}

label {
    font-size: 13px;
    font-weight: 500;
    color: #374151;
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

.save-btn:hover {
    background: #1f2937;
    transform: translateY(-1px);
}

.save-btn:active {
    transform: translateY(0);
}


</style>

<!-- 
Goal: 
Ask for SBA Loan Metrics in an accessible and efficient way with an ephasis on ease of use

Input:
I. Ask for SBA loan metrics
    1. SBA Interest Rate, 
    2. Term (years), 
    3. seller note and interest rate
    4. Loan Amount
    5. Down Payment
II. Purchase price
III. equity injection %
-->