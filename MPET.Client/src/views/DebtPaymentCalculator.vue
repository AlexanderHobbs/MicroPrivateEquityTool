<script setup>
import {ref} from 'vue';

import DebtPaymentInput from '@/services/DebtPaymentInput.vue';
import SingleOutput from '@/components/basic/SingeOutput.vue';

const debtPaymentData = ref(null)
const calculationResults = ref(null)
const successful = ref(false);

function saveDebtData(data){
    debtPaymentData.value = data;
    successful.value = true
}

function DebtDataResults(savedData){
    calculationResults.value = savedData;
    successful.value = true
}

</script>

<template>

<section class="parent">
    <div class="input">
        <DebtPaymentInput 
        @save = "saveDebtData"
        @results = "DebtDataResults"
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
                <div style = "display: flex; flex-direction: column; justify-content: space-between; height: 100%;">
                    <SingleOutput label="Loan Amount" :value="debtPaymentData?.SellersNote.LoanAmount" />
                    <SingleOutput label="Interest Rate" :value="debtPaymentData?.SellersNote.InterestRate" />
                    <SingleOutput label="Term Length" :value="debtPaymentData?.SellersNote.Term" />
                </div>
            </div>
        </div>
    </div>

    <div class="calculation-results" v-if="successful">

        <div>
            <h2>Loan Summary</h2>
            <SingleOutput label="SBA Annual Payment"    :value="calculationResults.sbA_AnnualPayment" />
            <SingleOutput label="Seller Annual Payment" :value="calculationResults.seller_AnnualPayment" />
            <SingleOutput label="Annual Debt Service"   :value="calculationResults.annualDebtService" />
            <SingleOutput label="Total Interest Paid"   :value="calculationResults.totalInterestPaid" />
            <SingleOutput label="Total Debt Burden"     :value="calculationResults.totalDebtBurden" />
        </div>

        <div>
            <h2>SBA Amortization Schedule</h2>
            <div class="table-header">
                <span>Year</span>
                <span>Beginning Balance</span>
                <span>Interest Payment</span>
                <span>Principal Payment</span>
                <span>Ending Balance</span>
            </div>
        
        
            <div v-for="item in calculationResults.amortizationSchedule" :key="item.termYear" class="table-row">
                <SingleOutput label="Year"              :value="item.termYear" />
                <SingleOutput label="Beginning Balance" :value="item.beginningBalance" />
                <SingleOutput label="Interest Payment"  :value="item.interestPayment" />
                <SingleOutput label="Principal Payment" :value="item.principalPayment" />
                <SingleOutput label="Ending Balance"    :value="item.endingBalance" />
            </div>
        </div>

        <div>
            <h2>Yearly Debt Payments</h2>
            <div v-for="item in calculationResults.yearlyDebtPayments" :key="item.year" class="table-row">
                <SingleOutput label="Year"           :value="item.year" />
                <SingleOutput label="SBA Payment"    :value="item.sbA_Payment" />
                <SingleOutput label="Seller Payment" :value="item.seller_Payment" />
                <SingleOutput label="Total Payment"  :value="item.totalPayment" />
            </div>
        </div>

        <div>
            <h2>Remaining Balances</h2>
            <div v-for="item in calculationResults.remainingBalances" :key="item.year" class="table-row">
                <SingleOutput label="Year"           :value="item.year" />
                <SingleOutput label="SBA Balance"    :value="item.sbA_Balance" />
                <SingleOutput label="Seller Balance" :value="item.seller_Balance" />
                <SingleOutput label="Total Balance"  :value="item.totalBalance" />
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

.calculation-results {
  font-family: 'Inter', sans-serif;
  padding: 2rem;
  display: flex;
  flex-direction: column;
  gap: 2.5rem;
  background: #f8f7f4;
  min-height: 100%;
}

/* Section headings */
h2 {
  font-size: 13px;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  color: #888780;
  margin-bottom: 1rem;
  padding-bottom: 0.5rem;
  border-bottom: 0.5px solid #d3d1c7;
}

/* Loan Summary — metric card grid */
.calculation-results > div:first-child {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(160px, 1fr));
  gap: 10px;
  align-items: start;
}

.calculation-results > div:first-child h2 {
  grid-column: 1 / -1;
}

/* Table sections — scrollable by year */
.table-header {
  display: grid;
  grid-template-columns: 60px 1fr 1fr 1fr 1fr;
  gap: 0;
  padding: 6px 1rem;
  background: #f1efe8;
  border-radius: 6px 6px 0 0;
  border: 0.5px solid #d3d1c7;
  border-bottom: none;
}

.table-header span {
  font-size: 11px;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.07em;
  color: #888780;
}

.table-row {
  display: grid;
  grid-template-columns: 60px 1fr 1fr 1fr 1fr;
  gap: 0;
  padding: 0 1rem;
  border: 0.5px solid #d3d1c7;
  border-top: none;
  background: #ffffff;
  transition: background 0.12s;
}

.table-row:last-child {
  border-radius: 0 0 6px 6px;
}

.table-row:hover {
  background: #f8f7f4;
}

/* Yearly debt payments — 4-column */
.calculation-results > div:nth-child(3) .table-row {
  grid-template-columns: 60px 1fr 1fr 1fr;
}

/* Remaining balances — 4-column */
.calculation-results > div:nth-child(4) .table-row {
  grid-template-columns: 60px 1fr 1fr 1fr;
}

/* Scroll container for table sections */
.calculation-results > div:not(:first-child) {
  background: #ffffff;
  border-radius: 10px;
  padding: 1.25rem;
  border: 0.5px solid #d3d1c7;
  overflow-x: auto;
}

.calculation-results > div:not(:first-child) .table-row,
.calculation-results > div:not(:first-child) .table-header {
  min-width: 560px;
}

/* Year scroll — limit height and enable vertical scroll */
.calculation-results > div:not(:first-child) {
  max-height: 420px;
  overflow-y: auto;
  overflow-x: auto;
  scroll-behavior: smooth;
}

/* Keep heading pinned above scroll */
.calculation-results > div:not(:first-child) h2 {
  position: sticky;
  top: 0;
  background: #ffffff;
  z-index: 1;
  margin: -1.25rem -1.25rem 1rem -1.25rem;
  padding: 1rem 1.25rem 0.75rem;
  border-radius: 10px 10px 0 0;
}

.calculation-results > div:not(:first-child) .table-header {
  position: sticky;
  top: 52px;
  z-index: 1;
}

/* Scrollbar styling */
.calculation-results > div:not(:first-child)::-webkit-scrollbar {
  width: 5px;
  height: 5px;
}

.calculation-results > div:not(:first-child)::-webkit-scrollbar-track {
  background: #f1efe8;
  border-radius: 10px;
}

.calculation-results > div:not(:first-child)::-webkit-scrollbar-thumb {
  background: #b4b2a9;
  border-radius: 10px;
}

.calculation-results > div:not(:first-child)::-webkit-scrollbar-thumb:hover {
  background: #888780;
}

</style>


