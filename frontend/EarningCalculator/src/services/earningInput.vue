<script setup>

import {ref, useId} from 'vue'

const selectedYear = ref()
const selectedYear_Options = ref([
    {text: '2025', value: 2025},
    {text: '2024', value: 2024},
    {text: '2023', value: 2023},
])

const revenueAmount = ref('')
const expenseAmount = ref('')
const SDE = ref('')
const ownerSalary = ref('')
const addBackExist = ref(false)

const AddBackList = ref([])

const AddBackForm= ref(
    {
        Id: useId(),
        description: "",
        price: "",
        category: "",
        confidenceLevel: ""
    }
)

async function add_AddBack() {
    try{
        AddBackList.value.push({
            ...AddBackForm.value
        })

        Object.assign(AddBackForm.value, {
            Id: 0,
            description: '',
            price: '',
            category: '',
            confidenceLevel:''
        })
    
    }catch (err){
        console.error(err);
    }
}

const EBITDAValuesExist = ref(false);

const InterestRate = ref('');
const Taxes = ref('');
const Depreciation = ref('');
const Amortization = ref('');

const emit = defineEmits(['save'])

function submitYear() {
    const yearData = {
        revenue: revenueAmount.value,
        expense: expenseAmount.value,
        SDE: SDE.value,
        ownerSalary: ownerSalary.value,
        addBacks: AddBackList.value,
        EBITDA: EBITDAValuesExist.value ? {
            interestRate: InterestRate.value,
            taxes: Taxes.value,
            depreciation: Depreciation.value,
            amortization: Amortization.value
        } : null,
    }

    emit('save', selectedYear.value, yearData);
}


const financialData = ref({
    revenue: '',
    expense: '',
    notes: '',
})

const fields = [
    {key: 'revenue', label: 'Revenue', type: 'number', placeholder: '0.00'},
    {key: 'expense', label: 'Expense', type: 'number', placeholder: '0.00'},
    {key: 'notes', label: 'Notes', type: 'textarea'}
]

</script>

<template>

<div class = "parent-container">

    <div class = "selected-year-input">
        <label for="SelectedYearInput">Select Fiscal Year: </label>
        <select name="SelectedYearInput" id="InputYear" v-model = "selectedYear">
            <option disabled value="">Select a Year</option>
            <option v-for = "year in selectedYear_Options" :key = "year.value" :value = "year.value">
                {{ year.text }}
            </option>
        </select>
        <p>Selected Year: {{ selectedYear }}</p>
    </div>

    <div class = "currency-input">
        <label for="revenue">Revenue Amount: </label>
        <input type="text" v-model = "revenueAmount" placeholder="00.00">
        <p>{{ revenueAmount }}</p>

    </div>
    <div class="currency-input">
        <label for="expense">Expense Amount: </label>
        <input type="text" v-model = "expenseAmount" placeholder = "00.00">
        <p>{{ expenseAmount }}</p>
    </div>

    <div class="currency-input">
        <label for="SDE">SDE: </label>
        <input type="text" v-model = "SDE" placeholder = "00.00">
        <p>{{ SDE }}</p>
    </div>

    <div class="currency-input">
        <label for="expense">Owner Salary: </label>
        <input type="text" v-model = "ownerSalary" placeholder = "00.00">
        <p>{{ ownerSalary }}</p>
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
            <input type="text" v-model = "AddBackForm.price">
            <label for="AddBack-Category">Add Back Category: </label>
            <input type="text" v-model = "AddBackForm.category">
            <label for = "AddBack-ConfidenceLevel">Add Back Confidence Level:</label>
            <input type="text" v-model = "AddBackForm.confidenceLevel">

            <button class = "addBack-btn" @click="add_AddBack()">Create Add Back</button>
        </div>

        <div class = "AddBackList" v-if = "AddBackList.length > 0">
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
                <tr v-for = "addBack in AddBackList">
                    <td>{{ addBack.description }}</td>
                    <td>{{ addBack.price }}</td>
                    <td>{{ addBack.category }}</td>
                    <td>{{ addBack.confidenceLevel }}</td>
                </tr>
                </tbody>
            </table>
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
            <label for="Interest">Interest Rate: </label>
            <input type = "text" v-model = "InterestRate">
            <label for="AddBack-Value">Taxes: </label>
            <input type="text" v-model = "Taxes">
            <label for="AddBack-Category">Depreciation: </label>
            <input type="text" v-model = "Depreciation">
            <label for = "AddBack-ConfidenceLevel">Amortization:</label>
            <input type="text" v-model = "Amortization">
    </div>

    <button class = "save-btn" @click = "submitYear">Save Data</button>

</div>

</template>


<style scoped>

.parent-container {
    display: flex;
    flex-direction: column;
    gap: 22px;
    padding: 20px;
    box-sizing: border-box;
}

/* Shared input row style */
.selected-year-input,
.currency-input {
    display: flex;
    align-items: center;
    gap: 16px;
    background: #ffffff;
    padding: 12px 14px;
    border-radius: 12px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 2px 6px rgba(0,0,0,0.04);
}

/* Label consistency */
.selected-year-input label,
.currency-input label {
    font-size: 13px;
    font-weight: 500;
    color: #374151;
    min-width: 140px;
}

/* Inputs */
input, select, textarea {
    padding: 8px 10px;
    border-radius: 8px;
    border: 1px solid #d1d5db;
    outline: none;
    font-size: 13px;
    background: #fff;
}

input:focus, select:focus, textarea:focus {
    border-color: #60a5fa;
    box-shadow: 0 0 0 3px rgba(96,165,250,0.2);
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



</style>