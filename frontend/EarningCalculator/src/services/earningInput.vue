<script setup>

import {ref, useId} from 'vue'
import CurrencyInput from '@/components/CurrencyInput.vue'
import CurrencyOutput from '@/components/CurrencyOutput.vue'
import EBITDAInput from '@/components/EBITDAInput.vue'
import EBITDAOutput from '@/components/EBITDAOutput.vue'
import { errorMessages } from 'vue/compiler-sfc'

const selectedYear = ref()
const selectedYear_Options = ref([
    {text: '2025', value: 2025},
    {text: '2024', value: 2024},
    {text: '2023', value: 2023},
])

const currencyForm = ref({
    revenue: null,
    expense: null,
    ownerSalary: null,
    ReportedSDE: null
})

const updatedValues = ref(null);

function updateCurrencyValues() {
    try{
        updatedValues.value = () => structuredClone(currencyForm.value)
    }catch(err){
        errorMessages.value = (err)
    }
    
    Object.keys(currencyForm.value).forEach(key => {
        currencyForm.value[key] = null;
    })


}


const addBackExist = ref(false)

const AddBackList = ref([])

const AddBackForm= ref(
    {
        id: useId(),
        description: "",
        price: null,
        category: "",
        confidenceLevel: null
    }
)

async function add_AddBack() {
    try{
        AddBackList.value.push({
            ...AddBackForm.value
        })

        Object.assign(AddBackForm.value, {
            id: null,
            description: '',
            price: '',
            category: '',
            confidenceLevel: 50
        })
    
    }catch (err){
        console.error(err);
    }
}

const EBITDAValuesExist = ref(false);

const EBITDAForm = ref({
    InterestRate: null,
    Taxes: null,
    Depreciation: null,
    Amortization: null
})

const emit = defineEmits(['save'])

function submitYear() {
    const yearData = {
        operating: {
            ...currencyForm.value
        },

        adjustments: {
            addBacks: AddBackList.value
        },

        financials: {
            ...EBITDAForm.value
        }
    }

    emit('save', selectedYear.value, yearData);
}

</script>

<template>
<body>
    <div class = "parent-container">
        <div class = "ea-input-container">
            <h1>Earning Calculation</h1>
            <div class = "selected-year-input">
                <label for="SelectedYearInput">Select Fiscal Year: </label>
                <select name="SelectedYearInput" id="InputYear" v-model = "selectedYear">
                    <option disabled value="">Select a Year</option>
                    <option v-for = "year in selectedYear_Options" :key = "year.value" :value = "year.value">
                        {{ year.text }}
                    </option>
                </select>
            </div>

            <div class = "currency-input-form">
                <CurrencyInput  label = "Revenue Amount: " v-model = "currencyForm.revenue"/>

                <CurrencyInput  label = "Expense Amount:" v-model = "currencyForm.expense" />

                <CurrencyInput label = "Reported SDE:" v-model = "currencyForm.ReportedSDE" />
                
                <CurrencyInput label = "Owner Salary:" v-model = "currencyForm.ownerSalary" />

                <button @click="updateCurrencyValues">Submit Values</button>
                
            </div>

            <div class = "radio-btn-class">
                <label for="hasAddBacks?">Owner Add Backs Exist for year {{ selectedYear }}?: </label>
                <input type="radio" :value ="true" v-model = "addBackExist">
                <label for="yes">Yes </label>
                <input type="radio" :value = "false" v-model = "addBackExist">
                <label for="no">No </label>
            </div>

            <div class = "OwnerAddBacks" v-if = "addBackExist">

                <div class = "addBackEntry">
                    <h4>Add claimed add backs (as many as needed): </h4>
                    <label for="AddBack-Description">Add Back Description: </label>
                    <textarea v-model = "AddBackForm.description" placeholder="description..."></textarea>
                    <label for="AddBack-Value">Add Back Value: </label>
                    <input type="number" v-model.number = "AddBackForm.price">
                    <label for="AddBack-Category">Add Back Category: </label>
                    <input type="text" v-model = "AddBackForm.category">
                    <label for = "AddBack-ConfidenceLevel">Add Back Confidence Level:</label>
                    <input type="range" v-model.number = "AddBackForm.confidenceLevel" min = "0" max = "100" step = "1">
                    <span class = "confidence-value">{{ AddBackForm.confidenceLevel }}%</span>

                    <button class = "addBack-btn" @click="add_AddBack()">Create Add Back</button>
                </div>

            </div>

            <div class="radio-btn-class">
                <label for="hasEBITDA?">Do EBITDA Records Exist for year {{ selectedYear }}?</label>
                <input type="radio" :value = "true" v-model = "EBITDAValuesExist">
                <label for="yes-rd-btn">Yes</label>
                <input type="radio" :value = "false" v-model = "EBITDAValuesExist">
                <label for="no-rd-btn">No</label>
            </div>

            <div class="EBITDA" v-if = "EBITDAValuesExist">
                    <h4>Add EBITDA Values: </h4>

                    <EBITDAInput label = "Interest Rate" v-model = "EBITDAForm.InterestRate"/>

                    <EBITDAInput label = "Taxes" v-model = "EBITDAForm.Taxes"/>

                    <EBITDAInput label = "Depreciation" v-model = "EBITDAForm.Depreciation"/>

                    <EBITDAInput label = "Amortization" v-model = "EBITDAForm.Amortization"/>

            </div>

            <button class = "save-btn" @click = "submitYear">Save Data</button>

        </div>

        <div class = "ea-output-container">

            <div class = "output-container"><CurrencyOutput :data = "currencyForm" :label = "selectedYear"/></div>

            <div class="output-container">
                <div class = "AddBackList">
                        <h4>Add Backs</h4>
                        <table class = "Table">
                            <thead>
                                <tr>
                                    <th>Description</th>
                                    <th>Price</th>
                                    <th>Category</th>
                                    <th>Confidence Level</th>
                                </tr>
                            </thead>
                            <tbody>
                            <tr v-for = "addBack in AddBackList" :key = "addBack.id">
                                <td>{{ addBack.description }}</td>
                                <td>{{ addBack.price }}</td>
                                <td>{{ addBack.category }}</td>
                                <td>{{ addBack.confidenceLevel }}</td>
                            </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

                <div class = "output-container">
                    <EBITDAOutput :data = "EBITDAForm"/>
                </div>
        </div>
    </div>
