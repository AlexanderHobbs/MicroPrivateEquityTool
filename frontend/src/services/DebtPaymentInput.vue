<script setup>

import {ref} from 'vue'
import SingleInput from '@/components/basic/SingleInput.vue';
import ToggleBtn from '@/components/basic/ToggleBtn.vue';

// ----------------------
// State
// ----------------------


const termOptions = ref([
    {text: "1 Year", value: 1},
    {text: "2 Year", value: 2},
    {text: "3 Year", value: 3},
    {text: "4 Year", value: 4},
    {text: "5 Year", value: 5},
    {text: "6 Year", value: 6},
    {text: "7 Year", value: 7},
    {text: "8 Year", value: 8},
    {text: "9 Year", value: 9},
    {text: "9+ Years", value: 10},
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
    Amount: null,
    InterestRate: null,
    Term: null
})


// ----------------------
// Action
// ----------------------

function calculateDownPayment(){

        const downPayment = null;

        if(!purchaseForm){
            alert("Enter Purchase Price and Equity Injection")
        }else{
            const percentage = SBA_MetricForm.value.equityInjection * 0.01
            downPayment = SBA_MetricForm.value.purchasePrice * percentage
        }

        return downPayment
}


// ----------------------
// Functions
// ----------------------

const emit = defineEmits(['save']);

function saveData() {
    
    if(autoCalculate){
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

    emit('save', debtData);
    alert("Loaded");

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
<body>
    <div class = "parent-container">
        <div class="db-input-container">
            <h2>Calculate Debt Payment</h2>

            <div class = "business-purchase-price">
                <SingleInput label = "Asking/Purchase Price: " v-model = "purchaseForm.purchasePrice"/>
                <SingleInput label = "Equity Injection: " v-model = "purchaseForm.equityInjection" placeholder = "0%"/>
            </div>

            <div class="SBA-loan-metrics">

                <div class = "SBA-loan-container">
                    <h3>SBA Loan Metrics</h3>
                    <SingleInput label = "Loan Amount/Portion: " v-model = "SBA_MetricForm.LoanAmount" />
                    <div class = "down-payment-div">
                        <SingleInput label = "Down Payment Amount: " style = "no-border" v-model = "SBA_MetricForm.DownPayment" />
                        <ToggleBtn v-model = "SBA_MetricForm.autoCalculate" label = "Auto Calculate?"/>
                    </div>

                    <SingleInput label = "Loan Interest Rate: " v-model = "SBA_MetricForm.InterestRate" placeholder = "0%"/>
                
                    <div class = "selected-year-input">
                        <label>Term Length:</label>
                        <select v-model="SBA_MetricForm.Term">
                            <option v-for="amount in termOptions" :key = "amount.value" :value = "amount.value">
                                {{ amount.text }}
                            </option>
                        </select>
                    </div>
                </div>

                <div class = "sellers-note-container">
                    <h3>Sellers Note Data</h3>
                    <SingleInput label = "Sellers Note Amount" :inputType = "2" v-model = "SellersNoteForm.Amount"/>
                    <SingleInput label = "Sellers Note Interest Rate" v-model = "SellersNoteForm.InterestRate"/>
                    <div class = "selected-year-input">
                        <label>Term Length:</label>
                        <select v-model="SellersNoteForm.Term">
                            <option v-for="amount in termOptions" :key = "amount.value" :value = "amount.value">
                                {{ amount.text }}
                            </option>
                        </select>
                    </div>
                </div>
            </div>
            
            <button class = "save-btn" @click = "saveData">Save Data</button>

        </div>
    </div>
</body>
</template>

<style scoped>

.parent-container {
    display: flex;
    box-sizing: border-box;
}

.db-input-container {
    display: flex;
    flex: 1.50;
    width: 100%;
    flex-direction: column;
    gap: 22px;
    box-sizing: border-box;
}

.business-purchase-price {
    display: flex;
    flex-direction: column;
    gap: 20px;
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
    gap: 40px;
    padding: 12px 14px;
    border-radius: 12px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 2px 6px rgba(0,0,0,0.04);
    align-items: center;
}

.down-payment-div label {
    font-size: 13px;
    font-weight: 500;
    color: #374151;
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
    padding: 12px 14px;
    border-radius: 12px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 2px 6px rgba(0,0,0,0.04);
    min-width: none;
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