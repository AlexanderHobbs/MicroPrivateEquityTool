<script setup>
import EarningInput from '@/services/EarningCalculatorInput.vue';

import {ref} from 'vue';

// ----------------------
// State
// ----------------------


const yearlyData = ref({})
const selectedYearData = ref(null)


// ----------------------
// Actions
// ----------------------

function saveYearData(payload) {
    if(!payload?.year) return
    
    yearlyData.value[payload.year] = payload
}

function handleLoadYear(year){
    selectedYearData.value = yearlyData.value[year] || null
    alert("function called")
}

</script>

<template>

    <div class = "parent">

        <div class = "input">
            <EarningInput 
                @save = "saveYearData"
                @load-year = "handleLoadYear"
                :initialData="selectedYearData"
            />
            <div class="year-grid">
                <div v-for="(entry, year) in yearlyData" :key="year" class="year-card">

                    <div class="year-header">
                    <h2>{{ year }}</h2>
                    </div>

                    <div>
                        <h3>Operating</h3>
                        <div class="metrics">
                            <div class="metric">
                                <span class="label">Revenue</span>
                                <span class="value">{{ entry.operating.revenue }}</span>
                            </div>

                            <div class="metric">
                                <span class="label">Expense</span>
                                <span class="value">{{ entry.operating.expense }}</span>
                            </div>

                            <div class="metric">
                                <span class="label">SDE</span>
                                <span class="value">{{ entry.operating.ReportedSDE }}</span>
                            </div>

                            <div class="metric">
                                <span class="label">Owner Salary</span>
                                <span class="value">{{ entry.operating.ownerSalary }}</span>
                            </div>
                        </div>
                    </div>


                    <div>
                        <h3>Adjustments</h3>
                        
                        <div class="section" v-if = "entry.adjustments.addBacks.length">
                            <div v-for="addBack in entry.adjustments.addBacks" :key="addBack.id" class="sub-table">
                                <div class = "metric">
                                    <span class = "label">Description:</span>
                                    <span class = "value">{{ addBack.description }}</span>
                                </div>
                                <div class = "metric">
                                    <span class = "label">Value: </span>
                                    <span class = "value">{{ addBack.price }}</span>
                                </div>
                                <div class = "metric">
                                    <span class = "label">Category: </span>
                                    <span class = "value">{{ addBack.category }}</span>
                                </div>
                                <div class = "metric">
                                    <span class = "label">Confidence Level:</span>
                                    <span class = "value">{{ addBack.confidenceLevel }}</span>
                                </div>
                            </div>
                        </div>
                        <div v-else>
                            <div class = "metric">
                                <span class = "label">No adjustment recorded:</span>
                            </div>
                        </div>
                    </div>

                    <div>
                        <h3>Financials</h3>
                        <div class="metrics">
                            <div class = "metric">
                                <span class = "label">Interest:</span>
                                <span class = "value">{{ entry.financials.InterestRate }}</span>
                            </div>
                            <div class = "metric">
                                <span class = "label">Taxes:</span>
                                <span class = "value">{{ entry.financials.Taxes }}</span>
                            </div>
                            <div class = "metric">
                                <span class = "label">Depreciation:</span>
                                <span class = "value">{{ entry.financials.Depreciation }}</span>
                            </div>
                            <div class = "metric">
                                <span class = "label">Amortization:</span>
                                <span class = "value">{{ entry.financials.Amortization }}</span>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        

    </div>

    
</template>

<style scoped>

.parent {
    display: flex;
    width: 100%;
    height: 100%;
    background: #f2f2f2;
    padding: 12px 14px;
    border-radius: 12px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 2px 6px rgba(0,0,0,0.04);
    box-sizing: border-box;
}

.input{
    width: 100%;
    display: flex;
    flex-direction: column;
}

.year-grid {
    display: grid;
    grid-template-rows: repeat(auto-fit, minmax(320px, 1fr));
    gap: 20px;
    padding: 20px;
}

.year-card {
    background: #ffffff;
    border-radius: 14px;
    padding: 18px;
    box-shadow: 0 8px 20px rgba(0,0,0,0.06);
    border: 1px solid #e8e8e8;
    transition: transform 0.15s ease, box-shadow 0.15s ease;
}

.year-card:hover {
    transform: translateY(-2px);
    box-shadow: 0 12px 26px rgba(0,0,0,0.10);
}

.year-header {
    border-bottom: 1px solid #eee;
    margin-bottom: 12px;
    padding-bottom: 8px;
}

.year-header h2 {
    margin: 0;
    font-size: 18px;
    font-weight: 600;
    color: #1f2937;
}

.metrics {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 10px;
    margin-bottom: 15px;
}

.metric {
    background: #f9fafb;
    padding: 10px;
    border-radius: 10px;
    display: flex;
    justify-content: space-between;
    font-size: 13px;
}

.metric .label {
    color: #6b7280;
    font-weight: 500;
}

.metric .value {
    color: #111827;
    font-weight: 600;
}

.section {
    gap: 20px;
    margin-bottom: 15px;
}

.section h3 {
    font-size: 14px;
    margin-bottom: 8px;
    color: #374151;
}

.sub-table {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 6px;
    margin-bottom: 20px;
}


.ebitda-grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 8px;
    font-size: 12px;
    background: #f9fafb;
    padding: 10px;
    border-radius: 10px;
}




</style>