</body>

</template>


<style scoped>

.parent-container {
    display: flex;
}

.ea-output-container {
    flex: 1;
    display: flex;
    flex-direction: column;
    gap: 22px;
    background: #ffffff;
    padding: 20px;
    border-radius: 14px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 6px 14px rgba(0,0,0,0.05);
}

.ea-input-container {
    display: flex;
    flex: 1.50;
    flex-direction: column;
    gap: 22px;
    padding: 20px;
    box-sizing: border-box;
}

/* Shared input row style */
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

/* Label consistency */
.selected-year-input label {
    font-size: 13px;
    font-weight: 500;
    color: #374151;
    min-width: 140px;
}

.currency-input-form {
    display: flex;
    flex-direction: column;
    gap: 22px;
}

/* Inputs */
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

/* AddBack section container */
.OwnerAddBacks {
    display: flex;
    gap: 24px;
    background: #ffffff;
    padding: 18px;
    border-radius: 14px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 6px 14px rgba(0,0,0,0.05);
    box-sizing: border-box;
}

/* Left form */
.addBackEntry {
    display: flex;
    flex-direction: column;
    gap: 10px;
    flex: 1;
}

/* Table container */
.AddBackList {
    flex: 1;
    margin-top: 0;
}

/* Table styling */
.Table {
    width: 100%;
    border-collapse: collapse;
    font-size: 12px;
}

.Table th {
    text-align: left;
    padding: 8px;
    background: #f3f4f6;
    color: #374151;
    font-weight: 600;
    border-bottom: 1px solid #e5e7eb;
}

.Table td {
    padding: 8px;
    border-bottom: 1px solid #f1f1f1;
    color: #111827;
}

/* EBITDA section styled like a card */
.EBITDA {
    display: flex;
    flex-direction: column;
    gap: 10px;
    background: #ffffff;
    padding: 18px;
    border-radius: 14px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 6px 14px rgba(0,0,0,0.05);
}

/* Button styling */
.save-btn, .addBack-btn {
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

.save-btn:hover, .addBack-btn:hover {
    background: #1f2937;
    transform: translateY(-1px);
}

.save-btn:active, .addBack-btn:active {
    transform: translateY(0);
}

.confidence-value {
    font-size: 12px;
    color: #6b7280;
}


</style>