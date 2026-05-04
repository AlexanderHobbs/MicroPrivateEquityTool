<script setup>
import {ref} from 'vue';

import DebtPaymentInput from '@/services/DebtPaymentInput.vue';
import SingleOutput from '@/components/basic/SingeOutput.vue';

const debtPaymentData = ref(null)
const successful = ref(false);

function saveDebtData(data){

    debtPaymentData.value = data;
    successful.value = true
}

</script>

<template>

<section class="parent">
    <div class="input">
        <DebtPaymentInput 
        @save = "saveDebtData"
        />
    </div>
    <div class = "output" v-if = "successful">
        <div class="year-card">
            <h3>Purchase / Equity</h3>
            <SingleOutput label="Purchase Price" :value="debtPaymentData?.Purchase.purchasePrice" />
            <SingleOutput label="Equity Injection" :value="debtPaymentData?.Purchase.equityInjection" />
        </div>
        <div class = "SBA-container">
            <div class="year-card">
                <h3>SBA Loan Metrics</h3>
                <SingleOutput label="Loan Amount" :value="debtPaymentData?.SBA_Metrics.LoanAmount" />
                <SingleOutput label="Down Payment" :value="debtPaymentData?.SBA_Metrics.DownPayment" />
                <SingleOutput label="Interest Rate" :value="debtPaymentData?.SBA_Metrics.InterestRate" />
                <SingleOutput label="Term Length" :value="debtPaymentData?.SBA_Metrics.Term" />
            </div>

            <div class="year-card">
                <h3>Sellers Note Data</h3>
                <SingleOutput label="Loan Amount" :value="debtPaymentData?.SellersNote.Amount" />
                <SingleOutput label="Interest Rate" :value="debtPaymentData?.SellersNote.InterestRate" />
                <SingleOutput label="Term Length" :value="debtPaymentData?.SellersNote.Term" />
            </div>
        </div>
    </div>
</section>

</template>

<style scoped>

.parent {
    display: flex;
    flex-direction: column;
    width: 100%;
    height: 100%;
    background: #f2f2f2;
    border-radius: 12px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 2px 6px rgba(0,0,0,0.04);
    box-sizing: border-box;
    padding: 40px;
}

.input{
    width: 100%;
    box-sizing: border-box;
}

.output {
    display: flex;
    flex-direction: column;
    width: 100%;
    gap: 10%;
    padding: 40px 20px;
    border-radius: 14px;
    box-sizing: border-box;
    background: rgba(255, 255, 255);
    border: 1px solid #e5e7eb;
    box-shadow: 0 6px 14px rgba(0,0,0,0.05);
    margin-top: 50px;
}


.year-card {
    display: flex;
    flex-direction: column;
    gap: 10px;
}

.SBA-container {
    display: inline-flex;
    flex-direction: row;
    gap: 40px;
    margin-bottom: 40px;
}


.SBA-container .year-card {
    flex: 1;
}


</style>