<script setup>

import {ref} from 'vue'
import SingleInput from '@/components/basic/SingleInput.vue';

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

const purchasePrice = ref()
const equityInjection = ref()

const SBA_MetricForm = ref({
    LoanAmount: null,
    DownPayment: null,
    InterestRate: null,
    Term: null,
})

const SellersNoteForm = ref({
    Amount: null,
    InterestRate: null,
    Term: null
})



// ----------------------
// Action
// ----------------------

// ----------------------
// Helper
// ----------------------

// ----------------------
// Derived
// ----------------------

// ----------------------
// Functions
// ----------------------


</script>

<template>
<body>
    <div class = "parent-container">
        <div class="db-input-container">
            <h2>Calculate Debt Payment</h2>

            <div class = "business-purchase-price">
                <SingleInput label = "Asking/Purchase Price: " v-model = "purchasePrice"/>
                <SingleInput label = "Equity Injection: " v-model = "equityInjection"/>
            </div>

            <div class="SBA-loan-metrics">

                <div class = "SBA-loan-container">
                    <h3>SBA Loan Metrics</h3>
                    <SingleInput label = "Loan Amount/Portion: " v-model = "SBA_MetricForm.LoanAmount" />
                    <div>
                        <SingleInput label = "Down Payment Amount: " style = "no-border" v-model = "SBA_MetricForm.DownPayment" />
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


        </div>
    </div>
</body>
</template>

<style scoped>

.parent-container {
    display: flex;
    box-sizing: border-box;
    padding-right: 20px;
}

.db-input-container {
    display: flex;
    flex: 1.50;
    width: 100%;
    flex-direction: column;
    gap: 22px;
    padding: 20px;
    box-sizing: border-box;
    
}

.SBA-loan-metrics {
    display: flex;
    flex-direction: row;
    width: 100%;
    gap: 10%;
    padding: 20px;
    border-radius: 14px;
    background: rgba(255, 255, 255);
    border: 1px solid #e5e7eb;
    box-shadow: 0 6px 14px rgba(0,0,0,0.05);
}

.SBA-loan-container {
    flex: 1;
    display: flex;
    flex-direction: column;
    gap: 22px;
    
}

.sellers-note-container {
    flex: 1;
    display: flex;
    flex-direction: column;
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